using Godot;
using System;

public partial class GameManager : Node
{
	public enum GameStates {
		ROUND_COMPLETE = 0, // Shop menu is active
		ROUND_ACTIVE = 1, // Gameplay is active
		GAME_OVER = 2 // Game over
	}
	public GameStates GameState = GameStates.ROUND_ACTIVE;
	
	private double startTime;
	private double elapsedTime;
	private double pausedTime;
	
	[Export] public PackedScene enemySpawner { get; set; }
	
	private Random rand;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rand = new Random();
		
		enemySpawner = ResourceLoader.Load<PackedScene>("res://game/scenes/enemy_spawner.tscn");
		
		// Save the time at start of gameplay scene
		startTime = GetTicksInSeconds();
	
		SetupEnemySpawners(4);
	}
	
	public override void _Process(double delta)
	{
		elapsedTime = GetTicksInSeconds() - startTime;
	}
	
	public void SetupEnemySpawners(int numSpawners)
	{		
		// Get viewport size for plopping down random spawners within the bounds
		Vector2 viewportSize = GetViewport().GetVisibleRect().Size;
		
		for (int i = 0; i < numSpawners; i++)
		{
			int randX = rand.Next(100, (int)viewportSize.X - 100);
			int randY = rand.Next(100, (int)viewportSize.Y - 100);
			
			Node2D spawner = (Node2D)enemySpawner.Instantiate();
			spawner.Position = new Vector2(randX, randY);
			AddChild(spawner);
		}
	}
	
	// Gets time since engine start in seconds
	private double GetTicksInSeconds()
	{
		return Time.GetTicksMsec() / 1000;
	}
}
