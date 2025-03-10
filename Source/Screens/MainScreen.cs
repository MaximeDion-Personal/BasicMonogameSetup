using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Mouse_Avoider.Source.Core;
using Mouse_Avoider.Source.Core.Managers;
using Mouse_Avoider.Source.Entities;

namespace Mouse_Avoider.Source.Screens;

public class MainScreen : GameScreen
{
	// Variables
	private int _currentScore = 0;

	private int _highScore = 0;

	private float _enemySpeed = 3;
	private const float InitialEnemySpeed = 2;
	private const float EnemySpeedIncrement = 0.15f;

	private Random _random = new Random();

	// Entities
	private Player _player;

	private List<GameObject> _enemies = new List<GameObject>();
	private List<GameObject> _collectibles = new List<GameObject>();

	//private Texture2D playerTexture, orbTexture, collectibleTexture;
	//private SpriteFont scoreFont;

	public MainScreen(GameObject parent, string name = "") : base(parent, name)
	{
	}

	public override void Initialize()
	{
		initPlayer();
		initCollectible();
		base.Initialize();
	}

	private void initPlayer()
	{
		_player = new Player(this);
		_player.Initialize();
		Children.Add(_player);

		// Start player at screen center.
		_player.Position = new Vector2(DrawManager.Graphics.PreferredBackBufferWidth / 2, DrawManager.Graphics.PreferredBackBufferHeight / 2);
	}

	private void initCollectible()
	{
		var collectible = new Collectible(this);
		collectible.Initialize();
		_collectibles.Add(collectible);
		Children.Add(collectible);

		collectible.Position = GetRandomPosition(collectible);
	}

	public override void LoadContent()
	{
	}

	public override void Update(GameTime gameTime)
	{
		// Exit on Escape.
		if (Keyboard.GetState().IsKeyDown(Keys.Escape))
		{
			Main.ExitGame();
		}

		updatePlayer(gameTime);

		// Check if player collects the collectible.
		foreach (var collectible in _collectibles)
		{
			if (_player.CollidesWith(collectible))
			{
				handleCollectibleCollision(collectible);
			}
		}

		// Collision with enemy logic
		foreach (var enemy in _enemies)
		{
			if (_player.CollidesWith(enemy))
			{
				resetGame();

				break;
			}
			else
			{
				enemy.Update(gameTime);
			}
		}
	}

	private void handleCollectibleCollision(GameObject collectible)
	{
		_currentScore++;
		_enemySpeed += EnemySpeedIncrement;
		collectible.Position = GetRandomPosition(collectible);
		createEnemy();
	}

	private void resetGame()
	{
		if (_currentScore > _highScore)
		{
			_highScore = _currentScore;
		}

		_currentScore = 0;
		_enemySpeed = InitialEnemySpeed;

		foreach (var enemy in _enemies)
		{
			Children.Remove(enemy);
		}
		_enemies.Clear();

		_collectibles[0].Position = GetRandomPosition(_collectibles[0]);
	}

	// Update player position from the mouse
	private void updatePlayer(GameTime gameTime)
	{
		_player.Update(gameTime);

		MouseState mouse = Mouse.GetState();

		_player.XCenter = mouse.X;
		_player.YCenter = mouse.Y;
	}

	private void createEnemy()
	{
		var enemyClasses = new List<string> { "Wanderer", "Chaser", "Bouncer" };
		var enemyType = enemyClasses[_random.Next(enemyClasses.Count)];
		GameObject enemy;

		switch (enemyType)
		{
			case "Wanderer":
				enemy = new Wanderer(this);
				break;

			case "Sniper":
				enemy = new Sniper(this);
				break;

			case "Chaser":
				enemy = new Chaser(this);
				break;

			case "Bouncer":
			default:
				enemy = new Bouncer(this);
				break;
		}

		enemy.Initialize();
		enemy.Position = GetRandomPosition(enemy);
		enemy.Speed = enemyType == "Chaser" ? _enemySpeed / 2 : _enemySpeed;

		_enemies.Add(enemy);
		Children.Add(enemy);
	}

	private Vector2 GetRandomPosition(GameObject gameObject)
	{
		float x = (float)_random.NextDouble() * (DrawManager.Graphics.PreferredBackBufferWidth - gameObject.Width);
		float y = (float)_random.NextDouble() * (DrawManager.Graphics.PreferredBackBufferHeight - gameObject.Height);

		return new Vector2(x, y);
	}
}