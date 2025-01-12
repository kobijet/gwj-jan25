using Godot;
using System;
using System.Collections.Generic;

public partial class EnemySpawner : Node2D
{
	PackedScene enemyScene;
	
	// Maybe want to store just the health components as opposed to the whole enemy? -KR
	List<EnemyCharacter> livingEntities = new List<EnemyCharacter>();
	
	public override void _Ready() 
	{
		// Load external resources
		enemyScene = ResourceLoader.Load<PackedScene>("res://game/scenes/enemy_character.tscn");
		
		// Spawn position offset is a point centered around
		// the spawner's center point
		
		// Spawns enemy for testing
		Vector2 spawnPositionOffset = new Vector2(0.0f, 0.0f);
		SpawnEnemy(spawnPositionOffset);
		
	}
	
	public void SpawnEnemy(Vector2 spawnPosition)
	{
		Node2D enemy = (Node2D)enemyScene.Instantiate();
		enemy.SetPosition(spawnPosition);
		
		livingEntities.Add(enemy as EnemyCharacter);
		
		AddChild(enemy);
	}
}
