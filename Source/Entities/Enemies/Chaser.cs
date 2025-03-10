using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Mouse_Avoider.Source.Core;

namespace Mouse_Avoider.Source.Entities;

public class Chaser : GameObject
{
	//Variables
	private const string _textureName = "orb";

	public Chaser(GameObject parent, string name = "Chaser") : base(parent, name)
	{
		TextureName = _textureName;
	}

	public override void Update(GameTime gameTime)
	{
		var mouse = Mouse.GetState();
		var target = new Vector2(mouse.X, mouse.Y);
		var diff = target - Position;

		Direction = (float)Math.Atan2(diff.Y, diff.X);

		base.Update(gameTime);
	}
}