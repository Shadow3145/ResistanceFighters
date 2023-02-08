using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class DialogueNodeEditorWindow : EditorWindow
{
    private List<Node> nodes;
    private List<Connection> connections;

    private GUIStyle nodeStyle;
    private GUIStyle selectedNodeStyle;
    private GUIStyle knobStyle;
    private GUIStyle buttonStyle;


    private ConnectionKnob selectedInKnob;
    private ConnectionKnob selectedOutKnob;

    private GUIStyle topPanelStyle;
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
        topPanelStyle = new GUIStyle();
        topPanelStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/darkviewbackground.png") as Texture2D;
        topPanelStyle.normal.textColor = Color.white;
        topPanelStyle.border = new RectOffset(12, 12, 12, 12);
        topPanel = new Rect(0, 0, this.position.width, 50);

        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        nodeStyle.border = new RectOffset(12, 12, 12, 12);

        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

        knobStyle = new GUIStyle();
        knobStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        knobStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        knobStyle.border = new RectOffset(4, 4, 12, 12);

        buttonStyle = new GUIStyle();
        buttonStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn.png") as Texture2D;
        buttonStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn on.png") as Texture2D;
        buttonStyle.border = new RectOffset(12, 12, 12, 12);
        buttonStyle.alignment = TextAnchor.UpperCenter;
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.fontSize = 20;
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
        GUI.Box(topPanel, "", topPanelStyle);

        if (GUI.Button(new Rect(250, 10, 30, 30), "+", buttonStyle))
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
            foreach (Node node in nodes)
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
            connections.Remove(toRemove);
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
            foreach (Node node in nodes)
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
        contextMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(position));
        contextMenu.ShowAsContext();
    }

    private void OnClickAddNode(Vector2 position)
    {
        if (nodes == null)
            nodes = new List<Node>();

       nodes.Add(new Node(position, 200, 100, nodeStyle, selectedNodeStyle ,knobStyle, OnClickInKnob, OnClickOutKnob, OnClickRemoveNode));
    }

    private void OnClickInKnob(ConnectionKnob inKnob)
    {
        selectedInKnob = inKnob;

        if (selectedOutKnob != null)
        {
            if (selectedOutKnob.ownerNode != selectedInKnob.ownerNode)
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
            connections.Remove(connection);
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
            if (selectedOutKnob.ownerNode != selectedInKnob.ownerNode)
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

        connections.Add(new Connection(selectedInKnob, selectedOutKnob));
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
                if (connection.inKnob == node.inKnob || connection.outKnob == node.outKnob)
                    connectionsToRemove.Add(connection);
            }

            foreach (Connection connection in connectionsToRemove)
                connections.Remove(connection);
        }

        nodes.Remove(node);
    }

    private void OnDrag(Vector2 delta)
    {
        drag = delta;
        if (nodes != null)
        {
            foreach (Node node in nodes)
                node.DragNode(delta);
        }

        GUI.changed = true;
    }
}
