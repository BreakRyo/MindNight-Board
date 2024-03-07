using Godot;
using System;
using System.Text.RegularExpressions;

public partial class scene2 : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		int mypos = GetNode<Glob>("/root/Glob").myPosition;
		var tableNodes = GetTree().GetNodesInGroup("table");
		foreach(var i in tableNodes){
			if(i.Name==mypos.ToString()){
				i.GetParent().Call("_on_pressed");
				i.GetParent().ProcessMode = ProcessModeEnum.Disabled;
			}
		}
		var cards = GetTree().GetNodesInGroup("cards");
		((Area2D)cards[mypos-1]).Hide();


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void exitApp(){
		GetTree().Quit();
	}
	private void goBack(){
		GetTree().ChangeSceneToFile("res://scene1.tscn");
	}
}
