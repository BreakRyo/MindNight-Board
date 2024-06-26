using System;
using System.Linq;
using Godot;

public partial class SystemInfo : Control
{
    private PackedScene passMessage = GD.Load<PackedScene>(
        "res://tooMuchFilesHere/ProposerInfo/passPanel.tscn"
    );
    private PackedScene propMessage = GD.Load<PackedScene>(
        "res://tooMuchFilesHere/ProposerInfo/Proposition.tscn"
    );
    private Glob globy;
    private int currentMessage = 0;
    private int round = 0;
    private int node = 1;
    private int nOfPlayer = 5;
    private SystemBlock chillingBlock;
    private Control vision;

    private VBoxContainer vBox;

    public override void _Ready()
    {
        globy = GetNode<Glob>("/root/Glob");
        nOfPlayer = globy.numberOfPlayer;
        vBox = GetNode<VBoxContainer>("Panel/ScrollContainer/VBoxContainer");
        vision = GetNode<Control>("../Vision");
    }

    private void newSystemMessage()
    {
        SystemBlock currentBlock = globy.systemMessages[currentMessage];

        switch (currentBlock.messageType)
        {
            case 's':
                selectingPhase(currentBlock);
                break;
            case 'v':
                votingPhase(currentBlock);
                break;
            case 'm':
                messionPhase(currentBlock);
                break;
            default:

                break;
        }

        currentMessage++;
    }

    private void selectingPhase(SystemBlock b)
    {
        var player = globy.players[b.proposer];
        if (b.passed)
        {
            // show pass scene with editin the rich..
            Control passMsm = (Control)passMessage.Instantiate();
            passMsm.GetNode<RichTextLabel>("PassPanel/RichTextLabel").Text =
                "[center][color="
                + player.color
                + "]"
                + player.name
                + "[/color]"
                + " Passed The Proposal.[/center]";
            passMsm.GetNode<Label>("PassPanel/RoundNumber").Text = round + "/" + nOfPlayer;

            vBox.AddChild(passMsm);
        }
        else
        {
            // insert in the waiting block { proposer, propose}
            chillingBlock = b;
        }
    }

    private void votingPhase(SystemBlock b)
    {
        if (!b.waitForOptional)
        {
            // show propose scene with editing the rich..
            Control propMsm = (Control)propMessage.Instantiate();
            propMsm.GetNode<RichTextLabel>("PropositionPanel/Proposal").Text =
                "[center][color="
                + globy.players[chillingBlock.proposer].color
                + "]"
                + globy.players[chillingBlock.proposer].name
                + "[/color][/center]";
            string TheProposed = string.Join(
                ", ",
                chillingBlock
                    .propose.ToList()
                    .ConvertAll(i =>
                        "[color="
                        + globy.players[i].color
                        + "]"
                        + globy.players[i].name
                        + "[/color]"
                    )
            );
            propMsm.GetNode<RichTextLabel>("PropositionPanel/Propose").Text =
                "[center]" + TheProposed + "[/center]";
            string accepted = string.Join(
                ", ",
                b.accepted.ToList()
                    .ConvertAll(i =>
                        "[color="
                        + globy.players[i].color
                        + "]"
                        + globy.players[i].name
                        + "[/color]"
                    )
            );
            propMsm.GetNode<RichTextLabel>("PropositionPanel/Acc").Text =
                "[center]" + accepted + "[/center]";
            ;
            string refused = string.Join(
                ", ",
                b.refused.ToList()
                    .ConvertAll(i =>
                        "[color="
                        + globy.players[i].color
                        + "]"
                        + globy.players[i].name
                        + "[/color]"
                    )
            );
            propMsm.GetNode<RichTextLabel>("PropositionPanel/Ref").Text =
                "[center]" + refused + "[/center]";
            propMsm.GetNode<Label>("PropositionPanel/RoundNumber").Text = round + "/" + nOfPlayer;
            round++;
            vBox.AddChild(propMsm);
        }
        else
        {
            // insert in the waiting block { accepted, refused }
            chillingBlock.accepted = b.accepted;
            chillingBlock.refused = b.refused;
        }
    }

    private void messionPhase(SystemBlock b)
    {
        var player = globy.players[b.proposer];
        // show propose scene with editing the rich with adding the extra space in the control
        Control propMsm = (Control)propMessage.Instantiate();
        propMsm.GetNode<RichTextLabel>("PropositionPanel/Proposal").Text =
            "[center][color=" + player.color + "]" + player.name + "[/color][/center]";

        string TheProposed = string.Join(
            ", ",
            chillingBlock
                .propose.ToList()
                .ConvertAll(i =>
                    "[color=" + globy.players[i].color + "]" + globy.players[i].name + "[/color]"
                )
        );
        propMsm.GetNode<RichTextLabel>("PropositionPanel/Propose").Text =
            "[center]" + TheProposed + "[/center]";
        string accepted = string.Join(
            ", ",
            chillingBlock
                .accepted.ToList()
                .ConvertAll(i =>
                    "[color=" + globy.players[i].color + "]" + globy.players[i].name + "[/color]"
                )
        );
        propMsm.GetNode<RichTextLabel>("PropositionPanel/Acc").Text =
            "[center]" + accepted + "[/center]";
        string refused = string.Join(
            ", ",
            chillingBlock
                .refused.ToList()
                .ConvertAll(i =>
                    "[color=" + globy.players[i].color + "]" + globy.players[i].name + "[/color]"
                )
        );
        propMsm.GetNode<RichTextLabel>("PropositionPanel/Ref").Text =
            "[center]" + refused + "[/center]";

        propMsm.CustomMinimumSize = new Vector2(454, 114);
        propMsm.GetNode<Panel>("PropositionPanel").CustomMinimumSize = new Vector2(454, 114);
        string hacks = "";

        if (b.hacks != 0)
        {
            hacks = "[center][color=#db363a]Hacked[/color][/center]";
            propMsm.GetNode<RichTextLabel>("PropositionPanel/optional").Theme = GD.Load<Theme>(
                "res://tooMuchFilesHere/ProposerInfo/PropositionHacked.tres"
            );

            vision.Call("updateList", chillingBlock.propose, b.hacks);
            int amount = (b.hacks == 1) ? 3 : 5;
            foreach (var f in chillingBlock.propose)
            {
                globy.changeChance(f, true, amount);
            }
        }
        else
        {
            hacks = "[center][color=#5dcd50]Secured[/color][/center]";
            propMsm.GetNode<RichTextLabel>("PropositionPanel/optional").Theme = GD.Load<Theme>(
                "res://tooMuchFilesHere/ProposerInfo/PropositionSecured.tres"
            );
            foreach (var f in chillingBlock.propose)
            {
                globy.changeChance(f, false, 2);
            }
        }

        propMsm.GetNode<RichTextLabel>("PropositionPanel/optional").Text = hacks;
        propMsm.GetNode<Label>("PropositionPanel/RoundNumber").Text = round + "/" + nOfPlayer;

        propMsm.GetNode<Label>("PropositionPanel/node").Text = "N" + node;
        round = 0;
        node++;
        vBox.AddChild(propMsm);
    }
}
