using Godot;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
	public int roundsCompleted;
	[Export] public double roundLength = 10.0;
	
	private Timer gameTimer;
	private double timeLeft;
	
	[Export] public int spawnersToCreate = 1;
	[Export] public int maxSpawners = 8;
	private PackedScene enemySpawner;
	private List<Node2D> enemySpawners;
	
	private Hud gameHud;
	private PackedScene gameOverScene;
	
	private Random rand;
	
	public float difficulty = 1.0f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rand = new Random();
		
		// Start a round timer
		// End round when timer runs out
		gameTimer = new Timer();
		gameTimer.Timeout += EndRound;
		AddChild(gameTimer);
		gameTimer.SetWaitTime(roundLength);
		gameTimer.Start();
		
		enemySpawner = ResourceLoader.Load<PackedScene>("res://game/scenes/enemy_spawner.tscn");
		
		// Get game hud elements to display info for the player
		var hudNode = GetNode<Node>("%PlayerCharacter/Camera2D/HUD");
		gameHud = hudNode as Hud;
		gameHud.GetNode<Button>("GamePanel/NextRoundButton").Pressed += StartRound;
		
		gameOverScene = ResourceLoader.Load<PackedScene>("res://game/scenes/UI/game_over.tscn");
	
		// Keep a list of enemy spawners for controling
		enemySpawners = new List<Node2D>();
		
		// Subscribe to player death event
		PlayerCharacter player = GetNode("%PlayerCharacter") as PlayerCharacter;
		player.PlayerDied += EndGame;
		
		StartRound();
	}
	
	public override void _Process(double delta)
	{
		timeLeft = gameTimer.TimeLeft;
	}
	
	private void StartRound()
	{
		if (enemySpawners.Count != 0)
		{
			DestroyEnemySpawners();	
		}
		
		// Increase "difficulty" as time goes on
		// Used for scaling different elements of the game
		difficulty = 1.0f + (float)Math.Sqrt(roundsCompleted / 2);
		GD.Print($"{difficulty} difficulty factor at round {roundsCompleted}");
		
		// Increase number of spawners by difficulty factor up to max
		if (roundsCompleted != 0)
		{
			spawnersToCreate = (int)Math.Round(1 + (difficulty / 2));
			if (spawnersToCreate > maxSpawners)
			{
				spawnersToCreate = maxSpawners;
			}
		}
		
		SetupEnemySpawners(spawnersToCreate);
		
		// Scale up round length with difficulty
		if (roundsCompleted != 0)
		{
			roundLength = 10.0f * difficulty;
		}
		
		// Set timer length to new round length and unpause timer
		gameTimer.Start(roundLength);
		gameTimer.SetPaused(false);
		
		StartEnemySpawners();
		
		gameHud.StartRound();
	}	
	
	public void SetupEnemySpawners(int numSpawners)
	{		
		// Get viewport size for plopping down random spawners within the bounds
		Vector2 viewportSize = GetViewport().GetVisibleRect().Size;
		
		for (int i = 0; i < numSpawners; i++)
		{
			// Get random position for spawner, at least 100px within the level edge
			int randX = rand.Next(100, (int)viewportSize.X - 100);
			int randY = rand.Next(100, (int)viewportSize.Y - 100);
			
			// Instantiate spawner scene and add to spawners list
			Node2D spawner = (Node2D)enemySpawner.Instantiate();
			spawner.Position = new Vector2(randX, randY);
			
			var spawnerScript = spawner as EnemySpawner;
			spawnerScript.active = true;
			
			enemySpawners.Add(spawner);
			AddChild(spawner);
		}
	}

	private void EndRound()
	{
		roundsCompleted++;
		
		gameTimer.SetPaused(true);

		PauseEnemySpawners();
		
		gameHud.EndRound();
	}
	
	private bool CheckSpawnersForEnemies()
	{
		bool enemiesAlive = false;
		for (int i = 0; i < enemySpawners.Count; i++)
		{
			var spawnerScript = enemySpawners[i] as EnemySpawner;
			if (enemiesAlive != spawnerScript.CheckForLivingEnemies())
			{
				enemiesAlive = spawnerScript.CheckForLivingEnemies();
			}
		}
		return enemiesAlive;
	}
	
	private void EndGame()
	{
		var gameOverScreen = gameOverScene.Instantiate();
		gameHud.AddChild(gameOverScreen);
	}
	
	private void DestroyEnemySpawners()
	{
		for (int i = 0; i < enemySpawners.Count; i++)
		{
			enemySpawners[i].QueueFree();
		}
		
		enemySpawners.Clear();
	}
	
	private void PauseEnemySpawners()
	{
		for (int i = 0; i < enemySpawners.Count; i++)
		{
			var spawnerScript = enemySpawners[i] as EnemySpawner;
			spawnerScript.StopSpawner();
		}
	}
	
	private void StartEnemySpawners()
	{
		for (int i = 0; i < enemySpawners.Count; i++)
		{
			var spawnerScript = enemySpawners[i] as EnemySpawner;
			spawnerScript.StartSpawner();
		}
	}
	
	public double GetTimeLeftInRound()
	{
		return Math.Round(timeLeft, 2);
	}
}
