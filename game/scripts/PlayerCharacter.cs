using Godot;
using System;

public partial class PlayerCharacter : CharacterBody2D
{
	// Movement variables
	[Export] public float speed { get; set; } = 10.0f;
	[Export] public float maxSpeed { get; set; } = 250.0f;
	private Vector2 direction;
	
	private HealthComponent healthComponent;

	public override void _Ready()
	{	
		healthComponent = GetNode<HealthComponent>("HealthComponent");
		
		// Subscribe to health depleted event
		healthComponent.HealthDepleted += OnHealthDepleted;
	}

	public override void _PhysicsProcess(double delta)
	{
		MovePlayer(delta);
	}
	
	// Get input updates the character's direction,
	// returns true if there is user input,
	// false when all keys are released
	public bool GetInput()
	{
		direction = Input.GetVector("left", "right", "up", "down");
		
		return (direction == Vector2.Zero ? false : true);
	}
	
	// If player is pressing an input key, or multiple, move in the normalized
	// direction of the input. If they aren't pressing anything,
	// decelerates them to zero
	private void MovePlayer(double delta)
	{
		GetInput();
		
		Vector2 velocity = Velocity;
		
		// Check if player is pressing an input key
		// GetInput() can be replaced by any bool condition
		if (GetInput())
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
	}
	
	public override void _ExitTree()
	{
		healthComponent.HealthDepleted -= OnHealthDepleted;
	}
}
