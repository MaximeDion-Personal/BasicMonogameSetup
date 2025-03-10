using System;

using Microsoft.Xna.Framework;

namespace Mouse_Avoider.Source.Core.Managers
{
	/// <summary>
	/// The normalized directions that can be returned.
	/// </summary>
	public enum NormalizedDirection
	{
		None,
		Up,
		Down,
		Left,
		Right,
		UpRight,
		DownRight,
		DownLeft,
		UpLeft
	}

	/// <summary>
	/// The quantization mode for stick normalization.
	/// </summary>
	public enum DirectionMode
	{
		TwoVertical,     // Only Up and Down.
		TwoHorizontal,   // Only Left and Right.
		FourDirections,  // Up, Down, Left, Right.
		EightDirections  // Up, Down, Left, Right, and the four diagonals.
	}

	public static class InputHelper
	{
		/// <summary>
		/// Normalizes the stick input into one of a fixed set of directions.
		/// </summary>
		/// <param name="input">The stick input vector.</param>
		/// <param name="mode">The quantization mode determining allowed directions.</param>
		/// <returns>A NormalizedDirection enum value.</returns>
		public static NormalizedDirection NormalizeStickDirection(Vector2 input, DirectionMode mode)
		{
			// If the input is zero, return None.
			if (input == Vector2.Zero)
			{
				return NormalizedDirection.None;
			}

			// Get the angle of the input in radians.
			float angle = (float)Math.Atan2(input.Y, input.X);

			// Convert angle to degrees in the range 0 -> 360
			float degrees = MathHelper.ToDegrees(angle);
			if (degrees < 0)
			{
				degrees += 360;
			}

			switch (mode)
			{
				case DirectionMode.TwoVertical:
					return input.Y < 0 ? NormalizedDirection.Up : NormalizedDirection.Down;

				case DirectionMode.TwoHorizontal:
					return input.X < 0 ? NormalizedDirection.Left : NormalizedDirection.Right;

				case DirectionMode.FourDirections:
					{
						float adjusted = (degrees + 45) % 360;
						int sector = (int)(adjusted / 90);
						switch (sector)
						{
							case 0: return NormalizedDirection.Up;
							case 1: return NormalizedDirection.Right;
							case 2: return NormalizedDirection.Down;
							case 3: return NormalizedDirection.Left;
							default: return NormalizedDirection.None;
						}
					}

				case DirectionMode.EightDirections:
					{
						float adjusted = (degrees + 22.5f) % 360;
						int sector = (int)(adjusted / 45);
						switch (sector)
						{
							case 0: return NormalizedDirection.Right;       // 0° to 45°
							case 1: return NormalizedDirection.UpRight;     // 45° to 90°
							case 2: return NormalizedDirection.Up;          // 90° to 135°
							case 3: return NormalizedDirection.UpLeft;      // 135° to 180°
							case 4: return NormalizedDirection.Left;        // 180° to 225°
							case 5: return NormalizedDirection.DownLeft;    // 225° to 270°
							case 6: return NormalizedDirection.Down;        // 270° to 315°
							case 7: return NormalizedDirection.DownRight;   // 315° to 360°
							default: return NormalizedDirection.None;
						}
					}

				default:
					return NormalizedDirection.None;
			}
		}
	}
}