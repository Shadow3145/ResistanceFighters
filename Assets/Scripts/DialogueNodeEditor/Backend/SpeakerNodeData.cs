using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SpeakerNodeData
{ 
    public string name;
    public Sprite icon;

    public SpeakerNodeData(string name, Sprite icon)
    {
        this.name = name;
        this.icon = icon;
    }
}
