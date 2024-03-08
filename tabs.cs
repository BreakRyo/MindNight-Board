using Godot;
using System;

public partial class tabs : Button
{


	private void _on_pressed(){
		Theme = ResourceLoader.Load<Theme>("res://junk/themes/buttontheme.tres");
		switch(Name){
			case "original":
				GetNode<Button>("../unoriginal").Theme = ResourceLoader.Load<Theme>("res://junk/themes/buttontheme1.tres");
				GetNode<Node2D>("../Tab2").Hide();
				GetNode<Node2D>("../Tab2").ProcessMode = ProcessModeEnum.Disabled;
				GetNode<Node2D>("../Tab1").Show();
				GetNode<Node2D>("../Tab1").ProcessMode = ProcessModeEnum.Inherit;

			break;
			case "unoriginal":
				GetNode<Button>("../original").Theme = ResourceLoader.Load<Theme>("res://junk/themes/buttontheme1.tres");
				GetNode<Node2D>("../Tab1").ProcessMode = ProcessModeEnum.Disabled;
				GetNode<Node2D>("../Tab2").Show();
				GetNode<Node2D>("../Tab2").ProcessMode = ProcessModeEnum.Inherit;
			break;
		}
	}
}
