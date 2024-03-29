using System;
using Godot;

public partial class FocusMode : Button
{
    private bool focused = false;

    private void _on_pressed()
    {
        focused = !focused;
        GetWindow().Borderless = focused;
        GetWindow().Position = new Vector2I(0, 32);
    }
}
