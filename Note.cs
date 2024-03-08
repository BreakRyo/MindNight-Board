using Godot;
using System;

public partial class Note : Button
{
	private bool visi = false;
	private Control n;
    public override void _Ready()
    {
        n = GetNode<Control>("../NotePad");
    }
    private void _on_pressed(){
		n.Visible = !visi;
		visi = !visi;
		if(visi) n.ProcessMode = ProcessModeEnum.Inherit;
		else n.ProcessMode = ProcessModeEnum.Disabled;
	}
}
