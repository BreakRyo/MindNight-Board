using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class ScrollContainer : Godot.ScrollContainer
{
    PackedScene messageInChat = GD.Load<PackedScene>(
        "res://tooMuchFilesHere/chatSystem/message_in_chat.tscn"
    );
    private List<MessageStructure> messages = new List<MessageStructure>();
    private VBoxContainer container;

    public override void _Ready()
    {
        container = GetNode<VBoxContainer>("VBoxContainer");
    }

    private void scrollBottom()
    {
        ScrollVertical = (int)GetVScrollBar().MaxValue + 10;
    }

    private void addNewMessage(string talkerName, string txt, int talkerID)
    {
        RichTextLabel msmInst = (RichTextLabel)messageInChat.Instantiate();
        msmInst.Text = "[" + talkerName + "]: " + txt;
        MessageStructure newMsm = new MessageStructure
        {
            talker = talkerName,
            message = txt,
            talkerId = talkerID
        };

        msmInst.SetMeta("Player", talkerID);
        messages.Add(newMsm);
        container.AddChild(msmInst);
    }

    private void filterMessages(int filterOption)
    {
        switch (filterOption)
        {
            case 0:
                filterThis(Enumerable.Range(0, 10).ToList());
                break;
            case 1:
                foreach (var i in container.GetChildren())
                {
                    if (!i.GetMeta("Pinned").AsBool())
                    {
                        ((RichTextLabel)i).Visible = false;
                    }
                    else
                    {
                        ((RichTextLabel)i).Visible = true;
                    }
                }
                break;
            default:
                int[] tst = { filterOption - 3 };
                filterThis(tst.ToList());
                break;
        }
    }

    private void filterThis(List<int> wannaSee)
    {
        foreach (var i in container.GetChildren())
        {
            if (!wannaSee.Contains(i.GetMeta("Player").As<int>()))
            {
                ((RichTextLabel)i).Visible = false;
            }
            else
            {
                ((RichTextLabel)i).Visible = true;
            }
        }
    }

    class MessageStructure
    {
        public string talker;
        public string message;
        public int talkerId;
    }
}
