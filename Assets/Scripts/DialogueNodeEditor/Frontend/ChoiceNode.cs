using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class ChoiceNode : Node
{
    public List<string> choices;

    public Action<int> OnRemoveChoice;
    public Action<ConnectionKnob> OnClickOutKnob;
    
    public ChoiceNode(int id, Vector2 position, float width, float height, List<ConnectionKnob> inKnobs, List<ConnectionKnob> outKnobs, List<string> choices) :
        base(id, position, width, height, inKnobs, outKnobs)
    {
        this.choices = choices;

        nodeType = NodeType.ChoiceNode;
        title = "Choice";
    }

    public override void Init(Action<ConnectionKnob> OnClickInKnob, Action<ConnectionKnob> OnClickOutKnob, Action<Node> OnClickRemoveNode)
    {
        base.Init(OnClickInKnob, OnClickOutKnob, OnClickRemoveNode);
        if (inKnobs == null)
        {
            inKnobs = new List<ConnectionKnob>();
            inKnobs.Add(new ConnectionKnob(this, ConnectionKnobType.In, stylesheet.leftKnob, OnClickInKnob, 15,
                new List<NodeType>() { NodeType.StartNode, NodeType.DialogueNode }, true, ConnectionKnobSubType.Flow));
            inKnobs.Add(new ConnectionKnob(this, ConnectionKnobType.In, stylesheet.leftKnob, OnClickInKnob, 40,
                new List<NodeType>() { NodeType.SpeakerNode }, false, ConnectionKnobSubType.Speaker));
        }
        if (outKnobs == null)
            outKnobs = new List<ConnectionKnob>();

        this.OnClickOutKnob = OnClickOutKnob;
    }
#if UNITY_EDITOR

    public override void DrawNodeContent()
    {
        if (GUI.Button(new Rect(rect.x + rect.width/2 - 10, rect.y + 50 + choices.Count*35, 30, 30), "+", stylesheet.button))
        {
            choices.Add("");
            outKnobs.Add(new ConnectionKnob(this, ConnectionKnobType.Out, stylesheet.rightKnob, OnClickOutKnob, 50 + (choices.Count-1)*35,
                new List<NodeType>() {NodeType.DialogueNode, NodeType.EndNode }, false, ConnectionKnobSubType.Flow));
            rect.height += 35;
        }
        List<int> toRemove = new List<int>();
        for (int i = 0; i < choices.Count; i++)
        {
            int res = DrawChoice(i * 35, i);
            if (res != -1)
                toRemove.Add(i);
        }

        foreach (int index in toRemove)
        {
            choices.RemoveAt(index);
            if (outKnobs[index].connections != null)
                foreach (int connection in outKnobs[index].connections)
                    OnRemoveChoice(connection);
            outKnobs.RemoveAt(index);
            rect.height -= 35;
        }
    }

    private int DrawChoice(float height, int index)
    {
        EditorGUI.LabelField(new Rect(rect.x + leftMargin + 10, rect.y + 50 + height, 15, 30), (index+1).ToString(), stylesheet.label);
        choices[index] = EditorGUI.TextArea(new Rect(rect.x + leftMargin + 30, rect.y + 50 + height, 200, 30), choices[index]);
        if (GUI.Button(new Rect(rect.x + leftMargin + 237.5f, rect.y + 50 + height, 30, 30), "-", stylesheet.button))
        {
            return index;
        }

        return -1;
    }
#endif
}
