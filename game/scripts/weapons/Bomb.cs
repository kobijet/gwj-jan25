using Godot;
using System;

public partial class Bomb : Node2D
{
	[Export] public float damage = 75.0f;
	
	public float cooldownTime = 8.0f;
	
	private Timer timer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Start bomb timer and set to deal damage on timeout
		timer = new Timer();
		timer.SetWaitTime(cooldownTime);
		timer.Timeout += Detonate;
		AddChild(timer);
	}
	
	public void StartTimer()
	{
		timer.Start();
	}

	private void Detonate()
	{
		Godot.Collections.Array<Node2D> overlappingBodies = GetNode<Area2D>("BlastRadius").GetOverlappingBodies();
		
		for (int i = 0; i < overlappingBodies.Count; i++)
		{
			EnemyCharacter enemy = overlappingBodies[i] as EnemyCharacter;
			HealthComponent healthComponent = enemy.GetNode<HealthComponent>("HealthComponent");
			healthComponent.TakeDamage(damage);
		}
		
		QueueFree();
	}
}
