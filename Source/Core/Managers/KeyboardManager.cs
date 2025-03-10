using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Mouse_Avoider.Source.Core.Managers
{
	public class KeyboardManager
	{
		private KeyboardState _currentState;
		private KeyboardState _previousState;

		public void Update(GameTime gameTime)
		{
			_previousState = _currentState;
			_currentState = Keyboard.GetState();
		}

		public bool IsKeyDown(Keys key) => _currentState.IsKeyDown(key);

		public bool IsKeyUp(Keys key) => _currentState.IsKeyUp(key);

		public bool IsKeyPressed(Keys key) => _currentState.IsKeyDown(key) && _previousState.IsKeyUp(key);

		public bool IsKeyReleased(Keys key) => _currentState.IsKeyUp(key) && _previousState.IsKeyDown(key);
	}
}