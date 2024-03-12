using Godot;
using System;

public partial class tabs : Button
{

	private Button unoriginal;
	private Button original;
	private Node2D tab1; 
	private Node2D tab2;
    public override void _Ready()
    {
        unoriginal = GetNode<Button>("../unoriginal");
        original = GetNode<Button>("../original");
		tab1 = GetNode<Node2D>("../Tab1");
		tab2 = GetNode<Node2D>("../Tab2");
    }

    private void _on_pressed(){
		Theme = ResourceLoader.Load<Theme>("res://junk/themes/buttontheme.tres");
		switch(Name){
			case "original":
				unoriginal.Theme = ResourceLoader.Load<Theme>("res://junk/themes/buttontheme1.tres");
				tab2.Hide();
				tab2.ProcessMode = ProcessModeEnum.Disabled;
				tab1.Show();
				tab1.ProcessMode = ProcessModeEnum.Inherit;

			break;
			case "unoriginal":
				original.Theme = ResourceLoader.Load<Theme>("res://junk/themes/buttontheme1.tres");
				tab1.Hide();
				tab1.ProcessMode = ProcessModeEnum.Disabled;
				tab2.Show();
				tab2.ProcessMode = ProcessModeEnum.Inherit;
			break;
		}
	}
}
