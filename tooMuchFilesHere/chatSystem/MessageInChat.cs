using System;
using Godot;

public partial class MessageInChat : RichTextLabel
{
    public override void _Ready() { }

    private void _on_check_box_toggled(bool state)
    {
        if (state)
        {
            Theme = ResourceLoader.Load<Theme>(
                "res://tooMuchFilesHere/chatSystem/message_in_chat1.tres"
            );
            SetMeta("Pinned", true);

            GetNode<Glob>("/root/Glob").changeChance(GetMeta("Player").As<int>(), true, 1);
        }
        else
        {
            Theme = ResourceLoader.Load<Theme>(
                "res://tooMuchFilesHere/chatSystem/message_in_chat.tres"
            );
            SetMeta("Pinned", false);

            GetNode<Glob>("/root/Glob").changeChance(GetMeta("Player").As<int>(), false, 1);
        }
    }
}
