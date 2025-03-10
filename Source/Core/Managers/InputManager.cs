using Microsoft.Xna.Framework;

namespace Mouse_Avoider.Source.Core.Managers
{
	/// <summary>
	/// Facade that aggregates keyboard, mouse, and gamepad input managers.
	/// Call InputManager.Update() once per frame.
	/// </summary>
	public static class InputManager
	{
		public static KeyboardManager Keyboard { get; } = new KeyboardManager();
		public static MouseManager Mouse { get; } = new MouseManager();
		public static GamePadManager GamePad { get; } = new GamePadManager();

		public static void Update(GameTime gameTime)
		{
			Keyboard.Update(gameTime);
			Mouse.Update(gameTime);
			GamePad.Update(gameTime);
		}
	}
}