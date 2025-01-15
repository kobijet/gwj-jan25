using Godot;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
	public enum GameStates {
		ROUND_COMPLETE = 0, // Shop menu is active
		ROUND_ACTIVE = 1, // Gameplay is active
		GAME_OVER = 2 // Game over
	}
	public GameStates GameState = GameStates.ROUND_ACTIVE;
	
	[Export] public double roundLength = 4.0;
	
	private Timer gameTimer;
	private double timeLeft;
	
	private PackedScene enemySpawner;
	private List<Node2D> enemySpawners;
	
	private Hud gameHud;
	
	private Random rand;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rand = new Random();
		
		// Start a master clock
		gameTimer = new Timer();
		gameTimer.SetWaitTime(roundLength);
		gameTimer.Timeout += EndRound;
		AddChild(gameTimer);
		
		enemySpawner = ResourceLoader.Load<PackedScene>("res://game/scenes/enemy_spawner.tscn");
		
		// Get game hud elements to display info for the player
		gameHud = GetNode<Hud>("%PlayerCharacter/Camera2D/HUD");
		gameHud.GetNode<Button>("GamePanel/NextRoundButton").Pressed += StartRound;
	
		// Keep a list of enemy spawners for control
		enemySpawners = new List<Node2D>();
		SetupEnemySpawners(4);
		
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
		gameTimer.SetWaitTime(roundLength);
		gameTimer.Start();
		StartEnemySpawners();
		//gameHud.StartRound();
	}	

	private void EndRound()
	{
		gameTimer.SetPaused(true);
		PauseEnemySpawners();
		gameHud.EndRound();
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
