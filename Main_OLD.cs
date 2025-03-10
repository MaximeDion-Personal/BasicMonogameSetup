using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mouse_Avoider;

public class Main_Old : Game
{
	private GraphicsDeviceManager graphics;
	private SpriteBatch spriteBatch;

	private Texture2D playerTexture, orbTexture, collectibleTexture;
	private SpriteFont scoreFont;

	private Vector2 playerPosition;
	private Vector2 collectiblePosition;

	private List<Orb> orbs = new List<Orb>();

	private int currentScore = 0;
	private int highScore = 0;

	private Random random = new Random();

	public Main_Old()
	{
		graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
	}

	protected override void Initialize()
	{
		// Start player at screen center.
		playerPosition = new Vector2(
			graphics.PreferredBackBufferWidth / 2,
			graphics.PreferredBackBufferHeight / 2);

		// Place the collectible at a random position.
		collectiblePosition = GetRandomPosition(32, 32);
		base.Initialize();
	}

	protected override void LoadContent()
	{
		spriteBatch = new SpriteBatch(GraphicsDevice);

		// Load textures and font from the Content pipeline.
		// Ensure these assets are added to your Content project.
		playerTexture = Content.Load<Texture2D>("player");
		orbTexture = Content.Load<Texture2D>("orb");
		collectibleTexture = Content.Load<Texture2D>("collectible");
		//scoreFont = Content.Load<SpriteFont>("ScoreFont");

		// Initialize collectible position now that we know its size.
		collectiblePosition = GetRandomPosition(collectibleTexture.Width, collectibleTexture.Height);
	}

	protected override void Update(GameTime gameTime)
	{
		// Exit on Escape.
		if (Keyboard.GetState().IsKeyDown(Keys.Escape))
			Exit();

		// Update player position from the mouse.
		MouseState mouse = Mouse.GetState();
		playerPosition = new Vector2(mouse.X - playerTexture.Width / 2, mouse.Y - playerTexture.Height / 2);
		Rectangle playerRect = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, playerTexture.Width, playerTexture.Height);

		// Create a rectangle for the collectible.
		Rectangle collectibleRect = new Rectangle((int)collectiblePosition.X, (int)collectiblePosition.Y, collectibleTexture.Width, collectibleTexture.Height);

		// Check if player collects the collectible.
		if (playerRect.Intersects(collectibleRect))
		{
			// Spawn a new orb at a random position with a random velocity.
			orbs.Add(new Orb(GetRandomPosition(orbTexture.Width, orbTexture.Height), GetRandomVelocity()));
			currentScore++;
			// Move the collectible to a new random position.
			collectiblePosition = GetRandomPosition(collectibleTexture.Width, collectibleTexture.Height);
		}

		// Update orbs.
		for (int i = orbs.Count - 1; i >= 0; i--)
		{
			orbs[i].Update(gameTime, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, orbTexture.Width, orbTexture.Height);

			// Check collision between orb and player.
			if (playerRect.Intersects(orbs[i].Bounds(orbTexture.Width, orbTexture.Height)))
			{
				// Update high score if needed.
				if (currentScore > highScore)
					highScore = currentScore;
				// Reset the game.
				currentScore = 0;
				orbs.Clear();
				collectiblePosition = GetRandomPosition(collectibleTexture.Width, collectibleTexture.Height);
				break;
			}
		}

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);

		spriteBatch.Begin();
		// Draw player.
		spriteBatch.Draw(playerTexture, playerPosition, Color.White);
		// Draw collectible.
		spriteBatch.Draw(collectibleTexture, collectiblePosition, Color.White);
		// Draw orbs.
		foreach (var orb in orbs)
			spriteBatch.Draw(orbTexture, orb.Position, Color.White);
		// Draw scores.
		//spriteBatch.DrawString(scoreFont, $"Score: {currentScore}", new Vector2(10, 10), Color.White);
		//spriteBatch.DrawString(scoreFont, $"High Score: {highScore}", new Vector2(10, 30), Color.White);
		spriteBatch.End();

		base.Draw(gameTime);
	}

	// Helper: Get a random position within the screen boundaries.
	private Vector2 GetRandomPosition(int textureWidth, int textureHeight)
	{
		int x = random.Next(0, graphics.PreferredBackBufferWidth - textureWidth);
		int y = random.Next(0, graphics.PreferredBackBufferHeight - textureHeight);
		return new Vector2(x, y);
	}

	// Helper: Get a random velocity for the orb.
	private Vector2 GetRandomVelocity()
	{
		// Random float between -3 and 3.
		float vx = (float)(random.NextDouble() * 6 - 3);
		float vy = (float)(random.NextDouble() * 6 - 3);
		return new Vector2(vx, vy);
	}
}

// Orb class representing bouncing orbs.
public class Orb
{
	public Vector2 Position;
	public Vector2 Velocity;

	public Orb(Vector2 position, Vector2 velocity)
	{
		Position = position;
		Velocity = velocity;
	}

	// Update position and bounce off screen edges.
	public void Update(GameTime gameTime, int screenWidth, int screenHeight, int textureWidth, int textureHeight)
	{
		Position += Velocity;

		// Bounce horizontally.
		if (Position.X < 0 || Position.X + textureWidth > screenWidth)
			Velocity.X *= -1;
		// Bounce vertically.
		if (Position.Y < 0 || Position.Y + textureHeight > screenHeight)
			Velocity.Y *= -1;
	}

	// Return the bounding rectangle of the orb.
	public Rectangle Bounds(int textureWidth, int textureHeight)
	{
		return new Rectangle((int)Position.X, (int)Position.Y, textureWidth, textureHeight);
	}
}