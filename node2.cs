using Godot;
using System;

public partial class node2 : Node2D
{
	// Called when the node enters the scene tree for the first time.

	private void goBack(){
		GetTree().ChangeSceneToFile("res://scene1.tscn");
	}
}
