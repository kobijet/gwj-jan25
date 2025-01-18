using Godot;
using System;

public partial class Bomb : Node2D
{
	[Export] public float damage = 75.0f;
	
	public float cooldownTime = 5.0f;
	
	private Timer timer;
	
	private AnimationPlayer animationPlayer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Start bomb timer and set to deal damage on timeout
		timer = new Timer();
		timer.SetWaitTime(cooldownTime);
		timer.Timeout += Detonate;
		AddChild(timer);
		
		animationPlayer = GetNode<AnimationPlayer>("Sprite/AnimationPlayer");
		animationPlayer.AssignedAnimation = "bomb_flash";
	}
	
	public void StartTimer()
	{
		animationPlayer.Play();
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
