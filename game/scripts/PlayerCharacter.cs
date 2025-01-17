using Godot;
using System;

public partial class PlayerCharacter : CharacterBody2D
{
	// Movement variables
	[Export] public float speed { get; set; } = 10.0f;
	[Export] public float maxSpeed { get; set; } = 250.0f;
	private Vector2 direction;
	
	private HealthComponent healthComponent;
	
	private enum WeaponTypes {
		MELEE,
		RANGED,
		BOMB,
		UTILITY
	}
	private WeaponTypes activeWeapon;
	
	private PackedScene meleeWeaponScene;
	private PackedScene rangedWeaponScene;
	private PackedScene bombScene;
	
	private Node2D weapon;
	private Marker2D weaponPivot;
	
	private AnimatedSprite2D playerSprite;

	public override void _Ready()
	{	
		healthComponent = GetNode<HealthComponent>("HealthComponent");
		
		// Subscribe to health depleted event
		healthComponent.HealthDepleted += OnHealthDepleted;
		
		// Load base weapon scenes
		meleeWeaponScene = ResourceLoader.Load<PackedScene>("res://game/scenes/weapons/melee_weapon.tscn");
		rangedWeaponScene = ResourceLoader.Load<PackedScene>("res://game/scenes/weapons/ranged_weapon.tscn");
		bombScene = ResourceLoader.Load<PackedScene>("res://game/scenes/weapons/bomb.tscn");
		
		weaponPivot = GetNode<Marker2D>("WeaponPivot");
		
		playerSprite = GetNode<AnimatedSprite2D>("Sprite");
		
		// Create melee weapon and add instantiate as a child of the weapon pivot
		SwapWeapons(WeaponTypes.BOMB);
		
	}

	public override void _Process(double delta)
	{
		// Hot swap keys for testing
		if (Input.IsActionJustPressed("swap_1"))
		{
			SwapWeapons(WeaponTypes.MELEE);
		}
		if (Input.IsActionJustPressed("swap_2"))
		{
			SwapWeapons(WeaponTypes.RANGED);
		}
		if (Input.IsActionJustPressed("swap_3"))
		{
			SwapWeapons(WeaponTypes.BOMB);
		}
		
		
		// Allow creation of bombs if bomb weapon is active
		if (activeWeapon == WeaponTypes.BOMB)
		{
			if (Input.IsActionJustPressed("attack_1"))
			{
				Node2D bomb = (Node2D)bombScene.Instantiate();
				GetNode("/root/Game").AddChild(bomb);
				Bomb bombScript = bomb as Bomb;
				bombScript.StartTimer();
				bomb.Position = Position;
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		MovePlayer(delta);
		
		// Get mouse position and direction from character to mouse position
		Vector2 lookDirection = GetGlobalMousePosition() - GlobalPosition;
		lookDirection = lookDirection.Normalized();
		
		FlipSprite(lookDirection);
	}
	
	// Updates the character's direction,
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
			playerSprite.Play(); // Play running animation
			
			// Apply movement in direction
			velocity = direction * maxSpeed;
			
			velocity = velocity.Clamp(-maxSpeed, maxSpeed);
		}
		else
		{
			playerSprite.Stop(); // Stop running animation
			
			// Apply deceleration to character
			if (velocity != Vector2.Zero)
			{
				velocity = velocity.Lerp(Vector2.Zero, 10.0f * (float)delta);
			}
		}
		
		Velocity = velocity;
		
		MoveAndSlide();
	}
	
	// Instantiates a new weapon in the weapon slot and destroys the old one
	private void SwapWeapons(WeaponTypes weaponType)
	{
		if (weaponPivot.GetChildCount() != 0 )
		{
			weapon.QueueFree();
		}
		
		activeWeapon = weaponType;
		
		// Set weapon to the proper scene for display
		switch (weaponType)
		{
			case WeaponTypes.MELEE:
				weapon = (Node2D)meleeWeaponScene.Instantiate();
				break;
			case WeaponTypes.RANGED:
				weapon = (Node2D)rangedWeaponScene.Instantiate();
				break;
			case WeaponTypes.BOMB:
				weapon = (Node2D)bombScene.Instantiate();
				break;
		}
		
		weaponPivot.AddChild(weapon);
	}
	
	private void FlipSprite(Vector2 lookDirection)
	{
		// Flip character, weapon, and child nodes to side the mouse cursor is on
		if (lookDirection.X > 0.0f)
		{
			//
			// Facing to the right
			//
			playerSprite.FlipH = false;
			weapon.GetNode<Sprite2D>("Sprite").FlipH = true;
			weaponPivot.Position = new Vector2(7, 0);
			
			// Swap sides of melee collision box
			if (activeWeapon == WeaponTypes.MELEE)
			{
				weapon.GetNode<CollisionShape2D>("AttackArea/CollisionShape2D").Position = new Vector2(20, -9);	
			}
			
			// Swap sides for bullets to shoot from
			if (activeWeapon == WeaponTypes.RANGED)
			{
				weapon.GetNode<Marker2D>("Tip").Position = new Vector2(-7, 1);
			}
		}
		else
		{
			//
			// Facing to the left
			//
			
			playerSprite.FlipH = true;
			weapon.GetNode<Sprite2D>("Sprite").FlipH = false;
			weaponPivot.Position = new Vector2(-7, 0);
			
			// Swap sides of melee collision box
			if (activeWeapon == WeaponTypes.MELEE)
			{
				weapon.GetNode<CollisionShape2D>("AttackArea/CollisionShape2D").Position = new Vector2(-20, -9);	
			}
			
			// Swap sides for bullets to shoot from
			if (activeWeapon == WeaponTypes.RANGED)
			{
				weapon.GetNode<Marker2D>("Tip").Position = new Vector2(7, 1);
			}
		}
	}
	
	private void OnHealthDepleted()
	{
		// Do stuff on character death
		GD.Print("Player died! Trigger game over.");
	}
	
	public override void _ExitTree()
	{
		healthComponent.HealthDepleted -= OnHealthDepleted;
	}
}
