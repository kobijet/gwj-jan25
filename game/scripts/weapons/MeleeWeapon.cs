using Godot;
using System;
using System.Collections.Generic;

public partial class MeleeWeapon : Node2D
{
	[Export] public float damage = 50.0f;
	
	private Area2D attackArea;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		attackArea = GetNode<Area2D>("AttackArea");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Godot.Collections.Array<Node2D> overlapping = attackArea.GetOverlappingBodies();
	
		if (Input.IsActionJustPressed("attack_1"))
		{
			for (int i = 0; i < overlapping.Count; i++)
			{
				HealthComponent healthComponent = overlapping[i].GetNode<HealthComponent>("HealthComponent");
				healthComponent.TakeDamage(damage);
			}
		}
	}
}
