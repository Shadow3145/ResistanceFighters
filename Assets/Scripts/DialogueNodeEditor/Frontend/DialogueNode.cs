using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[System.Serializable]
public class DialogueNode : Node
{
    public string dialogue;

    public DialogueNode(int id, Vector2 position, float width, float height, List<ConnectionKnob> inKnobs, List<ConnectionKnob> outKnobs, string dialogue) :
        base(id, position, width, height, inKnobs, outKnobs)
    {
        this.dialogue = dialogue;

        nodeType = NodeType.DialogueNode;
        title = "Dialogue";
    }

    public override void Init(Action<ConnectionKnob> OnClickInKnob, Action<ConnectionKnob> OnClickOutKnob, Action<Node> OnClickRemoveNode)
    {
        base.Init(OnClickInKnob, OnClickOutKnob, OnClickRemoveNode);
        if (inKnobs == null)
        {
            inKnobs = new List<ConnectionKnob>();
            inKnobs.Add(new ConnectionKnob(this, ConnectionKnobType.In, stylesheet.leftKnob, OnClickInKnob, 15,
            new List<NodeType>() { NodeType.DialogueNode, NodeType.StartNode, NodeType.ChoiceNode }, true, ConnectionKnobSubType.Flow));
            inKnobs.Add(new ConnectionKnob(this, ConnectionKnobType.In, stylesheet.leftKnob, OnClickInKnob, 40,
                new List<NodeType>() { NodeType.SpeakerNode }, false, ConnectionKnobSubType.Speaker));
        }
        if (outKnobs == null)
        {
            outKnobs = new List<ConnectionKnob>();
            outKnobs.Add(new ConnectionKnob(this, ConnectionKnobType.Out, stylesheet.rightKnob, OnClickOutKnob, 15,
                new List<NodeType>() { NodeType.ChoiceNode, NodeType.DialogueNode, NodeType.EndNode }, false, ConnectionKnobSubType.Flow));
        }
    }
#if UNITY_EDITOR

    public override void DrawNodeContent()
    {
        Rect dialogueRect = new Rect(rect.x + leftMargin + 10, rect.y + 50, rect.width - leftMargin*2 - 20, rect.height - topMargin - 70);
        dialogue = EditorGUI.TextArea(dialogueRect, dialogue);
    }
#endif
}
