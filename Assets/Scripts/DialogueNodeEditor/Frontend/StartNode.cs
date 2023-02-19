using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StartNode : Node
{
    public StartNode(int id, Vector2 position, float width, float height, List<ConnectionKnob> inKnobs, List<ConnectionKnob> outKnobs) : 
        base(id, position, width, height, inKnobs, outKnobs)
    {
        nodeType = NodeType.StartNode;
        title = "Start Node";
    }

    public override void Init(Action<ConnectionKnob> OnClickInKnob, Action<ConnectionKnob> OnClickOutKnob, Action<Node> OnClickRemoveNode)
    {
        base.Init(OnClickInKnob, OnClickOutKnob, OnClickRemoveNode);
        inKnobs = new List<ConnectionKnob>();
        if (outKnobs == null)
        {
            outKnobs = new List<ConnectionKnob>();
            outKnobs.Add(new ConnectionKnob(this, ConnectionKnobType.Out, stylesheet.rightKnob, OnClickOutKnob, 15,
                new List<NodeType>() { NodeType.DialogueNode, NodeType.ChoiceNode }, false, ConnectionKnobSubType.Flow));
        }

    }
}
