using System;

using Microsoft.Xna.Framework;

using Mouse_Avoider.Source.Core;
using Mouse_Avoider.Source.Core.Managers;

namespace Mouse_Avoider.Source.Entities;

public class Bouncer : GameObject
{
	//Variables
	private const string _textureName = "orb";

	private Random _random = new Random();

	public Bouncer(GameObject parent, string name = "Bouncer") : base(parent, name)
	{
		TextureName = _textureName;
		Direction = GetRandomDirection();
	}

	public override void Update(GameTime gameTime)
	{
		// Bounce horizontally
		if (Position.X + Width > DrawManager.ScreenWidth ||
			Position.X < 0)
		{
			var velocity = Velocity;
			velocity.X *= -1;
			Velocity = velocity;
		}

		// Bounce vertically
		if (Position.Y + Height > DrawManager.ScreenHeight ||
			Position.Y < 0)
		{
			var velocity = Velocity;
			velocity.Y *= -1;
			Velocity = velocity;
		}

		base.Update(gameTime);
	}

	private float GetRandomDirection()
	{
		var angle = (float)(_random.NextDouble() * MathHelper.TwoPi);
		return angle;
	}
}