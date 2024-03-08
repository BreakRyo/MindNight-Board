using Godot;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public partial class scene2 : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		int mypos = GetNode<Glob>("/root/Glob").myPosition;
		var tableNodes = GetTree().GetNodesInGroup("table1");
		foreach(var i in tableNodes){
			if(i.Name==mypos.ToString()){
				((Button)i.GetParent()).Hide();
			}
		}
		var cards = GetTree().GetNodesInGroup("cards1");
		((Area2D)cards[mypos-1]).Hide();
		

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


}
