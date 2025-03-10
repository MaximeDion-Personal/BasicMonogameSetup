using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mouse_Avoider.Source.Core.Managers;

public class DrawManager
{
	public static GraphicsDeviceManager Graphics;
	public static SpriteBatch SpriteBatch { get; private set; }

	//instance of the DrawManager
	private static DrawManager instance;

	public static DrawManager Instance
	{
		get
		{
			if (instance == null)
				instance = new DrawManager();
			return instance;
		}
	}

	public static void Initialize(GraphicsDeviceManager graphics)
	{
		SpriteBatch = new SpriteBatch(graphics.GraphicsDevice);
		Graphics = graphics;
	}

	public void Draw(GameTime gameTime)
	{
		Graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

		SpriteBatch.Begin();

		GameWorld.DrawWorld(gameTime);

		SpriteBatch.End();
	}

	// screen width
	public static int ScreenWidth
	{
		get { return Graphics.PreferredBackBufferWidth; }
	}

	// screen height
	public static int ScreenHeight
	{
		get { return Graphics.PreferredBackBufferHeight; }
	}
}