using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

public class DialogueNodeEditorWindow : EditorWindow
{
    private Dictionary<int, Node> nodes;
    private List<Connection> connections;

    private int lastId = 1;

    [SerializeField] private Stylesheet stylesheet;


    private ConnectionKnob selectedInKnob;
    private ConnectionKnob selectedOutKnob;

    private Rect topPanel;

    private Vector2 drag;
    private Vector2 offset;

    private bool stopDrawing = false;
    private bool isDrawing = false;

    public string selectedDialogue ="";
    public Dialogue[] dialogues;

    [MenuItem("Window/Dialogue Node Editor")]
    private static void ShowWindow()
    {
        DialogueNodeEditorWindow window = GetWindow<DialogueNodeEditorWindow>();
        window.titleContent = new GUIContent("Dialogue Node Editor");
        window.maximized = true;
    }

    private void OnEnable()
    {
        topPanel = new Rect(0, 0, this.position.width, 50);
    }

    private void SetDialogue()
    {
        dialogues = Resources.FindObjectsOfTypeAll<Dialogue>();
        string[] names = dialogues.Select(x => x.name).ToArray();

        if (selectedDialogue != "" && names.Contains(selectedDialogue))
        {
            int index = ArrayUtility.IndexOf(names, selectedDialogue);
            nodes = dialogues[index].nodes;
            connections = dialogues[index].connections;
        }
    }

    private void OnGUI()
    {
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);


        DrawTopPanel();
        SetDialogue();

        DrawNodes();
        DrawConnections();
        ProcessEvents(Event.current);
        ProcessNodesEvents(Event.current);

        DrawConnectionLine(Event.current);


        if (GUI.changed)
            Repaint();
    }

    private void DrawGrid(float spacing, float opacity, Color color)
    {
        int widthDivs = Mathf.CeilToInt(position.width / spacing);
        int heightDivs = Mathf.CeilToInt(position.height / spacing);

        Handles.BeginGUI();
        Handles.color = new Color(color.r, color.g, color.b, opacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % spacing, offset.y % spacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(spacing * i, -spacing, 0) + newOffset, new Vector3(spacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-spacing, spacing * j, 0) + newOffset, new Vector3(position.width, spacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void DrawTopPanel()
    {
        topPanel.width = position.width;
        GUI.Box(topPanel, "", stylesheet.topPanel);

        if (GUI.Button(new Rect(250, 10, 30, 30), "+", stylesheet.button))
        {
            DialoguePopupWindow window = new DialoguePopupWindow(OnClosePopupWindow);
            PopupWindow.Show(GUILayoutUtility.GetLastRect(), window);
        }

        string[] dialogues = Resources.FindObjectsOfTypeAll<Dialogue>().Select(x => x.name).ToArray();
        System.Array.Sort(dialogues);
        int index = selectedDialogue != ""
            ? ArrayUtility.IndexOf(dialogues, selectedDialogue)
            : -1;
        int selectedIndex = EditorGUI.Popup(new Rect(20, 10, 200, 30), index, dialogues);
        if (selectedIndex != -1)
            selectedDialogue = dialogues[selectedIndex];
    }

    private void OnClosePopupWindow(DialoguePopupWindow popupWindow)
    {
        if (popupWindow.newDialogueName != "")
        {
            selectedDialogue = popupWindow.newDialogueName;
            GUI.changed = true;
        }
    }

    private void DrawNodes()
    {
        if (nodes != null)
        {
            foreach (Node node in nodes.Values)
                node.DrawNode();
        }
    }

    private void DrawConnections()
    {
        Connection toRemove = null;
        if (connections != null)
        {
            foreach (Connection connection in connections)
            {
                Connection c = connection.DrawConnection();
                if (c != null)
                    toRemove = c;
            }
        }

        if (toRemove != null)
            RemoveConnection(toRemove);
    }

    private void RemoveConnection(Connection connection)
    {
        connection.inKnob.connections.Remove(connection);
        connection.outKnob.connections.Remove(connection);
        connections.Remove(connection);
    }

    private void DrawConnectionLine(Event e)
    {
        if (stopDrawing)
        {
            isDrawing = false;
            stopDrawing = false;
            selectedInKnob = null;
            selectedOutKnob = null;
            return;
        }
        if (selectedInKnob == null && selectedOutKnob != null)
        {
            DrawBezier(selectedOutKnob.rect.center, e);
            GUI.changed = true;
            isDrawing = true;
        }
    }

    private void DrawBezier(Vector2 startingPoint, Event e)
    {
        Handles.DrawBezier(startingPoint, e.mousePosition, startingPoint - Vector2.left * 50f, e.mousePosition + Vector2.left * 50f, Color.white, null, 2f);
    }

    private void ProcessEvents(Event e)
    {
        drag = Vector2.zero;

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 1)
                    ShowContextMenu(e.mousePosition);
                if ((e.button == 0 || e.button == 1) && isDrawing)
                    stopDrawing = true;
                break;
            case EventType.MouseDrag:
                if (e.button == 2)
                    OnDrag(e.delta);
                break;
            default:
                break;
        }
    }

    private void ProcessNodesEvents(Event e)
    {
        if (nodes != null)
        {
            foreach (Node node in nodes.Values)
            {
                bool guiChanged = node.ProcessEvents(e);

                if (guiChanged)
                    GUI.changed = true;
            }
        }
    }

    private void ShowContextMenu(Vector2 position)
    {
        GenericMenu contextMenu = new GenericMenu();
        foreach (NodeType nodeType in Enum.GetValues(typeof(NodeType)))
        {
            if (nodeType == NodeType.BaseNode || (nodeType == NodeType.StartNode && nodes.ContainsKey(0)))
                continue;
            contextMenu.AddItem(new GUIContent(nodeType.ToString()), false, () => OnClickAddNode(position, nodeType));
        }
        contextMenu.ShowAsContext();
    }

    private void OnClickAddNode(Vector2 position, NodeType nodeType = NodeType.BaseNode)
    {
        if (nodes == null)
            nodes = new Dictionary<int, Node>();

        switch (nodeType)
        {
            case NodeType.BaseNode:
                break;
            case NodeType.StartNode:
                nodes.Add(0, new StartNode(0, position, 200, 100, stylesheet, OnClickInKnob, OnClickOutKnob, OnClickRemoveNode));
                break;
            case NodeType.SpeakerNode:
                nodes.Add(lastId, new SpeakerNode(lastId, position, 215, 175, stylesheet, OnClickInKnob, OnClickOutKnob, OnClickRemoveNode));
                lastId++;
                break;
            case NodeType.DialogueNode:
                nodes.Add(lastId, new DialogueNode(lastId, position, 215, 175, stylesheet, OnClickInKnob, OnClickOutKnob, OnClickRemoveNode));
                lastId++;
                break;
            case NodeType.ChoiceNode:
                nodes.Add(lastId, new ChoiceNode(lastId, position, 300, 100, stylesheet, OnClickInKnob, OnClickOutKnob, OnClickRemoveNode));
                lastId++;
                break;
            case NodeType.EndNode:
                nodes.Add(lastId, new EndNode(lastId, position, 200, 100, stylesheet, OnClickInKnob, OnClickOutKnob, OnClickRemoveNode));
                lastId++;
                break;
            default:
                break;
        }

    }

    private void OnClickInKnob(ConnectionKnob inKnob)
    {
        selectedInKnob = inKnob;

        if (selectedOutKnob != null)
        {
            if (selectedOutKnob.ownerNode != selectedInKnob.ownerNode  
                && selectedOutKnob.allowedConnections.Contains(selectedInKnob.ownerNode.nodeType) 
                && selectedInKnob.allowedConnections.Contains(selectedOutKnob.ownerNode.nodeType)
                && (selectedInKnob.connections.Count == 0 || selectedInKnob.allowMoreConnections)
                && (selectedOutKnob.connections.Count == 0 || selectedOutKnob.allowMoreConnections))
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
                ClearConnectionSelection();
        }
        else
        {
            Connection connection = connections.FirstOrDefault(x => x.inKnob == selectedInKnob);
            if (connection == null)
                return;
            RemoveConnection(connection);
            selectedOutKnob = connection.outKnob;
            selectedInKnob = null;
            GUI.changed = true;
        }
    }

    private void OnClickOutKnob(ConnectionKnob outKnob)
    {
        selectedOutKnob = outKnob;

        if (selectedInKnob != null)
        {
            if (selectedOutKnob.ownerNode != selectedInKnob.ownerNode
                && selectedOutKnob.allowedConnections.Contains(selectedInKnob.ownerNode.nodeType)
                && selectedInKnob.allowedConnections.Contains(selectedOutKnob.ownerNode.nodeType)
                && (selectedInKnob.connections.Count == 0 || selectedInKnob.allowMoreConnections)
                && (selectedOutKnob.connections.Count == 0 || selectedOutKnob.allowMoreConnections))
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
                ClearConnectionSelection();
        }
    }

    private void CreateConnection()
    {
        if (connections == null)
            connections = new List<Connection>();

        Connection connection = new Connection(selectedInKnob, selectedOutKnob, RemoveConnection);
        selectedInKnob.connections.Add(connection);
        selectedOutKnob.connections.Add(connection);
        connections.Add(connection);
    }
    
    private void ClearConnectionSelection()
    {
        selectedInKnob = null;
        selectedOutKnob = null;
    }

    private void OnClickRemoveNode(Node node)
    {
        if (connections != null)
        {
            List<Connection> connectionsToRemove = new List<Connection>();

            foreach (Connection connection in connections)
            {
                if (node.inKnobs.Contains(connection.inKnob) || node.outKnobs.Contains(connection.outKnob))
                    connectionsToRemove.Add(connection);
            }

            foreach (Connection connection in connectionsToRemove)
            {
                RemoveConnection(connection);
            }
        }
        nodes.Remove(node.id);
    }

    private void OnDrag(Vector2 delta)
    {
        drag = delta;
        if (nodes != null)
        {
            foreach (Node node in nodes.Values)
                node.DragNode(delta);
        }

        GUI.changed = true;
    }
}
