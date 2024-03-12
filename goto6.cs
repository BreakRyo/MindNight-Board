using Godot;
using System;

public partial class goto6 : Button
{
	private void _on_pressed(){
		GetTree().ChangeSceneToFile("res://scene3.tscn");
	}
}
