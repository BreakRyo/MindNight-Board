using Godot;
using System;

public partial class NodePad : TextEdit
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var names = GetNode<Glob>("/root/Glob").players;
		var hightlight = GD.Load<CodeHighlighter>("res://tooMuchFilesHere/chatSystem/new_code_highlighter.tres");
		//foreach(var name in names){
		//	hightlight.AddKeywordColor(name.name.Substring(26,name.name.Length-43).ToLower(),Color.FromHtml(name.name.Substring(15,7)));
		//	hightlight.AddKeywordColor(name.name.Substring(26,name.name.Length-43),Color.FromHtml(name.name.Substring(15,7)));
		//	hightlight.AddKeywordColor(name.name.Substring(23,1),Color.FromHtml(name.name.Substring(15,7)));
		//}
	}


}
