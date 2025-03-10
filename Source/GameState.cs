namespace Mouse_Avoider.Source;

public static class GameState
{
	public static int CurrentScore { get; set; }
	public static int HighScore { get; set; }

	// enum of all possible game states
	public enum State
	{
		Menu,
		Playing,
		GameOver
	}
}