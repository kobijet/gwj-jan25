using Godot;
using System;
using System.Collections.Generic;

public partial class EnemySpawner : Node2D
{	
	// Emits a signal when all enemies are dead
	[Signal] public delegate void EnemiesDepletedEventHandler();
	
	public bool active = false;
	
	private PackedScene basicEnemyScene;
	private PackedScene slugEnemyScene;
	private PackedScene waspEnemyScene;
	
	private List<EnemyCharacter> livingEntities = new List<EnemyCharacter>();
	
	private Timer spawnTimer;
	
	[Export] public double spawnCooldown = 4.0;
	
	private Random rand;
	
	private GameManager gameManager;
	
	public override void _Ready() 
	{
		gameManager = GetNode("/root/Game/GameManager") as GameManager;
		
		rand = new Random();
		
		// Load external resources
		basicEnemyScene = ResourceLoader.Load<PackedScene>("res://game/scenes/enemy_character.tscn");
		slugEnemyScene = ResourceLoader.Load<PackedScene>("res://game/scenes/enemy_character_slug.tscn");
		waspEnemyScene = ResourceLoader.Load<PackedScene>("res://game/scenes/enemy_character_wasp.tscn");
		
		// Setup initial spawn timer
		spawnTimer = new Timer();
		spawnTimer.Timeout += SpawnEnemy;
		AddChild(spawnTimer);
		spawnTimer.Start();
		
		StartSpawner();
	}
	
	public void StartSpawner()
	{
		spawnTimer.SetWaitTime(spawnCooldown);
		spawnTimer.SetPaused(false);
	}
	
	public void StopSpawner()
	{
		spawnTimer.SetPaused(true);
	}
	
	private void SpawnEnemy()
	{
		//
		// Enemy spawner statistics
		//
		float slugSpawnChance = 5;
		float waspSpawnChance = 25;
		
		bool spawnSlug = rand.Next(0, 100) <= slugSpawnChance ? true : false;
		bool spawnWasp = rand.Next(0, 100) <= waspSpawnChance ? true : false;
		
		Node2D enemy = null;
		if (spawnWasp && !spawnSlug)
		{
			enemy = (Node2D)waspEnemyScene.Instantiate();	
		}
		else if (spawnSlug)
		{
			enemy = (Node2D)slugEnemyScene.Instantiate();
		}
		else
		{
			enemy = (Node2D)basicEnemyScene.Instantiate();
		}
		
		EnemyCharacter enemyScript = enemy as EnemyCharacter;
		
		livingEntities.Add(enemyScript);
		
		// Setup enemy health component
		HealthComponent healthComponent = enemy.GetNode<HealthComponent>("HealthComponent");
		healthComponent.HealthDepleted += () => livingEntities.Remove(enemy as EnemyCharacter);
		healthComponent.maxHealth = healthComponent.maxHealth * gameManager.difficulty;
		
		AddChild(enemy);
		
		// StopSpawner(); // REMOVE BEFORE PRODUCTION - Ensures spawning of only one enemy
	}
	
	public bool CheckForLivingEnemies()
	{
		return livingEntities.Count != 0 ? true : false;
	}
}
