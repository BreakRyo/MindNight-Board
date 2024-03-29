using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using Godot;

public partial class chat : Control
{
    public List<Glob.PlayerData> playersInChat = new List<Glob.PlayerData>();
    string[] messagesR = { "Sending SendChatPacket:", "Received ChatMessageReceive packet:" };

    Glob TheGlob;
    private ScrollContainer messageContainer;
    private OptionButton filterPlayerButton;
    private Control systemControl;

    public override void _Ready()
    {
        TheGlob = GetNode<Glob>("/root/Glob");
        messageContainer = GetNode<ScrollContainer>("Control/Panel1/ScrollContainer");
        filterPlayerButton = GetNode<OptionButton>("Control/OptionButton");
        systemControl = GetNode<Control>("System");

        playersInChat = TheGlob.players;
        var lines = TheGlob.newlines;

        addItemsToPlayersButton();

        foreach (var line in lines)
        {
            organizer(line);
        }
    }

    private void addMessage(int talkerId, string txt)
    {
        string talkerName =
            "[color="
            + playersInChat[talkerId].color
            + "]"
            + playersInChat[talkerId].name
            + "[/color]";
        messageContainer.Call("addNewMessage", talkerName, txt, talkerId);
    }

    public void organizer(string newline)
    {
        // [1] this reads file
        if (newline.Contains("Received ChatMessageReceive packet:"))
        {
            // call chat organizer
            int name = 0;
            string txt = " ";
            JsonDocument doc = JsonDocument.Parse(newline.Substring(56));
            JsonElement root = doc.RootElement;
            name = root.GetProperty("Slot").GetInt32();
            txt = root.GetProperty("Message").GetString();
            if (txt.Contains('<'))
            {
                txt = filtherEmoji(txt);
            }
            doc.Dispose();
            CallDeferred("addMessage", name, txt);
        }
        else if (
            newline.Contains("Received SelectPhaseEnd packet")
            || newline.Contains("Received VotePhaseEnd packet")
            || newline.Contains("Received MissionPhaseEnd packet")
        )
        {
            // call system organizer
            SystemBlock block = new SystemBlock();
            if (newline.Contains("Received SelectPhaseEnd packet"))
            {
                JsonDocument doc = JsonDocument.Parse(newline.Substring(52));
                JsonElement root = doc.RootElement;
                block.passed = root.GetProperty("Passed").GetBoolean();
                block.propose = root.GetProperty("SelectedTeam")
                    .EnumerateArray()
                    .Select(item => item.GetInt32())
                    .ToArray();
                block.proposer = root.GetProperty("Proposer").GetInt32();
                doc.Dispose();
                block.messageType = 's';
                // send block
                TheGlob.systemMessages.Add(block);

                systemControl.CallDeferred("newSystemMessage");
            }
            else if (newline.Contains("Received VotePhaseEnd packet"))
            {
                JsonDocument doc = JsonDocument.Parse(newline.Substring(50));
                JsonElement root = doc.RootElement;
                block.accepted = root.GetProperty("VotesFor")
                    .EnumerateArray()
                    .Select(item => item.GetInt32())
                    .ToArray();
                block.refused = root.GetProperty("VotesAgainst")
                    .EnumerateArray()
                    .Select(item => item.GetInt32())
                    .ToArray();
                block.waitForOptional = root.GetProperty("Passed").GetBoolean();
                doc.Dispose();
                block.messageType = 'v';
                // send block
                TheGlob.systemMessages.Add(block);

                systemControl.CallDeferred("newSystemMessage");
            }
            else if (newline.Contains("Received MissionPhaseEnd packet"))
            {
                JsonDocument doc = JsonDocument.Parse(newline.Substring(53));
                JsonElement root = doc.RootElement;
                block.proposer = root.GetProperty("Proposer").GetInt32();
                block.hacks = root.GetProperty("NumHacks").GetInt32();
                doc.Dispose();
                block.messageType = 'm';
                // send block
                TheGlob.systemMessages.Add(block);

                systemControl.CallDeferred("newSystemMessage");
            }
        }
    }

    private string filtherEmoji(string txt)
    {
        var tx = txt;

        foreach (char i in tx)
        {
            if (i == '<')
            {
                var preTX = tx.Substring(tx.IndexOf('<'), tx.IndexOf('>') - tx.IndexOf('<') + 1);

                tx = tx.Replace(preTX, img[sprites.ToList().IndexOf(preTX)]);
            }
        }
        return tx;
    }

    private void _on_chat_pressed()
    {
        toolVisibility("Control");
    }

    private void _on_note_pressed()
    {
        toolVisibility("NotePad");
    }

    private void _on_prop_pressed()
    {
        toolVisibility("System");
    }

    private void _on_vision_pressed()
    {
        toolVisibility("Vision");
    }

    private void toolVisibility(string target)
    {
        foreach (var i in GetTree().GetNodesInGroup("Tools"))
        {
            if (i.Name.ToString() != target)
                ((Control)i).Visible = false;
        }
        ((Control)GetNode(target)).Visible = !((Control)GetNode(target)).Visible;
    }

    private void addItemsToPlayersButton()
    {
        filterPlayerButton.AddItem("All");
        filterPlayerButton.AddItem("Pinned");
        filterPlayerButton.AddSeparator("PLAYERS");
        foreach (var i in TheGlob.players)
        {
            filterPlayerButton.AddItem(i.name);
        }
    }

    // this is junk i don't wanna look at.
    private string[] sprites =
    {
        "<sprite name=\"emoji_angry\">",
        "<sprite name=\"emoji_big_smile\">",
        "<sprite name=\"emoji_broken_heart\">",
        "<sprite name=\"emoji_cactus\">",
        "<sprite name=\"emoji_celebrate\">",
        "<sprite name=\"emoji_cry\">",
        "<sprite name=\"emoji_cute\">",
        "<sprite name=\"emoji_devil\">",
        "<sprite name=\"emoji_hammer\">",
        "<sprite name=\"emoji_heart\">",
        "<sprite name=\"emoji_hypnotized\">",
        "<sprite name=\"emoji_kiss\">",
        "<sprite name=\"emoji_mindblown\">",
        "<sprite name=\"emoji_nervous\">",
        "<sprite name=\"emoji_ok\">",
        "<sprite name=\"emoji_sad_face\">",
        "<sprite name=\"emoji_saint\">",
        "<sprite name=\"emoji_sleepy\">",
        "<sprite name=\"emoji_smile\">",
        "<sprite name=\"emoji_smug\">",
        "<sprite name=\"emoji_snowman\">",
        "<sprite name=\"emoji_straight_face\">",
        "<sprite name=\"emoji_surprised\">",
        "<sprite name=\"emoji_suspicious\">",
        "<sprite name=\"emoji_thinking\">",
        "<sprite name=\"emoji_thumbs_down\">",
        "<sprite name=\"emoji_thumbs_up\">",
        "<sprite name=\"emoji_time\">",
        "<sprite name=\"emoji_tongue\">",
        "<sprite name=\"emoji_wink\">"
    };
    private string[] img =
    {
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_angry.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_big_smile.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_broken_heart.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_cactus.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_celebrate.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_cry.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_cute.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_devil.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_hammer.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_heart.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_hypnotized.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_kiss.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_mindblown.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_nervous.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_ok.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_sad_face.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_saint.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_sleepy.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_smile.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_smug.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_snowman.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_straight_face.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_surprised.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_suspicious.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_thinking.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_thumbs_down.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_thumbs_up.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_time.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_tongue.png[/img]",
        "[img]res://tooMuchFilesHere/emojisIFoundAround/emoji_wink.png[/img]"
    };
}
