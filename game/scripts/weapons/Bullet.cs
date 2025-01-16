using Godot;
using System;

public partial class Bullet : Area2D
{
	public float speed = 750;
	
	public float damage = 25.0f;
	
	public Vector2 direction;

	public override void _Ready()
	{
		this.Connect( SignalName.BodyEntered, Callable.From((Node2D body) => OnBodyEntered(body)) );
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Position += direction * speed * (float)delta;
	}

	public void OnBodyEntered(Node2D body)
	{
		QueueFree();
		HealthComponent healthComponent = body.GetNode<HealthComponent>("HealthComponent");
		healthComponent.TakeDamage(damage);
	}
}
