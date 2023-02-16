using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class DialogueNode : Node
{
    public DialogueNodeData nodeData;

    public DialogueNode(int id, Vector2 position, float width, float height, Stylesheet stylesheet,
        Action<ConnectionKnob> OnClickInKnob, Action<ConnectionKnob> OnClickOutKnob, Action<Node> OnClickRemoveNode) :
        base(id, position, width, height, stylesheet,
            OnClickInKnob, OnClickOutKnob, OnClickRemoveNode)
    { }

    public override void Init(Stylesheet stylesheet, Action<ConnectionKnob> OnClickInKnob, Action<ConnectionKnob> OnClickOutKnob)
    {
        inKnobs.Add(new ConnectionKnob(this, ConnectionKnobType.In, stylesheet.leftKnob, OnClickInKnob, 15, 
            new List<NodeType>() { NodeType.DialogueNode, NodeType.StartNode, NodeType.ChoiceNode}, true, ConnectionKnobSubType.Flow));
        inKnobs.Add(new ConnectionKnob(this, ConnectionKnobType.In, stylesheet.leftKnob, OnClickInKnob, 40,
            new List<NodeType>() { NodeType.SpeakerNode}, false, ConnectionKnobSubType.Speaker));
        outKnobs.Add(new ConnectionKnob(this, ConnectionKnobType.Out, stylesheet.rightKnob, OnClickOutKnob, 15,
            new List<NodeType>() { NodeType.ChoiceNode, NodeType.DialogueNode, NodeType.EndNode}, false, ConnectionKnobSubType.Flow));
        nodeType = NodeType.DialogueNode;

        nodeData = new DialogueNodeData("");
        title = "Dialogue";
    }

    public override void DrawNodeContent()
    {
        Rect dialogueRect = new Rect(rect.x + leftMargin + 10, rect.y + 50, rect.width - leftMargin*2 - 20, rect.height - topMargin - 70);
        nodeData.dialogue = EditorGUI.TextArea(dialogueRect, nodeData.dialogue);
    }
}
