using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class SpeakerNode : Node
{
    public SpeakerNodeData nodeData;
    public SpeakerNode(int id, Vector2 position, float width, float height, Stylesheet stylesheet,
        Action<ConnectionKnob> OnClickInKnob, Action<ConnectionKnob> OnClickOutKnob, Action<Node> OnClickRemoveNode) :
        base(id, position, width, height, stylesheet,
            OnClickInKnob, OnClickOutKnob, OnClickRemoveNode)
    { }

    public override void Init(Stylesheet stylesheet, Action<ConnectionKnob> OnClickInKnob, Action<ConnectionKnob> OnClickOutKnob)
    {
        outKnobs.Add(new ConnectionKnob(this, ConnectionKnobType.Out, stylesheet.rightKnob, OnClickOutKnob, 15,
            new List<NodeType>() { NodeType.DialogueNode, NodeType.ChoiceNode}, true, ConnectionKnobSubType.Speaker));
        nodeType = NodeType.SpeakerNode;
        nodeData = new SpeakerNodeData("", null);
        title = "Speaker";
    }

    public override void DrawNodeContent()
    {
        Rect speakerName = new Rect(rect.x + leftMargin + 10, rect.y + 50, 50, 20);
        EditorGUI.LabelField(speakerName, "Name", stylesheet.label);
        nodeData.name = EditorGUI.TextField(new Rect(speakerName.x + 60, speakerName.y, 100, 20), nodeData.name);

        Rect speakerIcon = new Rect(rect.x + leftMargin + 10, rect.y + 80, 50, 20);
        EditorGUI.LabelField(speakerIcon, "Icon", stylesheet.label);
        nodeData.icon = EditorGUI.ObjectField(new Rect(speakerIcon.x + 60, speakerIcon.y, 60, 60), nodeData.icon, typeof(Sprite), false) as Sprite;
        
    }
}
