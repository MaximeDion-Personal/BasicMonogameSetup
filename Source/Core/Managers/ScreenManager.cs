using System.Collections.Generic;

namespace Mouse_Avoider.Source.Core.Managers;

public class ScreenManager
{
	public static void Initialize()
	{
		Instance.initialize();
	}

	public void initialize()
	{
		StartingScreen.Initialize();
	}

	public static void ChangeScreen(GameScreen screen)
	{
		CurrentScreen = screen;
	}

	public static void RegisterScreen(GameScreen screen)
	{
		if (Screens is null)
		{
			Screens = new List<GameScreen>();
		}

		Screens.Add(screen);
	}

	public static void SetStartingScreen(GameScreen screen)
	{
		if (StartingScreen is not null)
		{
			throw new System.Exception("Starting screen already set.");
		}

		StartingScreen = screen;
		CurrentScreen = screen;

		CurrentScreen.Initialize();
	}

	public static GameScreen CurrentScreen { get; private set; }

	public static GameScreen StartingScreen { get; private set; }

	public static List<GameScreen> Screens { get; private set; }

	// instance of the ScreenManager
	private static ScreenManager instance;

	public static ScreenManager Instance
	{
		get
		{
			if (instance == null)
				instance = new ScreenManager();
			return instance;
		}
	}
}