using Godot;
using System;

public partial class Positions : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void positionSelection(){
		GetNode<Glob>("/root/Glob").myPosition = Convert.ToInt32( Name );
		GetTree().ChangeSceneToFile("res://scene2.tscn");
	}
}
