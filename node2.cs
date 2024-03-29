using System;
using Godot;

public partial class node2 : Node2D
{
    public override void _Ready()
    {
        GetNode<Glob>("/root/Glob").connect();
    }

    private void goBack()
    {
        GetTree().ChangeSceneToFile("res://scene1.tscn");
    }
}
