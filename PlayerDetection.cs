using Godot;
using System;

public partial class PlayerDetection : Button
{
	
	private void _on_pressed(){
		GetNode<Glob>("/root/Glob").numberOfPlayer = Name.ToString()[0];
		
		Theme = ResourceLoader.Load<Theme>("res://junk/themes/buttontheme.tres");
		switch(Name){
			case "6P":
			GetNode<Button>("../5P").Theme = ResourceLoader.Load<Theme>("res://junk/themes/buttontheme1.tres");
			GetNode<Button>("../6").Visible = true;
			break;
			case "5P":
			GetNode<Button>("../6P").Theme = ResourceLoader.Load<Theme>("res://junk/themes/buttontheme1.tres");
			GetNode<Button>("../6").Visible = false;
			break;
		}
	}
}
