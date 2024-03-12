using Godot;
using System;

public partial class MessageInChat : RichTextLabel
{

    public override void _Ready()
    {
		
    }

    private void _on_check_box_toggled(bool state){
		
		if(state )
		{
			Theme =  ResourceLoader.Load<Theme>("res://tooMuchFilesHere/chatSystem/message_in_chat1.tres");
			
		}
		else 
		{
			Theme =  ResourceLoader.Load<Theme>("res://tooMuchFilesHere/chatSystem/message_in_chat.tres");
		}
		
	}
	
}
