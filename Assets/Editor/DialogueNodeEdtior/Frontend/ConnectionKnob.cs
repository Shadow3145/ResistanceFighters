using System;
using System.Collections.Generic;
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
    private float yOffset;

    public Action<ConnectionKnob> OnClickConnectionKnob;

    public List<NodeType> allowedConnections;

    public bool allowMoreConnections;

    public List<Connection> connections;

    public ConnectionKnob(Node ownerNode, ConnectionKnobType type, GUIStyle guiStyle, Action<ConnectionKnob> OnClickConnectionKnob, float yPos, List<NodeType> allowedConnections, bool allowMoreConnections)
    {
        this.ownerNode = ownerNode;
        this.type = type;
        this.guiStyle = guiStyle;
        this.OnClickConnectionKnob = OnClickConnectionKnob;

        this.allowedConnections = allowedConnections;
        this.allowMoreConnections = allowMoreConnections;

        this.rect = new Rect(0f, 0f, width, height);

        connections = new List<Connection>();
        yOffset = yPos;
    }

    public void DrawKnob()
    {
        rect.y = ownerNode.rect.y + yOffset;
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
