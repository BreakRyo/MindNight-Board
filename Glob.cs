using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using Godot;

public partial class Glob : Node
{
    // v data
    public int myPosition = 1;
    public string draging = "0";
    public int numberOfPlayer = 5;
    public int startIndex = 0;
    public bool siblingIsOn = false;
    public string[] lineType =
    {
        "Received GameFound packet:", // PlayerNumber : int
        "Received SpawnPlayer packet:", // Slot : int (where IsLocal==true), Color : int, Username : string
    };

    public List<PlayerData> players = new List<PlayerData>();

    public List<SystemBlock> systemMessages = new List<SystemBlock>();
    public int nOfHacks = 0;

    // c data
    string[] colors =
    {
        "#00A6F6",
        "#D31FD4",
        "#6FE015",
        "#9D9D9D",
        "#FF8113",
        "#FFEC16",
        "#00B48B",
        "#0041F6"
    };

    // should red player log here and get : {my position, names, colors, number of players} and update v dara.

    public string fileLocation = System.Environment.GetFolderPath(
        System.Environment.SpecialFolder.ApplicationData
    );
    string[] lines;

    [Signal]
    public delegate void newLineAddedEventHandler(string newline);
    Control n = null;
    public List<string> newlines = new List<string>();

    public override void _Ready()
    {
        // start reading files

        Thread thread = new Thread(ReadLines);
        thread.Start();
        //ReadLines();
    }

    public void connect()
    {
        n = GetNode<Control>("/root/Node2D/Chat");
        //GD.Print(n.Name);
        siblingIsOn = true;
    }

    private Process process;

    public async void ReadLines()
    {
        string logPrePath = System.Environment.GetFolderPath(
            System.Environment.SpecialFolder.ApplicationData
        );
        string logPath =
            logPrePath.Substring(0, logPrePath.Length - 7)
            + @"LocalLow\Nomoon\Mindnight\Player.log";

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "powershell",
            Arguments = $"-Command Get-Content '{logPath}' -Wait -Encoding UTF8",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (process = new Process { StartInfo = startInfo })
        {
            process.Start();
            StreamReader reader = process.StandardOutput;
            while (!process.StandardOutput.EndOfStream)
            {
                string line = await reader.ReadLineAsync();
                if (numberOfPlayer != players.Count())
                {
                    //GD.Print("On StartF");
                    startFetch(line);
                }
                else
                {
                    //GD.Print("On organizer");

                    if (siblingIsOn)
                    {
                        n.Call("organizer", line);
                    }
                    newlines.Add(line);
                }
            }
        }
    }

    public void startFetch(string line)
    {
        if (line.Contains(lineType[0]) || line.Contains(lineType[1]))
        {
            if (line.Contains(lineType[0]))
            {
                // get numberOfPlayers
                JsonDocument doc = JsonDocument.Parse(line.Substring(47));
                JsonElement root = doc.RootElement;
                numberOfPlayer = root.GetProperty("PlayerNumber").GetInt32();

                doc.Dispose();
            }
            else if (line.Contains(lineType[1]))
            {
                // get players
                JsonDocument doc = JsonDocument.Parse(line.Substring(49));
                JsonElement root = doc.RootElement;
                PlayerData newPlayer = new PlayerData
                {
                    slot = root.GetProperty("Slot").GetInt32(),
                    name = root.GetProperty("Username").GetString(),
                    color = colors[root.GetProperty("Color").GetInt32()],
                    // [center][color=#00A6F6]1 : name[/color][/center]
                    IsLocal = root.GetProperty("IsLocal").GetBoolean()
                };
                if (newPlayer.IsLocal)
                    myPosition = players.Count() + 1;

                players.Add(newPlayer);
                doc.Dispose();
            }
        }
    }

    public class PlayerData
    {
        public string name;
        public int slot;
        public string color;
        public bool IsLocal;
        public int chanceOfBeingHacker = 0;
    }

    public void changeChance(int playerIndex, bool addOrNo, int amount)
    {
        if (addOrNo)
        {
            players[playerIndex].chanceOfBeingHacker += amount;
        }
        else
        {
            players[playerIndex].chanceOfBeingHacker -= amount;
        }

        var chances = GetTree().GetNodesInGroup("Chance");
        foreach (var i in chances)
        {
            if (i.GetMeta("Index").As<int>() == playerIndex)
            {
                i.GetNode<ProgressBar>("ProgressBar").Value = players[
                    playerIndex
                ].chanceOfBeingHacker;
            }
        }
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            process.Kill();
            GetTree().Quit(); // default behavior
        }
    }
}
