using Godot;
using System;

public partial class HealthComponent : Control
{
	// Health bar variables
	[Export] public float maxHealth = 100.0f;
	private float health = 100.0f;
	
	// UI
	private ProgressBar healthBar;
	
	// Events
	[Signal] public delegate void HealthDepletedEventHandler();
	
	public override void _EnterTree()
	{	
		health = maxHealth;
		
		// Setup health bar values
		healthBar = GetNode<ProgressBar>("HealthBar");
		healthBar.Value = maxHealth;
		healthBar.MaxValue = maxHealth;
	}
	
	// Take damage and update healthbar
	public void TakeDamage(float amount)
	{
		health -= amount;
		
		healthBar.Value = health;
		
		if (health <= 0)
		{
			// Emit health signal
			EmitSignal(SignalName.HealthDepleted);
		}
	}
	
	// Heal damage and update healthbar
	public void HealDamage(float amount)
	{
		health += amount;
		
		healthBar.Value = health;
		
		if (health >= 100.0f)
		{
			health = 100.0f;
		}
	}
}
