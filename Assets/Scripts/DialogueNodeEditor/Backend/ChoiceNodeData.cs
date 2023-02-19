using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChoiceNodeData : NodeData
{
    public List<string> choices;

    public ChoiceNodeData(int id, Rect rect,
        List<ConnectionKnob> inKnobs, List<ConnectionKnob> outKnobs, List<string> choices) : base(id, rect, inKnobs, outKnobs)
    {
        this.choices = choices;
    }
}
