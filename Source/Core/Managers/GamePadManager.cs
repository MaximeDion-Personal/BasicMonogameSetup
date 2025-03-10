using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mouse_Avoider.Source.Core.Managers
{
	public class GamePadManager
	{
		// Private dead zone values for each thumbstick.
		private float _leftDeadZone = 0.25f;

		private float _rightDeadZone = 0.25f;

		/// <summary>
		/// Gets or sets the deadzone threshold for the left thumbstick.
		/// </summary>
		public float LeftDeadZone
		{
			get => _leftDeadZone;
			set => _leftDeadZone = value;
		}

		/// <summary>
		/// Gets or sets the deadzone threshold for the right thumbstick.
		/// </summary>
		public float RightDeadZone
		{
			get => _rightDeadZone;
			set => _rightDeadZone = value;
		}

		/// <summary>
		/// Sets the deadzone threshold for both thumbsticks.
		/// </summary>
		public void SetDeadZoneForBoth(float value)
		{
			_leftDeadZone = value;
			_rightDeadZone = value;
		}

		// Configurable thresholds for smash detection on the left thumbstick.
		private float _leftNeutralThreshold = 0.2f;

		private float _leftSmashThreshold = 0.8f;

		/// <summary>
		/// Gets or sets the neutral threshold for left thumbstick smash detection.
		/// </summary>
		public float LeftNeutralThreshold
		{
			get => _leftNeutralThreshold;
			set => _leftNeutralThreshold = value;
		}

		/// <summary>
		/// Gets or sets the smash threshold for left thumbstick smash detection.
		/// </summary>
		public float LeftSmashThreshold
		{
			get => _leftSmashThreshold;
			set => _leftSmashThreshold = value;
		}

		// Configurable thresholds for smash detection on the right thumbstick.
		private float _rightNeutralThreshold = 0.2f;

		private float _rightSmashThreshold = 0.8f;

		/// <summary>
		/// Gets or sets the neutral threshold for right thumbstick smash detection.
		/// </summary>
		public float RightNeutralThreshold
		{
			get => _rightNeutralThreshold;
			set => _rightNeutralThreshold = value;
		}

		/// <summary>
		/// Gets or sets the smash threshold for right thumbstick smash detection.
		/// </summary>
		public float RightSmashThreshold
		{
			get => _rightSmashThreshold;
			set => _rightSmashThreshold = value;
		}

		private GamePadState[] _currentStates = new GamePadState[GamePad.MaximumGamePadCount];
		private GamePadState[] _previousStates = new GamePadState[GamePad.MaximumGamePadCount];

		// List of player indices with connected gamepads.
		private List<PlayerIndex> _connectedGamePads = new List<PlayerIndex>();

		public void Update(GameTime gameTime)
		{
			_connectedGamePads.Clear();
			for (int i = 0; i < GamePad.MaximumGamePadCount; i++)
			{
				PlayerIndex index = (PlayerIndex)i;
				_previousStates[i] = _currentStates[i];
				_currentStates[i] = GamePad.GetState(index);
				if (_currentStates[i].IsConnected)
				{
					_connectedGamePads.Add(index);
				}
			}
		}

		public bool IsButtonDown(Buttons button, PlayerIndex playerIndex = PlayerIndex.One)
		{
			return _currentStates[(int)playerIndex].IsButtonDown(button);
		}

		public bool IsButtonPressed(Buttons button, PlayerIndex playerIndex = PlayerIndex.One)
		{
			return _currentStates[(int)playerIndex].IsButtonDown(button) && _previousStates[(int)playerIndex].IsButtonUp(button);
		}

		public bool IsButtonReleased(Buttons button, PlayerIndex playerIndex = PlayerIndex.One)
		{
			return _currentStates[(int)playerIndex].IsButtonUp(button) && _previousStates[(int)playerIndex].IsButtonDown(button);
		}

		public Vector2 GetLeftThumbstick(PlayerIndex playerIndex = PlayerIndex.One)
		{
			Vector2 thumb = _currentStates[(int)playerIndex].ThumbSticks.Left;
			return thumb.Length() < _leftDeadZone ? Vector2.Zero : thumb;
		}

		public Vector2 GetRightThumbstick(PlayerIndex playerIndex = PlayerIndex.One)
		{
			Vector2 thumb = _currentStates[(int)playerIndex].ThumbSticks.Right;
			return thumb.Length() < _rightDeadZone ? Vector2.Zero : thumb;
		}

		public NormalizedDirection GetNormalizedLeftThumbstick(DirectionMode mode, PlayerIndex playerIndex = PlayerIndex.One)
		{
			Vector2 thumb = _currentStates[(int)playerIndex].ThumbSticks.Left;
			return InputHelper.NormalizeStickDirection(thumb, mode);
		}

		public NormalizedDirection GetNormalizedRightThumbstick(DirectionMode mode, PlayerIndex playerIndex = PlayerIndex.One)
		{
			Vector2 thumb = _currentStates[(int)playerIndex].ThumbSticks.Right;
			return InputHelper.NormalizeStickDirection(thumb, mode);
		}

		/// <summary>
		/// Checks if the left thumbstick is being smashed. This means that in one frame, it transitions
		/// from below the neutral threshold to at or above the smash threshold.
		/// </summary>
		/// <param name="playerIndex">Player index to check.</param>
		/// <returns>True if a smash is detected for the left thumbstick.</returns>
		public bool IsLeftThumbSmashing(PlayerIndex playerIndex = PlayerIndex.One)
		{
			// Retrieve the previous and current left thumbstick values.
			Vector2 previousInput = _previousStates[(int)playerIndex].ThumbSticks.Left;
			Vector2 currentInput = _currentStates[(int)playerIndex].ThumbSticks.Left;

			// The smash event fires if the previous input was below the neutral threshold
			// and the current input is at or above the smash threshold.
			return previousInput.Length() < _leftNeutralThreshold && currentInput.Length() >= _leftSmashThreshold;
		}

		/// <summary>
		/// Checks if the right thumbstick is being smashed. This means that in one frame, it transitions
		/// from below the neutral threshold to at or above the smash threshold.
		/// </summary>
		/// <param name="playerIndex">Player index to check.</param>
		/// <returns>True if a smash is detected for the right thumbstick.</returns>
		public bool IsRightThumbSmashing(PlayerIndex playerIndex = PlayerIndex.One)
		{
			// Retrieve the previous and current right thumbstick values.
			Vector2 previousInput = _previousStates[(int)playerIndex].ThumbSticks.Right;
			Vector2 currentInput = _currentStates[(int)playerIndex].ThumbSticks.Right;

			// The smash event fires if the previous input was below the neutral threshold
			// and the current input is at or above the smash threshold.
			return previousInput.Length() < _rightNeutralThreshold && currentInput.Length() >= _rightSmashThreshold;
		}

		public float GetLeftTrigger(PlayerIndex playerIndex = PlayerIndex.One) => _currentStates[(int)playerIndex].Triggers.Left;

		public float GetRightTrigger(PlayerIndex playerIndex = PlayerIndex.One) => _currentStates[(int)playerIndex].Triggers.Right;

		public bool IsConnected(PlayerIndex playerIndex = PlayerIndex.One)
		{
			return _currentStates[(int)playerIndex].IsConnected;
		}

		public List<PlayerIndex> GetConnectedGamePads()
		{
			return new List<PlayerIndex>(_connectedGamePads);
		}
	}
}