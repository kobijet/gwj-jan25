using Godot;
using System;

public partial class Bullet : Area2D
{
	public float speed = 750;
	
	public float damage = 25.0f;
	
	public Vector2 direction;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Position += direction * speed * (float)delta;
	}

	public void OnBodyEntered()
	{
		GD.Print("Hit!");
	}
}
