using System;
using Godot;
using Microsoft.VisualBasic;

public partial class cards : Area2D
{
    private bool dragAble = false;
    private Vector2 of = Vector2.Zero;
    Godot.Collections.Array<Node> tableNodes;
    private Glob glob;

    public override void _Ready()
    {
        glob = GetNode<Glob>("/root/Glob");
        tableNodes = GetTree().GetNodesInGroup("table" + GetParent().Name.ToString()[3]);
        RichTextLabel cardName = (RichTextLabel)GetChild(1);
        if (glob.players.Count > 0)
            cardName.Text =
                "[center][color="
                + glob.players[Convert.ToInt32(Name) - 1].color
                + "]"
                + Convert.ToInt32(Name)
                + " "
                + glob.players[Convert.ToInt32(Name) - 1].name
                + "[/color][/center]";
    }

    public override void _Process(double delta)
    {
        if (dragAble && Input.IsActionPressed("mouse_left"))
        {
            if (GetNode<Glob>("/root/Glob").draging == "0")
            {
                GetNode<Glob>("/root/Glob").draging = Name;
                of = GetGlobalMousePosition() - Position;
            }
            if (GetNode<Glob>("/root/Glob").draging == Name)
                Position = GetGlobalMousePosition() - of;
        }
        else if (Input.IsActionJustReleased("mouse_left"))
        {
            GetNode<Glob>("/root/Glob").draging = "0";
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    private void _on_mouse_entered()
    {
        dragAble = true;
    }

    private void _on_mouse_exited()
    {
        dragAble = false;
    }

    private void _on_area_entered(Area2D ar)
    {
        foreach (var i in tableNodes)
        {
            if (i.Name == Name)
            {
                switch (ar.Name)
                {
                    case "Confirmed Hacker":
                        ((Sprite2D)i).Modulate = new Color("DB363A");
                        break;
                    case "Confirmed Agent":
                        ((Sprite2D)i).Modulate = new Color("5DCD50");
                        break;
                    case "Uncertain":
                        ((Sprite2D)i).Modulate = new Color("757C86");
                        break;
                    default:
                        GD.Print(ar.Name);
                        break;
                }
            }
        }
    }
}
