

using System;
using System.Collections.Generic;
using UnityEngine;

public enum ConnectionKnobType { In, Out};

public enum ConnectionKnobSubType {Flow, Speaker, Data};

[System.Serializable]
public class ConnectionKnob
{
    public Rect rect;
    public ConnectionKnobType type;

    public ConnectionKnobSubType subType;

    private GUIStyle guiStyle;

    private float width = 10f;
    private float height = 20f;
    [SerializeField] private float yOffset;

    public Action<ConnectionKnob> OnClickConnectionKnob;

    public int ownerNodeId;

    public List<NodeType> allowedConnections;

    public bool allowMoreConnections;

    public List<int> connections;

    public ConnectionKnob(Node ownerNode, ConnectionKnobType type, GUIStyle guiStyle, Action<ConnectionKnob> OnClickConnectionKnob, float yPos, 
    List<NodeType> allowedConnections, bool allowMoreConnections, ConnectionKnobSubType subType)
    {
        this.ownerNodeId = ownerNode.id;
        this.type = type;
        this.subType = subType;
        this.guiStyle = guiStyle;
        this.OnClickConnectionKnob = OnClickConnectionKnob;


        this.allowedConnections = allowedConnections;
        this.allowMoreConnections = allowMoreConnections;

        this.rect = new Rect(0f, 0f, width, height);

        connections = new List<int>();
        yOffset = yPos;
    }
#if UNITY_EDITOR
    public void DrawKnob(Node owner)
    {
        if (guiStyle == null)
        {
            Resources.LoadAll("");
            Stylesheet stylesheet = Resources.FindObjectsOfTypeAll<Stylesheet>()[0];
            guiStyle = type == ConnectionKnobType.In
                ? stylesheet.leftKnob
                : stylesheet.rightKnob;
        }
        rect.y = owner.rect.y + yOffset;
        switch (type)
        {
            case ConnectionKnobType.In:
                rect.x = owner.rect.x - rect.width + 8f;
                break;

            case ConnectionKnobType.Out:
                rect.x = owner.rect.x + owner.rect.width - 8f;
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
#endif
}
