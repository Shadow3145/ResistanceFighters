using UnityEngine;

[System.Serializable]
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
