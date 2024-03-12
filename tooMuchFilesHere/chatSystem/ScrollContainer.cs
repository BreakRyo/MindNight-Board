using Godot;
using System;

public partial class ScrollContainer : Godot.ScrollContainer
{


	private void scrollBottom(){
		
		ScrollVertical = (int)GetVScrollBar().MaxValue+10;
	}


}
