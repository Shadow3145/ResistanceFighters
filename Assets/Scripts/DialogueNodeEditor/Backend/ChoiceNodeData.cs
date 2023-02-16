using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ChoiceNodeData
{
    public List<string> choices;

    public ChoiceNodeData(List<string> choices)
    {
        this.choices = choices;
    }
}
