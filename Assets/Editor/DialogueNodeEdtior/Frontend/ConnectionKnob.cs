using System;
using UnityEngine;

public enum ConnectionKnobType { In, Out};

public class ConnectionKnob
{
    public Rect rect;
    public ConnectionKnobType type;
    public Node ownerNode;
    public GUIStyle guiStyle;

    private float width = 10f;
    private float height = 20f;

    public Action<ConnectionKnob> OnClickConnectionKnob;

    public ConnectionKnob(Node ownerNode, ConnectionKnobType type, GUIStyle guiStyle, Action<ConnectionKnob> OnClickConnectionKnob)
    {
        this.ownerNode = ownerNode;
        this.type = type;
        this.guiStyle = guiStyle;
        this.OnClickConnectionKnob = OnClickConnectionKnob;

        this.rect = new Rect(0f, 0f, width, height);
    }

    public void DrawKnob()
    {
        rect.y = ownerNode.rect.y + (ownerNode.rect.height * 0.5f) - rect.height * 0.5f;

        switch (type)
        {
            case ConnectionKnobType.In:
                rect.x = ownerNode.rect.x - rect.width + 8f;
                break;

            case ConnectionKnobType.Out:
                rect.x = ownerNode.rect.x + ownerNode.rect.width - 8f;
                break;
        }

        if (GUI.Button(rect, "", guiStyle))
        {
            if (OnClickConnectionKnob != null)
            {
                OnClickConnectionKnob(this);
            }
        }
    }
}
