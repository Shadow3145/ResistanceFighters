using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DialogueNodeData
{
    public string dialogue;

    public DialogueNodeData(string dialogue)
    {
        this.dialogue = dialogue;
    }
}
