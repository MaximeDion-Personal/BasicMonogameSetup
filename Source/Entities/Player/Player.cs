using Mouse_Avoider.Source.Core;

namespace Mouse_Avoider.Source.Entities;

public class Player : GameObject
{
	//Variables
	private const string _textureName = "player";

	public Player(GameObject parent, string name = "Player") : base(parent, name)
	{
		TextureName = _textureName;
	}
}