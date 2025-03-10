using Microsoft.Xna.Framework;

namespace Mouse_Avoider.Source.Core;

public class GameWorld : GameObject
{
	private static GameWorld instance;

	public GameWorld(GameObject parent = null, string name = "World") : base(parent, name)
	{
	}

	public static GameWorld Instance
	{
		get
		{
			if (instance == null)
				instance = new GameWorld();
			return instance;
		}
	}

	public static void InitializeWorld()
	{
		Instance.Initialize();
	}

	public static void UpdateWorld(GameTime gameTime)
	{
		Instance.Update(gameTime);
	}

	public static void DrawWorld(GameTime gameTime)
	{
		Instance.Draw(gameTime);
	}
}