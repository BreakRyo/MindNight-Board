using System;
using Godot;

public partial class scene1 : Control
{
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    private void exitApp()
    {
        GetTree().Quit();
    }

    private void goToTutorial()
    {
        OS.ShellOpen("https://youtu.be/VNBNuD1yv9I?si=7AN-5uNSSS3S7BY5");
    }

    private void positionSelection(int position)
    {
        GetNode<Glob>("/root/Glob").myPosition = position;
    }
}
