using Godot;
using System;
using System.Collections.Generic;

public partial class EnemySpawner : Node2D
{
	// Emits a signal when all enemies are dead
	[Signal] public delegate void EnemiesDepletedEventHandler();
	
	public bool paused = false;
	
	private PackedScene enemyScene;
	
	// Maybe want to store just the health components as opposed to the whole enemy? -KR
	private List<EnemyCharacter> livingEntities = new List<EnemyCharacter>();
	
	public override void _Ready() 
	{
		// Load external resources
		enemyScene = ResourceLoader.Load<PackedScene>("res://game/scenes/enemy_character.tscn");
		
		// Spawn position offset is a point centered around
		// the spawner's center point
		Vector2 spawnPositionOffset = new Vector2(0.0f, 0.0f);
		SpawnEnemy(spawnPositionOffset);
	}
	
	public override void _Process(double delta)
	{
	}
	
	public void SpawnEnemy(Vector2 spawnPosition)
	{
		Node2D enemy = (Node2D)enemyScene.Instantiate();
		enemy.SetPosition(spawnPosition);
		
		// Begin tracking enemy and subscribe to its death event
		livingEntities.Add(enemy as EnemyCharacter);
		
		HealthComponent healthComponent = enemy.GetNode<HealthComponent>("HealthComponent");
		healthComponent.HealthDepleted += () => livingEntities.Remove(enemy as EnemyCharacter);
		
		AddChild(enemy);
	}
}
