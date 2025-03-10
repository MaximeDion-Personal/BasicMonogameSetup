using Mouse_Avoider.Source.Core;

namespace Mouse_Avoider.Source.Entities;

public class Sniper : GameObject
{
	//Variables
	private const string _textureName = "orb";

	public Sniper(GameObject parent, string name = "Sniper") : base(parent, name)
	{
		TextureName = _textureName;
	}
}