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
	
	private List<EnemyCharacter> livingEntities = new List<EnemyCharacter>();
	
	private Timer spawnTimer;
	
	[Export] public double spawnCooldown = 4.0;
	
	private Random rand;
	
	public override void _Ready() 
	{
		rand = new Random();
		
		// Load external resources
		basicEnemyScene = ResourceLoader.Load<PackedScene>("res://game/scenes/enemy_character.tscn");
		slugEnemyScene = ResourceLoader.Load<PackedScene>("res://game/scenes/enemy_character_slug.tscn");
		
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
		float slugSpawnChance = 90;
		bool spawnSlug = rand.Next(0, 100) <= slugSpawnChance ? true : false;
		
		Node2D enemy = null;
		if (spawnSlug)
		{
			enemy = (Node2D)basicEnemyScene.Instantiate();	
		}
		else
		{
			enemy = (Node2D)slugEnemyScene.Instantiate();
		}
		
		// Begin tracking enemy and subscribe to its death event
		livingEntities.Add(enemy as EnemyCharacter);
		
		// Setup enemy health component
		HealthComponent healthComponent = enemy.GetNode<HealthComponent>("HealthComponent");
		healthComponent.HealthDepleted += () => livingEntities.Remove(enemy as EnemyCharacter);
		
		AddChild(enemy);
		
		// StopSpawner(); // REMOVE BEFORE PRODUCTION - Ensures spawning of only one enemy
	}
}
