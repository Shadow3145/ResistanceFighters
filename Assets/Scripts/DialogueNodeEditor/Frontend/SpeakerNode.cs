using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class SpeakerNode : Node
{
    public string name;
    public Sprite icon;

    public SpeakerNode(int id, Vector2 position, float width, float height, List<ConnectionKnob> inKnobs, List<ConnectionKnob> outKnobs, string name, Sprite icon) :
        base(id, position, width, height, inKnobs, outKnobs)
    {
        this.name = name;
        this.icon = icon;
        nodeType = NodeType.SpeakerNode;
        title = "Speaker";
    }

    public override void Init(Action<ConnectionKnob> OnClickInKnob, Action<ConnectionKnob> OnClickOutKnob, Action<Node> OnClickRemoveNode)
    {
        base.Init(OnClickInKnob, OnClickOutKnob, OnClickRemoveNode);
        if (outKnobs == null)
        {
            outKnobs = new List<ConnectionKnob>();
            outKnobs.Add(new ConnectionKnob(this, ConnectionKnobType.Out, stylesheet.rightKnob, OnClickOutKnob, 15,
                new List<NodeType>() { NodeType.DialogueNode, NodeType.ChoiceNode }, true, ConnectionKnobSubType.Speaker));
        }
        inKnobs = new List<ConnectionKnob>();
    }

    public override void DrawNodeContent()
    {
        Rect speakerName = new Rect(rect.x + leftMargin + 10, rect.y + 50, 50, 20);
        EditorGUI.LabelField(speakerName, "Name", stylesheet.label);
        name = EditorGUI.TextField(new Rect(speakerName.x + 60, speakerName.y, 100, 20), name);

        Rect speakerIcon = new Rect(rect.x + leftMargin + 10, rect.y + 80, 50, 20);
        EditorGUI.LabelField(speakerIcon, "Icon", stylesheet.label);
        icon = EditorGUI.ObjectField(new Rect(speakerIcon.x + 60, speakerIcon.y, 60, 60), icon, typeof(Sprite), false) as Sprite;
        
    }
}
