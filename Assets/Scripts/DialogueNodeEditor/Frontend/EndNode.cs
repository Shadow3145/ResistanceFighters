using System;
using System.Collections.Generic;
using UnityEngine;

public class EndNode : Node
{
    public EndNode(int id, Vector2 position, float width, float height, Stylesheet stylesheet,
        Action<ConnectionKnob> OnClickInKnob, Action<ConnectionKnob> OnClickOutKnob, Action<Node> OnClickRemoveNode) :
        base(id, position, width, height, stylesheet,
            OnClickInKnob, OnClickOutKnob, OnClickRemoveNode)
    { }

    public override void Init(Stylesheet stylesheet, Action<ConnectionKnob> OnClickInKnob, Action<ConnectionKnob> OnClickOutKnob)
    {
        inKnobs.Add(new ConnectionKnob(this, ConnectionKnobType.In, stylesheet.leftKnob, OnClickInKnob, 15,
            new List<NodeType>() { NodeType.DialogueNode, NodeType.ChoiceNode }, true, ConnectionKnobSubType.Flow));
        nodeType = NodeType.EndNode;
        title = "End Node";
    }
}
