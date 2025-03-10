using Mouse_Avoider.Source.Core;
using Mouse_Avoider.Source.Core.Managers;
using Mouse_Avoider.Source.Screens;

namespace Mouse_Avoider;

public static class Initializer
{
	public static void Initialize()
	{
		DrawManager.Initialize(Main.GraphicsManager);
		GameWorld.InitializeWorld();

		var mainScreen = new MainScreen(GameWorld.Instance);
		ScreenManager.RegisterScreen(mainScreen);
		ScreenManager.SetStartingScreen(mainScreen);

		GameWorld.Instance.Children.Add(mainScreen);
	}
}