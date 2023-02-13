using System;
using System.Collections.Generic;
using UnityEngine;

public class StartNode : Node
{
    public StartNode(int id, Vector2 position, float width, float height, Stylesheet stylesheet,
        Action<ConnectionKnob> OnClickInKnob, Action<ConnectionKnob> OnClickOutKnob, Action<Node> OnClickRemoveNode) : 
        base(id, position, width, height, stylesheet, 
            OnClickInKnob, OnClickOutKnob, OnClickRemoveNode)
    {}

    public override void Init(Stylesheet stylesheet, Action<ConnectionKnob> OnClickInKnob, Action<ConnectionKnob> OnClickOutKnob)
    {
        outKnobs.Add(new ConnectionKnob(this, ConnectionKnobType.Out, stylesheet.rightKnob, OnClickOutKnob, 15,
            new List<NodeType>() { NodeType.DialogueNode, NodeType.ChoiceNode}, false));
        nodeType = NodeType.StartNode;
        title = "Start Node";
    }
}
