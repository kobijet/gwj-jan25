using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	PackedScene enemyScene;
	
	public override void _Ready() 
	{
		// Pre-load external resources
		enemyScene = ResourceLoader.Load<PackedScene>("res://game/scenes/enemy_character.tscn");
		
		// Spawn position offset is a point centered around
		// the spawner's center point
		// Vector2 spawnPositionOffset = new Vector2(0.0f, 0.0f);
		// SpawnEnemy(spawnPositionOffset);
	}
	
	public void SpawnEnemy(Vector2 spawnPosition)
	{
		Node2D enemy = (Node2D)enemyScene.Instantiate();
		enemy.SetPosition(spawnPosition);
		AddChild(enemy);
	}
}
