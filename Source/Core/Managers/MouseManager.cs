using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Mouse_Avoider.Source.Core.Managers
{
	public class MouseManager
	{
		private MouseState _currentState;
		private MouseState _previousState;

		public void Update(GameTime gameTime)
		{
			_previousState = _currentState;
			_currentState = Mouse.GetState();
		}

		public bool IsLeftButtonDown() => _currentState.LeftButton == ButtonState.Pressed;

		public bool IsLeftButtonPressed() => _currentState.LeftButton == ButtonState.Pressed && _previousState.LeftButton == ButtonState.Released;

		public bool IsLeftButtonReleased() => _currentState.LeftButton == ButtonState.Released && _previousState.LeftButton == ButtonState.Pressed;

		public bool IsRightButtonDown() => _currentState.RightButton == ButtonState.Pressed;

		public bool IsRightButtonPressed() => _currentState.RightButton == ButtonState.Pressed && _previousState.RightButton == ButtonState.Released;

		public bool IsRightButtonReleased() => _currentState.RightButton == ButtonState.Released && _previousState.RightButton == ButtonState.Pressed;

		public Vector2 GetPosition() => new Vector2(_currentState.X, _currentState.Y);
	}
}