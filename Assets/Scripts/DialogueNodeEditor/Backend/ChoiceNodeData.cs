using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ChoiceNodeData
{
    public List<string> choices;

    public ChoiceNodeData(List<string> choices)
    {
        this.choices = choices;
    }
}
