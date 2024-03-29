using System;
using Godot;

public partial class FetchForData : Button
{
    // Called when the node enters the scene tree for the first time.
    private bool isLoading = false;

    private void _on_pressed()
    {
        //GetNode<Glob>("/root/Glob").ReadLines();
        int nop = GetNode<Glob>("/root/Glob").numberOfPlayer;
        if (nop == 5)
        {
            GetTree().ChangeSceneToFile("res://scene2.tscn");
        }
        else if (nop == 6)
        {
            GetTree().ChangeSceneToFile("res://scene3.tscn");
        }
        else if (!isLoading)
        {
            GetNode<Control>("../Loading").Visible = true;
            GetNode<Timer>("../Timer").Start();
            isLoading = true;
        }
        else if (nop > 6)
        {
            GetTree().ChangeSceneToFile("res://scene4.tscn");
        }
    }

    private void _on_timer_timeout()
    {
        _on_pressed();
    }
}
