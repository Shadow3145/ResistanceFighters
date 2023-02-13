using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum NodeType
{
    BaseNode,
    StartNode,
    SpeakerNode,
    DialogueNode,
    ChoiceNode,
    EndNode
}

[System.Serializable]
public class Node
{
    public int id;

    public Rect rect;
    public string title;
    
    public bool isDragged = false;
    public bool isSelected = false;

    public List<ConnectionKnob> inKnobs;
    public List<ConnectionKnob> outKnobs;

    public GUIStyle nodeStyle;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;

    protected Stylesheet stylesheet;

    public Action<Node> OnRemoveNode;

    public NodeType nodeType;

    protected float leftMargin;
    protected float topMargin;


    public Node(int id, Vector2 position, float width, float height, Stylesheet stylesheet,
        Action<ConnectionKnob> OnClickInKnob, Action<ConnectionKnob> OnClickOutKnob, Action<Node> OnClickRemoveNode)
    {
        this.id = id;
        this.stylesheet = stylesheet;
        rect = new Rect(position.x, position.y, width, height);
        nodeStyle = stylesheet.node.defaultStyle;
        defaultNodeStyle = stylesheet.node.defaultStyle;
        selectedNodeStyle = stylesheet.node.selectedStyle;

        OnRemoveNode = OnClickRemoveNode;

        leftMargin = nodeStyle.border.left / 1.5f;
        topMargin = nodeStyle.border.top / 2;

        inKnobs = new List<ConnectionKnob>();
        outKnobs = new List<ConnectionKnob>();

        Init(stylesheet, OnClickInKnob, OnClickOutKnob);
    }

    public virtual void Init(Stylesheet stylesheet, Action<ConnectionKnob> OnClickInKnob, Action<ConnectionKnob> OnClickOutKnob)
    {        
    }

    public virtual void DrawNode()
    {
        Rect headerRect = new Rect(new Vector2(rect.x + leftMargin, rect.y + topMargin), new Vector2(rect.width - (nodeStyle.border.left*1.3f), 25f));
        GUI.Box(rect, "", nodeStyle);
        GUI.Box(headerRect, title, stylesheet.nodeHeader);
        DrawNodeContent();
        foreach (ConnectionKnob knob in inKnobs)
            knob.DrawKnob();
        foreach (ConnectionKnob knob in outKnobs)
            knob.DrawKnob();
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
