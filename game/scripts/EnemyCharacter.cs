using Godot;
using System;

public partial class EnemyCharacter : CharacterBody2D
{
	// Movement variables
	[Export]
	public float speed { get; set; } = 5.0f;
	private Vector2 direction;
	
	private Vector2 targetPos = new Vector2(300.0f, 100.0f);
	private float distanceToTarget;

	// GetDistanceToTarget updates the character's direction,
	// returns true if character needs to move towards target,
	// false when character is within range of target
	public bool GetDistanceToTarget()
	{
		direction = targetPos - Position;
		
		// Calculate distance from character to target
		distanceToTarget = Mathf.Sqrt( Mathf.Pow(targetPos.X - Position.X, 2) + Mathf.Pow(targetPos.Y - Position.Y, 2) );
		GD.Print(distanceToTarget);
		
		direction = direction.Normalized();
		
		return (direction == Vector2.Zero ? false : true);
	}

	public override void _PhysicsProcess(double delta)
	{	
		Vector2 velocity = Velocity;
		
		// If enemy isn't at target location
		// GetInput() can be replaced by any bool condition
		if (GetDistanceToTarget())
		{
			// Apply movement in direction
			velocity += direction * speed * (float)delta;
		}
		else
		{
			// Apply deceleration to character
			if (velocity != Vector2.Zero)
			{
				velocity = velocity.MoveToward(Vector2.Zero, 100.0f * (float)delta);
			}
		}
		
		Velocity = velocity;
		
		MoveAndSlide();
	}
}
