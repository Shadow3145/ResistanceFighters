using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeData
{
    public int id;
    public Rect rect;
    public List<ConnectionKnob> inKnobs;
    public List<ConnectionKnob> outKnobs;

    public NodeData(int id, Rect rect, List<ConnectionKnob> inKnobs, List<ConnectionKnob> outKnobs)
    {
        this.id = id;
        this.rect = rect;
        this.inKnobs = inKnobs;
        this.outKnobs = outKnobs;
    }
}
