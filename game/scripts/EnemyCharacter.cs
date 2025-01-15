using Godot;
using System;

public partial class EnemyCharacter : CharacterBody2D
{
	// Movement variables
	[Export] public float speed { get; set; } = 50.0f;
	[Export] public float maxSpeed { get; set; } = 100.0f;
	private Vector2 direction;
	
	private HealthComponent healthComponent;
	
	// Pathfinding
	private Vector2 targetPos = new Vector2(300.0f, 100.0f);
	private float distanceToTarget;
	
	private Node2D playerCharacter;
	
	private Area2D attackArea;
	
	private Timer attackTimer;
	[Export] public double attackCooldown = 1.0;

	public override void _Ready()
	{
		// Set target chase position to player position
		playerCharacter = GetTree().Root.GetNode<Node2D>("Game/%PlayerCharacter");
		
		// Subscribe to health depleted event
		healthComponent = GetNode<HealthComponent>("HealthComponent");
		healthComponent.HealthDepleted += OnHealthDepleted;
		
		attackArea = GetNode<Area2D>("AttackArea");
		
		// Setup attack cooldown timer
		attackTimer = new Timer();
		attackTimer.SetWaitTime(attackCooldown);
		attackTimer.Autostart = true;
		attackTimer.Timeout += AttackEntity;
		AddChild(attackTimer);
	}

	public override void _PhysicsProcess(double delta)
	{
		// Set player character as target to chase
		targetPos = playerCharacter.GlobalPosition;
		
		// Calculate and set distance to target
		distanceToTarget = GetDistanceToTarget();
		
		MoveEnemy(delta);
	}
	
	private void AttackEntity()
	{
		Godot.Collections.Array<Node2D> overlappingBodies = attackArea.GetOverlappingBodies();
		
		if (overlappingBodies != null)
		{
			// Check if enemy is overlapping something it can damage
			// If it is, deal damage on a timer to that entity
			for (int i = 0; i < overlappingBodies.Count; i++)
			{
				HealthComponent healthComponent = overlappingBodies[i].GetNode<HealthComponent>("HealthComponent");
				healthComponent.TakeDamage(5.0f);
			}
		}
	}
	
	// Returns distance from current position to targetPos
	public float GetDistanceToTarget()
	{
		// Calculate distance from character to target
		return Mathf.Sqrt( Mathf.Pow(targetPos.X - GlobalPosition.X, 2) + Mathf.Pow(targetPos.Y - GlobalPosition.Y, 2) );
	}
	
	// Moves enemy until within range of targetPos
	public void MoveEnemy(double delta)
	{
		Vector2 velocity = Velocity;
		
		direction = targetPos - GlobalPosition;
		direction = direction.Normalized();
		
		if (distanceToTarget >= 50.0f ? true : false)
		{
			// Apply movement in direction
			velocity = direction * maxSpeed;
			
			velocity = velocity.Clamp(-maxSpeed, maxSpeed);	
		}
		else
		{
			// Apply deceleration to character
			if (velocity != Vector2.Zero)
			{
				velocity = velocity.Lerp(Vector2.Zero, 10.0f * (float)delta);
			}
		}
		
		Velocity = velocity;
		
		MoveAndSlide();
	}
	
	private void OnHealthDepleted()
	{
		// Do stuff on character death
		QueueFree();
	}
	
	public override void _ExitTree()
	{
		healthComponent.HealthDepleted -= OnHealthDepleted;
	}
}
