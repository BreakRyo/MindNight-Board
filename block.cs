using Godot;
using System;
using System.Linq;

public partial class block : Button
{
	// Called when the node enters the scene tree for the first time.
	private bool visi = true;
	public void _on_pressed(){
		var children = GetChildren().ToArray();
		foreach(var i in children){
			((Sprite2D)i).Visible=!visi;
		}
		visi=!visi;
	}

}
