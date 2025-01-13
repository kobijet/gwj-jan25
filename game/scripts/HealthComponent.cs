using Godot;
using System;

public partial class HealthComponent : Control
{
	[Export]
	public float maxHealth = 100.0f;
	public float health = 100.0f;
	
	[Signal]
	public delegate void HealthDepletedEventHandler();
	
	private ProgressBar healthBar;
	
	public override void _Ready()
	{
		healthBar = GetNode<ProgressBar>("HealthBar");
		healthBar.Value = health;
		healthBar.MaxValue = maxHealth;
	}
	
	public void TakeDamage(float amount)
	{
		GD.Print("Taking " + amount + " damage");
		health -= amount;
		
		healthBar.Value = health;
		
		if (health <= 0.0f)
		{
			GD.Print("Entity dead");
			EmitSignal(SignalName.HealthDepleted);
		}
	}
	
	public void HealDamage(float amount)
	{
		GD.Print("Healing " + amount + " damage");
		health += amount;
		
		healthBar.Value = health;
		
		if (health >= 100.0f)
		{
			health = 100.0f;
		}
	}
}
