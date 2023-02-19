using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EndNode : Node
{
    public EndNode(int id, Vector2 position, float width, float height, List<ConnectionKnob> inKnobs, List<ConnectionKnob> outKnobs) :
        base(id, position, width, height, inKnobs, outKnobs)
    {
        nodeType = NodeType.EndNode;
        title = "End Node";
    }

    public override void Init(Action<ConnectionKnob> OnClickInKnob, Action<ConnectionKnob> OnClickOutKnob, Action<Node> OnClickRemoveNode)
    {
        base.Init(OnClickInKnob, OnClickOutKnob, OnClickRemoveNode);
        outKnobs = new List<ConnectionKnob>();
        if (inKnobs == null)
        {
            inKnobs = new List<ConnectionKnob>();
            inKnobs.Add(new ConnectionKnob(this, ConnectionKnobType.In, stylesheet.leftKnob, OnClickInKnob, 15,
                new List<NodeType>() { NodeType.DialogueNode, NodeType.ChoiceNode }, true, ConnectionKnobSubType.Flow));
        }

    }
}
