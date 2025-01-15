using Godot;
using System;

public partial class Hud : CanvasLayer
{
	private GameManager GameManager;
	
	private Label gameClockText;
	private Button nextRoundButton;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameManager = GetNode<GameManager>("%GameManager");
		
		gameClockText = GetNode<Label>("GamePanel/GameClockText");
		
		nextRoundButton = GetNode<Button>("GamePanel/NextRoundButton");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		gameClockText.Text = GameManager.GetTimeLeftInRound().ToString();
	}
	
	public void EndRound()
	{
		gameClockText.Visible = false;
		nextRoundButton.Visible = true;
	}
	
	public void StartRound()
	{
		gameClockText.Visible = true;
		nextRoundButton.Visible = false;
	}
}
