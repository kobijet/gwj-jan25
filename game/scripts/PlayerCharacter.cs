using Godot;
using System;

public partial class PlayerCharacter : CharacterBody2D
{
	// Movement variables
	[Export]
	public float speed { get; set; } = 175.0f;
	private Vector2 direction;

	public override void _Ready()
	{
		HealthComponent healthComponent = GetNode<HealthComponent>("HealthComponent");
		healthComponent.TakeDamage(100.0f);
		healthComponent.HealthDepleted += OnHealthDepleted;
	}

	// Get input updates the character's direction,
	// returns true if there is user input,
	// false when all keys are released
	public bool GetInput()
	{
		direction = Input.GetVector("left", "right", "up", "down");
		
		return (direction == Vector2.Zero ? false : true);
	}

	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		
		Vector2 velocity = Velocity;
		
		// If player is pressing an input key
		// GetInput() can be replaced by any bool condition
		if (GetInput())
		{
			// Apply movement in direction
			velocity += direction * speed * (float)delta;
			
			velocity = velocity.Clamp(-speed, speed);
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
		GD.Print("Player is dead!");
	}
}
