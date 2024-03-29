using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class vision : Control
{
    private Glob TheGlob;
    private List<Glob.PlayerData> players;
    private Panel panel;
    private OptionButton selectButton;
    private VBoxContainer chancesContainer;
    private PackedScene chance = GD.Load<PackedScene>("res://tooMuchFilesHere/vision/Chances.tscn");

    private int myPosition; // not index
    private List<IEnumerable<int>> possibilitiesFive = new List<IEnumerable<int>>();

    public override void _Ready()
    {
        TheGlob = GetNode<Glob>("/root/Glob");
        selectButton = GetNode<OptionButton>("Panel/OptionButton");
        panel = GetNode<Panel>("Panel");
        chancesContainer = GetNode<VBoxContainer>("Panel/Panel/HBoxContainer");
        players = TheGlob.players;
        myPosition = TheGlob.myPosition;
        foreach (var i in players)
        {
            if (i.slot != myPosition - 1)
            {
                selectButton.AddItem(i.name);
                Control ChanceInst = (Control)chance.Instantiate();
                ChanceInst.GetNode<RichTextLabel>("name").Text =
                    "[center][color=" + i.color + "]" + i.name + "[/color][/center]";
                ChanceInst.SetMeta("Index", i.slot);
                chancesContainer.AddChild(ChanceInst);
            }
            else
            {
                selectButton.AddItem(i.name + "   ( Me )");
            }
        }

        foreach (var i in Enumerable.Range(1, players.Count))
        {
            possibilitiesFive.Add(
                Enumerable.Range(1, players.Count).Where(j => j != i && j != myPosition)
            );
        }
        GetNode<Panel>("Panel/Panel").Size += new Vector2(0, 35 * players.Count());

        setNewProfile(0);
    }

    private void setNewProfile(int player)
    {
        selectButton.Select(player);
        panel.GetNode<RichTextLabel>("Label").Text =
            "[center][color="
            + players[player].color
            + "]"
            + (players[player].slot + 1).ToString()
            + "[/color][/center]";
        List<int> lis = possibilitiesFive[player].ToList();

        string posH =
            "[center]"
            + string.Join(
                ", ",
                lis.ConvertAll(i =>
                    "[color=" + players[i - 1].color + "]" + players[i - 1].name + "[/color]"
                )
            )
            + "[/center]";
        panel.GetNode<RichTextLabel>("PossibleHackers").Text = posH;
        panel.GetNode<RichTextLabel>("PossibleHackers").TooltipText =
            "Players " + players[player].name + " Can Be Hackers With.";
        // Agents :
        IEnumerable<int> agents = Enumerable
            .Range(1, players.Count)
            .Where(i => !lis.Contains(i) && i != player + 1);
        string posA =
            "[center]"
            + string.Join(
                ", ",
                agents
                    .ToList()
                    .ConvertAll(i =>
                        "[color=" + players[i - 1].color + "]" + players[i - 1].name + "[/color]"
                    )
            )
            + "[/center]";
        panel.GetNode<RichTextLabel>("PossibleAgents").Text = posA;
        panel.GetNode<RichTextLabel>("PossibleAgents").TooltipText =
            "Players " + players[player].name + " Can't Be Hackers With.";
    }

    private void updateList(int[] prop, int nOfHacks)
    {
        if (nOfHacks == 1)
        {
            for (int i = 0; i < possibilitiesFive.Count; i++)
            {
                if (!prop.Contains(i))
                {
                    possibilitiesFive[i] = possibilitiesFive[i].Where(j => prop.Contains(j - 1));
                }
            }
        }
        else
        {
            for (int i = 0; i < possibilitiesFive.Count; i++)
            {
                if (prop.Contains(i))
                {
                    possibilitiesFive[i] = possibilitiesFive[i].Where(j => prop.Contains(j - 1));
                }
                else
                {
                    possibilitiesFive[i] = Enumerable.Empty<int>();
                }
            }
        }

        setNewProfile(0);
    }
}
