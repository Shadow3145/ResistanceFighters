using System;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Node
{
    public Rect rect;
    public string title;
    public bool isDragged = false;
    public bool isSelected = false;

    public ConnectionKnob inKnob;
    public ConnectionKnob outKnob;

    public GUIStyle nodeStyle;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;

    public Action<Node> OnRemoveNode;

    public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedNodeStyle, GUIStyle knobStyle, Action<ConnectionKnob> OnClickInKnob, Action<ConnectionKnob> OnClickOutKnob, Action<Node> OnClickRemoveNode)
    {
        rect = new Rect(position.x, position.y, width, height);
        this.nodeStyle = nodeStyle;
        this.defaultNodeStyle = nodeStyle;
        this.selectedNodeStyle = selectedNodeStyle;

        inKnob = new ConnectionKnob(this, ConnectionKnobType.In, knobStyle, OnClickInKnob);
        outKnob = new ConnectionKnob(this, ConnectionKnobType.Out, knobStyle, OnClickOutKnob);

        this.OnRemoveNode = OnClickRemoveNode;
    }

    public void DrawNode()
    {
        GUI.Box(rect, title, nodeStyle);
        inKnob.DrawKnob();
        outKnob.DrawKnob();
    }

    public virtual void DrawNodeContent()
    {
    }

    public void DragNode(Vector2 delta)
    {
        rect.position += delta;
    }

    public bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (rect.Contains(e.mousePosition))
                    {
                        isDragged = true;
                        GUI.changed = true;
                        isSelected = true;
                        nodeStyle = selectedNodeStyle;
                    }
                    else
                    {
                        isSelected = false;
                        nodeStyle = defaultNodeStyle;
                        GUI.changed = true;
                    }
                }
                if (e.button == 1 && isSelected)
                {
                    ShowContextMenu();
                    e.Use();
                }
                break;
            
            case EventType.MouseUp:
                isDragged = false;
                break;
            case EventType.MouseDrag:
                if (e.button == 0 && isDragged)
                {
                    DragNode(e.delta);
                    e.Use();
                    return true;
                }
                break;
            default:
                break;
        }
        return false;
    }

    private void ShowContextMenu()
    {
        GenericMenu contextMenu = new GenericMenu();
        contextMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        contextMenu.ShowAsContext();
    }

    private void OnClickRemoveNode()
    {
        if (this.OnRemoveNode != null)
            OnRemoveNode(this);
    }
}
