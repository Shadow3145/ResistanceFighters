using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeStyle
{
    public GUIStyle defaultStyle;
    public GUIStyle selectedStyle;
}

[System.Serializable]
[CreateAssetMenu(menuName = "SO/Editor/Stylesheet")]
public class Stylesheet : ScriptableObject
{
    public NodeStyle node;

    public GUIStyle topPanel;

    public GUIStyle leftKnob;
    public GUIStyle rightKnob;

    public GUIStyle button;

    public GUIStyle nodeHeader;

    public GUIStyle label;
    public GUIStyle textField;

}
