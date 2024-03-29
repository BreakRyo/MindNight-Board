using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
public partial class chat : Control
{
	// Called when the node enters the scene tree for the first time.
	PackedScene messageInChat = GD.Load<PackedScene>("res://tooMuchFilesHere/chatSystem/message_in_chat.tscn");
	public static string fileLocation = "";
	int xMessages = 0;
	int xSystem = 0;
	public List<Glob.PlayerData> playersInChat = new List<Glob.PlayerData>();
	string[] messagesR = {
		"Sending SendChatPacket:",
		"Received ChatMessageReceive packet:"
	};

	Glob TheGlob;

	[Signal]
	public delegate void sendToSystemEventHandler();





	public override void _Ready()
	{
		TheGlob = GetNode<Glob>("/root/Glob");
		fileLocation = TheGlob.fileLocation;
		xMessages = TheGlob.startIndex;
		xSystem = xMessages;
		playersInChat = TheGlob.players;
		organizer();
		//watch();
		GetNode<Godot.Timer>("Timer").Start();
		;
		
	}

	
	private void addMessage(string name, string txt){
		
		RichTextLabel msmInst = (RichTextLabel)messageInChat.Instantiate();
		msmInst.Text = "["+name.Substring(8,name.Length-17)+"]: "+txt;
		GetNode<VBoxContainer>("Control/Panel1/ScrollContainer/VBoxContainer").AddChild(msmInst);
	}





	public void organizer(){
		
		List<string> playerLog = File.ReadAllLines(
			System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData)+@"/Code Why/GameLogs/Player.log"
		).ToList();

		var system = playerLog.GetRange(xSystem+1,playerLog.Count-xSystem-1).Where(
			i => i.Contains("Received SelectPhaseEnd packet") || i.Contains("Received VotePhaseEnd packet") || i.Contains("Received MissionPhaseEnd packet")
			);
		var messages = playerLog.GetRange(xMessages+1,playerLog.Count-xMessages-1).Where(i => i.Contains("Received ChatMessageReceive packet:"));

		// TODO{ this needs to go inside second if }


		if( messages.Count() > 0 )
		{
			xMessages = playerLog.LastIndexOf(messages.Last());
		
			string name = " ";
			string txt = " ";
			
			foreach(var i in messages)
			{	
				JsonDocument doc = JsonDocument.Parse(i.Substring(56));
				JsonElement root = doc.RootElement;
				name = playersInChat[root.GetProperty("Slot").GetInt32()].name;
				txt = root.GetProperty("Message").GetString();
				if(txt.Contains('<')){
					txt = filtherEmoji(txt);
				}
				doc.Dispose();
				CallDeferred("addMessage",name,txt);
				
			}
		}
		if( system.Count() > 0)
		{
			xSystem = playerLog.LastIndexOf(system.Last());
			foreach(var i in system)
			{
				SystemBlock block = new SystemBlock();
				if ( i.Contains("Received SelectPhaseEnd packet"))
				{
					JsonDocument doc = JsonDocument.Parse(i.Substring(52));
					JsonElement root = doc.RootElement;
					block.passed = root.GetProperty("Passed").GetBoolean();
					block.propose = root.GetProperty("SelectedTeam").EnumerateArray().Select(item => item.GetInt32()).ToArray();
					block.proposer = root.GetProperty("Proposer").GetInt32();
					doc.Dispose();
					block.messageType = 's';
					// send block
					TheGlob.systemMessages.Add(block);
					EmitSignal("sendToSystem");

				}
				else if (i.Contains("Received VotePhaseEnd packet"))
				{
					JsonDocument doc = JsonDocument.Parse(i.Substring(50));
					JsonElement root = doc.RootElement;
					block.accepted = root.GetProperty("VotesFor").EnumerateArray().Select(item => item.GetInt32()).ToArray();
					block.refused = root.GetProperty("VotesAgainst").EnumerateArray().Select(item => item.GetInt32()).ToArray();
					block.waitForOptional = root.GetProperty("Passed").GetBoolean();
					doc.Dispose();
					block.messageType = 'v';
					// send block
					TheGlob.systemMessages.Add(block);
					EmitSignal("sendToSystem");
				}
				else if (i.Contains("Received MissionPhaseEnd packet"))
				{
					JsonDocument doc = JsonDocument.Parse(i.Substring(53));
					JsonElement root = doc.RootElement;
					block.proposer = root.GetProperty("Proposer").GetInt32();
					block.hacks = root.GetProperty("NumHacks").GetInt32();
					doc.Dispose();
					block.messageType = 'm';
					// send block
					TheGlob.systemMessages.Add(block);
					EmitSignal("sendToSystem");
				}
			}
		}
		

	}

	private string filtherEmoji(string txt){
		var tx = txt;
		
		foreach(char i in tx){
			if(i=='<')
			{
				var preTX = tx.Substring( tx.IndexOf('<'), tx.IndexOf('>') - tx.IndexOf('<')+1 );

				tx = tx.Replace( preTX , img[sprites.ToList().IndexOf( preTX )] );
			}
		}
		return tx;

	}
	DirAccess dir = DirAccess.Open("user://");
	private void _on_timeout(){
		dir.Copy(fileLocation+@"\Player.log","user://GameLogs/Player.log");
		organizer();
	}
	

	private bool chatVisibility = false;

	private void _on_chat_pressed(){
		

		((Control)GetChild(0)).Visible = !chatVisibility;

		chatVisibility = !chatVisibility;
	}


	private bool systemVisibility = false;
	private void _on_prop_pressed(){

		
		((Control)GetChild(1)).Visible = !systemVisibility;

		systemVisibility = !systemVisibility;
	}
















	// this is junk i don't wanna look at.
	private string[] sprites = {
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
	private string[] img = {
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
