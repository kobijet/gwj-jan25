using Godot;
using System;

public partial class PlayerCharacter : CharacterBody2D
{
	[Export]
	public float acceleration = 30.0f;
	
	[Export]
	public float deceleration = 20.0f;
	
	[Export]
	public float maxSpeed = 4.0f;

	public override void _PhysicsProcess(double delta)
	{	
		MovePlayer(delta);
	}
	
	private void MovePlayer(double delta)
	{
		// Gets normalized direction
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
		Vector2 velocity = Velocity;
		
		velocity += direction * acceleration * (float)delta;
		
		if (velocity != Vector2.Zero)
		{
			velocity = velocity.MoveToward(Vector2.Zero, deceleration * (float)delta);
		}
		
		GD.Print(velocity);
		
		// Keep speed at or below max speed
		velocity.X = Mathf.Clamp(velocity.X, -maxSpeed, maxSpeed);
		velocity.Y = Mathf.Clamp(velocity.Y, -maxSpeed, maxSpeed);
		Velocity = velocity;
		
		MoveAndCollide(velocity);	
	}
}
