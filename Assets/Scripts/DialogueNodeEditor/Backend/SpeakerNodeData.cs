using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpeakerNodeData : NodeData
{ 
    public string speakerName;
    public Sprite icon;

    public SpeakerNodeData(int id, Rect rect, List<ConnectionKnob> inKnobs, List<ConnectionKnob> outKnobs, string speakerName, Sprite icon) : 
        base(id, rect, inKnobs, outKnobs)
    {
        this.speakerName = speakerName;
        this.icon = icon;
    }
}
