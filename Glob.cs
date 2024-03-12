using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public partial class Glob : Node
{
	// v data
	public int myPosition = 1;
	public string draging = "0";
	public int numberOfPlayer = 0 ;
	public int startIndex = 0;
	public string[] lineType = {
		"Received GameFound packet:", // PlayerNumber : int
		"Received SpawnPlayer packet:", // Slot : int (where IsLocal==true), Color : int, Username : string
	};
	public List<PlayerData> players = new List<PlayerData>();


	// c data
	string[] colors = {"#00A6F6","#D31FD4","#6FE015","#9D9D9D","#FF8113","#FFEC16","#00B48B","#0041F6"};

	

	// should red player log here and get : {my position, names, colors, number of players} and update v dara.
	
	public string fileLocation = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
	string[] lines;
    public override void _Ready()
    {
		fileLocation = fileLocation.Substring(0,fileLocation.LastIndexOf('\\')+1)+@"LocalLow\Nomoon\Mindnight";
		var dir = DirAccess.Open("user://");
		dir.MakeDir("GameLogs");
		dir.Copy(fileLocation+@"\Player.log","user://GameLogs/Player.log");
		
	}


	public void startF(){
		

		List<string> playerLog = File.ReadAllLines(
			System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData)+@"/Code Why/GameLogs/Player.log"
		).ToList();
		//var file = File.Open(fileLocation+@"\Player.log",FileMode.Open, System.IO.FileAccess.Read, FileShare.Read);
		//List<string> playerLog = File.ReadAllLines(fileLocation+@"\Player.log").ToList();
		var StartOfGame = playerLog.FindLastIndex(i => i.Contains(lineType[0]));
		startIndex = StartOfGame;

		if(StartOfGame>0){
			var newLog = playerLog.GetRange(StartOfGame,playerLog.Count-StartOfGame);

			var initialLines = newLog.Where( line => line.Contains(lineType[0]) || line.Contains(lineType[1]) );
			foreach(var i in newLog){
				// corp -> trait as json

				if(i.Contains(lineType[0]))
				{
						// get numberOfPlayers
					JsonDocument doc = JsonDocument.Parse(i.Substring(47));
					JsonElement root = doc.RootElement;
					numberOfPlayer = root.GetProperty("PlayerNumber").GetInt32();
					doc.Dispose();
				}
				else if(i.Contains(lineType[1]))
				{
						// get players
					JsonDocument doc = JsonDocument.Parse(i.Substring(49));
					JsonElement root = doc.RootElement;
					PlayerData newPlayer = new PlayerData
					{
						slot = root.GetProperty("Slot").GetInt32(),
						name = "[center][color="+ colors[root.GetProperty("Color").GetInt32()] +"]"+ (root.GetProperty("Slot").GetInt32()+1) +"  "+ root.GetProperty("Username").GetString() + "[/color][/center]",
						// [center][color=#00A6F6]1 : name[/color][/center]
						IsLocal = root.GetProperty("IsLocal").GetBoolean()
					};
					if(newPlayer.IsLocal) myPosition = players.Count()+1;
						
					players.Add(newPlayer);
					doc.Dispose();
				}
					
			}
		}
	}



	public class PlayerData{
		public string name;
		public int slot;
		public bool IsLocal;
	}
}
