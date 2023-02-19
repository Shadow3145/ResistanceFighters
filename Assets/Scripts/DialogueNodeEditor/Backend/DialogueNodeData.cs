using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNodeData : NodeData
{

    public string dialogue;

    public DialogueNodeData(int id, Rect rect, 
        List<ConnectionKnob> inKnobs, List<ConnectionKnob> outKnobs, string dialogue) : base (id, rect, inKnobs, outKnobs) 
    {
        this.dialogue = dialogue;
    }
}
