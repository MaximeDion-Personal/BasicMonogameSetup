using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mouse_Avoider.Source.Core.Managers;

namespace Mouse_Avoider.Source.Core;

/// <summary>
/// Represents a game object that exist in the game world.
/// Supports hierarchical transformations and collision/lifecycle management.
/// </summary>
public class GameObject
{
	// -----------------------------------------------------------
	// Private Fields
	// -----------------------------------------------------------
	private bool _centerOfRotationSet;

	private Vector2 _centerOfRotation;

	private bool _isAlive = true;
	private bool _isVisible = true;
	private bool _isSolid = true;

	protected bool _isInitialized = false;

	// Backing field for direction if Velocity is zero.
	private float _direction = 0f;

	// -----------------------------------------------------------
	// Constructors
	// -----------------------------------------------------------
	public GameObject(GameObject parent = null, string name = "")
	{
		Parent = parent;
		Name = name;

		Position = Vector2.Zero;
		Velocity = Vector2.Zero;

		Size = Vector2.Zero;
		Scale = Vector2.One;

		Alpha = 1f;

		Rotation = 0f;
		_centerOfRotationSet = false;
		_centerOfRotation = Vector2.Zero;

		Children = new List<GameObject>();
	}

	// -----------------------------------------------------------
	// Basic Properties
	// -----------------------------------------------------------
	public GameObject Parent { get; protected set; }

	public string Name { get; protected set; }

	protected Texture2D Texture { get; set; }
	protected string TextureName { get; set; }

	// -----------------------------------------------------------
	// Transformation Properties
	// -----------------------------------------------------------
	public Vector2 Position { get; set; }

	/// <summary>
	/// The velocity vector.
	/// Note: Speed and Direction properties work with this value.
	/// </summary>
	public Vector2 Velocity { get; set; }

	public Vector2 Size { get; set; }
	public float Rotation { get; set; } // In radians.
	public Vector2 Scale { get; set; }
	public float Alpha { get; set; } // 1 = opaque, 0 = transparent.

	/// <summary>
	/// Center of rotation used as the origin in the draw call.
	/// If not explicitly set, it defaults to the center of the object.
	/// </summary>
	public Vector2 CenterOfRotation
	{
		get => _centerOfRotation;
		set
		{
			_centerOfRotation = value;
			_centerOfRotationSet = true;
		}
	}

	/// <summary>
	/// The speed of the object computed from the Velocity vector.
	/// Setting this property updates Velocity while preserving the current direction.
	/// </summary>
	public float Speed
	{
		get => Velocity.Length();
		set
		{
			// Ensure non-negative.
			value = Math.Max(0, value);

			// If Velocity is zero, use the stored _direction.
			Vector2 dir = (Velocity == Vector2.Zero)
				? new Vector2((float)Math.Cos(_direction), (float)Math.Sin(_direction))
				: Vector2.Normalize(Velocity);

			Velocity = dir * value;
		}
	}

	/// <summary>
	/// The direction (in radians) of the object's movement.
	/// Getting returns the angle from Velocity; setting updates Velocity.
	/// </summary>
	public float Direction
	{
		get
		{
			if (Velocity != Vector2.Zero)
			{
				return (float)Math.Atan2(Velocity.Y, Velocity.X);
			}

			return _direction;
		}
		set
		{
			_direction = value;
			float speed = Speed; // current speed
			Velocity = new Vector2((float)Math.Cos(value), (float)Math.Sin(value)) * speed;
		}
	}

	// -----------------------------------------------------------
	// Computed Global Transformation Properties (from the parent)
	// -----------------------------------------------------------
	public Vector2 GlobalPosition
	{
		get
		{
			if (Parent != null)
			{
				var scaled = new Vector2(Position.X * Parent.GlobalScale.X, Position.Y * Parent.GlobalScale.Y);
				var rotated = Vector2.Transform(scaled, Matrix.CreateRotationZ(Parent.GlobalRotation));
				return Parent.GlobalPosition + rotated;
			}
			return Position;
		}
	}

	public float GlobalRotation => (Parent?.GlobalRotation ?? 0) + Rotation;

	public Vector2 GlobalScale => Parent != null ? new Vector2(Parent.GlobalScale.X * Scale.X, Parent.GlobalScale.Y * Scale.Y) : Scale;

	public Rectangle Bounds => new Rectangle(
		(int)GlobalPosition.X,
		(int)GlobalPosition.Y,
		(int)Size.X,
		(int)Size.Y);

	public float ZIndex { get; set; }

	// -----------------------------------------------------------
	// Initialization and Load Content
	// -----------------------------------------------------------
	public virtual void Initialize()
	{
		if (_isInitialized)
		{
			return;
		}

		_isInitialized = true;

		if (!string.IsNullOrEmpty(TextureName))
		{
			Texture = Main.ContentManager.Load<Texture2D>(TextureName);

			if (Size == Vector2.Zero)
			{
				Size = new Vector2(Texture.Width, Texture.Height);
			}
		}

		if (!_centerOfRotationSet)
		{
			CenterOfRotation = new Vector2(Size.X / 2f, Size.Y / 2f);
		}

		InitializeChildren();
	}

	public virtual void LoadContent()
	{
		// Override to load additional content.
	}

	// -----------------------------------------------------------
	// Update and Draw Methods
	// -----------------------------------------------------------
	public virtual void Update(GameTime gameTime)
	{
		Position += Velocity;
		UpdateChildren(gameTime);
	}

	public virtual void Draw(GameTime gameTime)
	{
		if (!IsVisible())
		{
			return;
		}

		if (Texture != null)
		{
			DrawManager.SpriteBatch.Draw(
				Texture,
				GlobalPosition,
				null,
				Color.White * Alpha,
				GlobalRotation,
				CenterOfRotation,
				GlobalScale,
				SpriteEffects.None,
				0f);
		}
		DrawChildren(gameTime);
	}

	// -----------------------------------------------------------
	// Children Management
	// -----------------------------------------------------------
	public List<GameObject> Children { get; private set; }

	public virtual void InitializeChildren()
	{
		foreach (var child in Children)
		{
			child.Initialize();
		}
	}

	protected virtual void UpdateChildren(GameTime gameTime)
	{
		foreach (var child in Children)
		{
			child.Update(gameTime);
		}
	}

	protected virtual void DrawChildren(GameTime gameTime)
	{
		foreach (var child in Children)
		{
			child.Draw(gameTime);
		}
	}

	public void AddChild(GameObject child)
	{
		if (child != null)
		{
			child.Parent = this;
			Children.Add(child);
		}
	}

	public void RemoveChild(GameObject child)
	{
		if (child != null && Children.Remove(child))
		{
			child.Parent = null;
		}
	}

	public void RemoveAllChildren()
	{
		foreach (var child in Children)
		{
			child.Parent = null;
		}

		Children.Clear();
	}

	public GameObject FindChild(string name)
	{
		return Children.Find(child => child.Name == name);
	}

	public List<GameObject> FindChildren(string name)
	{
		return Children.FindAll(child => child.Name == name);
	}

	// -----------------------------------------------------------
	// Collision and Lifecycle Methods
	// -----------------------------------------------------------
	public virtual bool CollidesWith(GameObject other)
	{
		if (!IsSolid())
		{
			return false;
		}

		return Bounds.Intersects(other.Bounds);
	}

	public virtual void Destroy()
	{
		// TODO: destruction logic.
	}

	public virtual bool IsAlive() => _isAlive;

	public virtual bool IsVisible() => _isVisible;

	public virtual bool IsSolid() => _isSolid;

	public virtual void Kill() => _isAlive = false;

	public virtual void Hide() => _isVisible = false;

	public virtual void Show() => _isVisible = true;

	public virtual void SetSolid(bool solid) => _isSolid = solid;

	// -----------------------------------------------------------
	// Convenience Properties
	// -----------------------------------------------------------
	public float X
	{
		get => Position.X;
		set => Position = new Vector2(value, Position.Y);
	}

	public float XCenter
	{
		get => GlobalPosition.X + Size.X / 2f;
		set => Position = new Vector2(value - Size.X / 2f, Position.Y);
	}

	public float Y
	{
		get => Position.Y;
		set => Position = new Vector2(Position.X, value);
	}

	public float YCenter
	{
		get => GlobalPosition.Y + Size.Y / 2f;
		set => Position = new Vector2(Position.X, value - Size.Y / 2f);
	}

	public float Width
	{
		get => Size.X;
		set => Size = new Vector2(value, Size.Y);
	}

	public float Height
	{
		get => Size.Y;
		set => Size = new Vector2(Size.X, value);
	}
}