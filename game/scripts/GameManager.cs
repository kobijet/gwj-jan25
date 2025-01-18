using Godot;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
	public int roundsComplete;
	
	[Export] public double roundLength = 10.0;
	
	private Timer gameTimer;
	private double timeLeft;
	
	private PackedScene enemySpawner;
	private List<Node2D> enemySpawners;
	
	private Hud gameHud;
	private PackedScene gameOverScene;
	
	private Random rand;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rand = new Random();
		
		// Start a round timer
		// End round when timer runs out
		gameTimer = new Timer();
		gameTimer.SetWaitTime(roundLength);
		gameTimer.Timeout += EndRound;
		AddChild(gameTimer);
		gameTimer.Start();
		
		enemySpawner = ResourceLoader.Load<PackedScene>("res://game/scenes/enemy_spawner.tscn");
		
		// Get game hud elements to display info for the player
		var hudNode = GetNode<Node>("%PlayerCharacter/Camera2D/HUD");
		gameHud = hudNode as Hud;
		gameHud.GetNode<Button>("GamePanel/NextRoundButton").Pressed += StartRound;
		
		gameOverScene = ResourceLoader.Load<PackedScene>("res://game/scenes/UI/game_over.tscn");
	
		// Keep a list of enemy spawners for control
		enemySpawners = new List<Node2D>();
		SetupEnemySpawners(1);
		
		// Subscribe to player death event
		PlayerCharacter player = GetNode("%PlayerCharacter") as PlayerCharacter;
		player.PlayerDied += EndGame;
		
		StartRound();
	}
	
	public override void _Process(double delta)
	{
		timeLeft = gameTimer.TimeLeft;
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
	
	private void StartRound()
	{
		if (roundsComplete != 0)
		{
			roundLength = roundLength * (roundsComplete / 10);
		}
		
		gameTimer.SetWaitTime(roundLength);
		gameTimer.SetPaused(false);
		StartEnemySpawners();
		gameHud.StartRound();
	}	

	private void EndRound()
	{
		GD.Print("Rounds complete: " + roundsComplete);
		roundsComplete++;
		
		gameTimer.SetPaused(true);
		PauseEnemySpawners();
		gameHud.EndRound();
	}
	
	private void EndGame()
	{
		var gameOverScreen = gameOverScene.Instantiate();
		gameHud.AddChild(gameOverScreen);
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
