using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using Mouse_Avoider.Source.Core;
using Mouse_Avoider.Source.Core.Managers;

namespace Mouse_Avoider;

public class Main : Game
{
	public static GraphicsDeviceManager GraphicsManager;
	public static ContentManager ContentManager;
	public static Main Instance;

	public Main()
	{
		GraphicsManager = new GraphicsDeviceManager(this);
		ContentManager = Content;
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
		Instance = this;
	}

	protected override void Initialize()
	{
		Initializer.Initialize();
		base.Initialize();
	}

	protected override void LoadContent()
	{
		// Should I have a loadContentWorld also?
		//GameWorld.LoadContentWorld(gameTime);
	}

	protected override void Update(GameTime gameTime)
	{
		InputManager.Update(gameTime);
		GameWorld.UpdateWorld(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		DrawManager.Instance.Draw(gameTime);
		base.Draw(gameTime);
	}

	public static void ExitGame()
	{
		Instance.Exit();
	}
}