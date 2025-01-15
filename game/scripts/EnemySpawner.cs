using Godot;
using System;
using System.Collections.Generic;

public partial class EnemySpawner : Node2D
{
	// Emits a signal when all enemies are dead
	[Signal] public delegate void EnemiesDepletedEventHandler();
	
	public bool active = false;
	
	private PackedScene enemyScene;
	
	private List<EnemyCharacter> livingEntities = new List<EnemyCharacter>();
	
	private Timer spawnTimer;
	
	[Export] public double spawnCooldown = 4.0;
	
	public override void _Ready() 
	{
		// Load external resources
		enemyScene = ResourceLoader.Load<PackedScene>("res://game/scenes/enemy_character.tscn");
		
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
		Node2D enemy = (Node2D)enemyScene.Instantiate();
		
		// Begin tracking enemy and subscribe to its death event
		livingEntities.Add(enemy as EnemyCharacter);
		
		HealthComponent healthComponent = enemy.GetNode<HealthComponent>("HealthComponent");
		healthComponent.HealthDepleted += () => livingEntities.Remove(enemy as EnemyCharacter);
		
		AddChild(enemy);
	}
}
