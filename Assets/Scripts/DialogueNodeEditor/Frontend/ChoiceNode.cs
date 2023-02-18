using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class ChoiceNode : Node
{
    public ChoiceNodeData nodeData;

    private Action<int> OnRemoveChoice;
    private Action<ConnectionKnob> OnClickOutKnob;
    public ChoiceNode(int id, Vector2 position, float width, float height, Stylesheet stylesheet,
        Action<ConnectionKnob> OnClickInKnob, Action<ConnectionKnob> OnClickOutKnob, Action<Node> OnClickRemoveNode, Action<int> OnRemoveChoice) :
        base(id, position, width, height, stylesheet,
            OnClickInKnob, OnClickOutKnob, OnClickRemoveNode)
    { }

    public override void Init(Stylesheet stylesheet, Action<ConnectionKnob> OnClickInKnob, Action<ConnectionKnob> OnClickOutKnob)
    {
        inKnobs.Add(new ConnectionKnob(this, ConnectionKnobType.In, stylesheet.leftKnob, OnClickInKnob, 15,
           new List<NodeType>() {NodeType.StartNode, NodeType.DialogueNode }, true, ConnectionKnobSubType.Flow ));
        inKnobs.Add(new ConnectionKnob(this, ConnectionKnobType.In, stylesheet.leftKnob, OnClickInKnob, 40,
            new List<NodeType>() { NodeType.SpeakerNode}, false, ConnectionKnobSubType.Speaker));

        this.OnClickOutKnob = OnClickOutKnob;

        nodeType = NodeType.ChoiceNode;
        title = "Choice";
        nodeData = new ChoiceNodeData(new List<string>());
    }

    public override void DrawNodeContent()
    {
        if (GUI.Button(new Rect(rect.x + rect.width/2 - 10, rect.y + 50 + nodeData.choices.Count*35, 30, 30), "+", stylesheet.button))
        {
            nodeData.choices.Add("");
            outKnobs.Add(new ConnectionKnob(this, ConnectionKnobType.Out, stylesheet.rightKnob, OnClickOutKnob, 50 + (nodeData.choices.Count-1)*35,
                new List<NodeType>() {NodeType.DialogueNode, NodeType.EndNode }, false, ConnectionKnobSubType.Flow));
            rect.height += 35;
        }
        List<int> toRemove = new List<int>();
        for (int i = 0; i < nodeData.choices.Count; i++)
        {
            int res = DrawChoice(i * 35, i);
            if (res != -1)
                toRemove.Add(i);
        }

        foreach (int index in toRemove)
        {
            nodeData.choices.RemoveAt(index);
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
        nodeData.choices[index] = EditorGUI.TextArea(new Rect(rect.x + leftMargin + 30, rect.y + 50 + height, 200, 30), nodeData.choices[index]);
        if (GUI.Button(new Rect(rect.x + leftMargin + 237.5f, rect.y + 50 + height, 30, 30), "-", stylesheet.button))
        {
            return index;
        }

        return -1;
    }
}
