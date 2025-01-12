using Godot;
using System;

public partial class EnemyCharacter : CharacterBody2D
{
	// Player movement variables
	[Export]
	public float acceleration = 30.0f;
	[Export]
	public float deceleration = 20.0f;
	[Export]
	public float maxSpeed = 1.0f;

	// Enemy has a target point that they always try and move towards,
	// whether it's the base or the player.
	private Vector2 target;

	public override void _PhysicsProcess(double delta)
	{	
		MoveCharacter(delta);
	}
	
	private void MoveCharacter(double delta)
	{
		Vector2 direction = new Vector2(1.0f, 0.0f);
		
		Vector2 velocity = Velocity;
		
		velocity += direction * acceleration * (float)delta;
		
		if (velocity != Vector2.Zero)
		{
			velocity = velocity.MoveToward(Vector2.Zero, deceleration * (float)delta);
		}
		
		// Keep speed clamped to maxSpeed in both directions
		velocity.X = Mathf.Clamp(velocity.X, -maxSpeed, maxSpeed);
		velocity.Y = Mathf.Clamp(velocity.Y, -maxSpeed, maxSpeed);
		
		Velocity = velocity;
		
		MoveAndCollide(velocity);	
	}
}
