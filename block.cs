using Godot;
using System;

public partial class block : Button
{
	// Called when the node enters the scene tree for the first time.
	private bool visi = true;
	private void _on_pressed(){
		var children = GetChildren();
		foreach (var item in children)
		{

			((Sprite2D)item).Visible=!visi;
		}
		visi=!visi;
	}

}
