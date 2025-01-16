using Godot;
using System;

public partial class RangedWeapon : Node2D
{
	[Export] public float damage = 25.0f;
	
	private PackedScene bulletScene;
	private Marker2D tip;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		bulletScene = ResourceLoader.Load<PackedScene>("res://game/scenes/weapons/bullet.tscn");
		
		tip = GetNode<Marker2D>("Tip");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Get mouse position and direction from character to mouse position
		Vector2 lookDirection = GetGlobalMousePosition() - GlobalPosition;
		lookDirection = lookDirection.Normalized();
		
		if (Input.IsActionJustPressed("attack_1"))
		{
			// Set bullet damage and spawn a bullet
			Node2D bullet = (Node2D)bulletScene.Instantiate();
			Bullet bulletScript = bullet as Bullet;
			bulletScript.damage = damage;
			bulletScript.direction = lookDirection;
			GetNode("/root/Game").AddChild(bullet);
			bullet.GlobalPosition = tip.GlobalPosition;
		}
	}
}
