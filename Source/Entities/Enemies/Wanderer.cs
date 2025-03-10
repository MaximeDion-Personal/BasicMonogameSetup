using System;

using Microsoft.Xna.Framework;

using Mouse_Avoider.Source.Core;
using Mouse_Avoider.Source.Core.Managers;

namespace Mouse_Avoider.Source.Entities;

public class Wanderer : GameObject
{
	//Variables
	private const string _textureName = "orb";
	private Vector2 _target;

	private Random _random = new Random();

	public Wanderer(GameObject parent, string name = "Wanderer") : base(parent, name)
	{
		TextureName = _textureName;
		Direction = GetRandomDirection();
		SetNewTarget();
	}

	public override void Update(GameTime gameTime)
	{
		// move toward the target
		var diff = _target - Position;
		Direction = (float)Math.Atan2(diff.Y, diff.X);

		//check if the wanderer is close enought to the target
		var distanceToTarget = Vector2.Distance(Position, _target);

		if (distanceToTarget < 10)
		{
			SetNewTarget();
		}

		base.Update(gameTime);
	}

	private void SetNewTarget()
	{
		// the wanderer will move toward a random target and once it's git close enought pick a new target
		var target_x = _random.Next(0, DrawManager.Graphics.PreferredBackBufferWidth);
		var target_y = _random.Next(0, DrawManager.Graphics.PreferredBackBufferHeight);
		_target = new Vector2(target_x, target_y);
	}

	private float GetRandomDirection()
	{
		var angle = (float)(_random.NextDouble() * MathHelper.TwoPi);
		return angle;
	}

	// reverse string
}