using Mouse_Avoider.Source.Core;

namespace Mouse_Avoider.Source.Entities;

public class Collectible : GameObject
{
	//Variables
	private const string _textureName = "collectible";

	public Collectible(GameObject parent, string name = "Collectible") : base(parent, name)
	{
		TextureName = _textureName;
	}
}