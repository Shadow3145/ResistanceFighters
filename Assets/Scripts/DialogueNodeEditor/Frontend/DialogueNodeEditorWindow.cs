using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

public class DialogueNodeEditorWindow : EditorWindow
{
    private Stylesheet stylesheet;

    private ConnectionKnob selectedInKnob;
    private ConnectionKnob selectedOutKnob;

    private Rect topPanel;

    private Vector2 drag;
    private Vector2 offset;

    private bool stopDrawing = false;
    private bool isDrawing = false;

    public string selectedDialogue ="";
    private int selectedDialogueIndex;
    public Dialogue[] dialogues;

    private bool reloadDialogue = true;

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
        Resources.LoadAll("");
        stylesheet = Resources.FindObjectsOfTypeAll<Stylesheet>()[0];
        dialogues = Resources.FindObjectsOfTypeAll<Dialogue>();
        if (dialogues == null)
            return;
        for (int i = 0; i < dialogues.Length; i++)
        {
            dialogues[i].LoadData();
            ResetActions(i);
        }
    }

    private bool SetDialogue()
    {
        dialogues = Resources.FindObjectsOfTypeAll<Dialogue>();
        string[] names = dialogues.Select(x => x.name).ToArray();

        if (selectedDialogue != "" && names.Contains(selectedDialogue))
        {
            int index = ArrayUtility.IndexOf(names, selectedDialogue);
            if (index == selectedDialogueIndex && !reloadDialogue)
                return true;
            dialogues[index].LoadData();
            selectedDialogueIndex = index;
            ResetActions(selectedDialogueIndex);
            return true;
        }

        return false;
    }

    private void ResetActions(int dialogueIndex)
    {

        foreach (Connection connection in dialogues[dialogueIndex].connectionsList)
        {
            connection.RemoveConnection = RemoveConnection;
        }
        ResetActions(dialogues[dialogueIndex].startNodes, dialogueIndex);
        ResetActions(dialogues[dialogueIndex].endNodes, dialogueIndex);
        ResetActions(dialogues[dialogueIndex].speakerNodes, dialogueIndex);
        ResetActions(dialogues[dialogueIndex].dialogueNodes, dialogueIndex);
        ResetActions(dialogues[dialogueIndex].choiceNodes, dialogueIndex);
        foreach (ChoiceNode node in dialogues[dialogueIndex].choiceNodes)
        {
            node.OnRemoveChoice = OnRemoveChoice;
            node.OnClickOutKnob = OnClickOutKnob;
        }
    }

    private void ResetActions<T>(List<T> nodes, int dialogueIndex) where T : Node
    {
        foreach (Node node in nodes)
        {
            node.OnRemoveNode = OnClickRemoveNode;
            foreach (ConnectionKnob knob in node.inKnobs)
            {
                knob.OnClickConnectionKnob = OnClickInKnob;
                foreach (int connection in knob.connections)
                {
                    Connection c = dialogues[dialogueIndex].connectionsList.Find(x => x.id == connection);
                    if (c != null)
                        c.inKnob = knob;
                }
            }

            foreach (ConnectionKnob knob in node.outKnobs)
            {
                knob.OnClickConnectionKnob = OnClickOutKnob;
                foreach (int connection in knob.connections)
                {
                    Connection c = dialogues[dialogueIndex].connectionsList.Find(x => x.id == connection);
                    if (c != null)
                        c.outKnob = knob;
                }
            }
        }
    }

    private void OnGUI()
    {
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        if (stylesheet == null)
            stylesheet = Resources.FindObjectsOfTypeAll<Stylesheet>()[0];

        DrawTopPanel();
        if (!SetDialogue())
            return;
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

        if (GUI.Button(new Rect(300, 10, 30, 30), "S", stylesheet.button))
        {
            if (this.dialogues.Length > selectedDialogueIndex && selectedDialogueIndex >= 0)
                this.dialogues[selectedDialogueIndex].SaveData();
        }


        Resources.LoadAll<Dialogue>("");
        string[] dialogues = Resources.FindObjectsOfTypeAll<Dialogue>().Select(x => x.name).ToArray();
        Array.Sort(dialogues);
        int index = selectedDialogue != ""
            ? ArrayUtility.IndexOf(dialogues, selectedDialogue)
            : -1;
        int selectedIndex = EditorGUI.Popup(new Rect(20, 10, 200, 30), index, dialogues);
        if (selectedIndex != -1)
        {
            selectedDialogue = dialogues[selectedIndex];
            reloadDialogue = (selectedDialogueIndex != selectedIndex);
        }
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
        foreach (StartNode node in dialogues[selectedDialogueIndex].startNodes)
            node.DrawNode();
        foreach (EndNode node in dialogues[selectedDialogueIndex].endNodes)
            node.DrawNode();
        foreach (DialogueNode node in dialogues[selectedDialogueIndex].dialogueNodes)
            node.DrawNode();
        foreach (ChoiceNode node in dialogues[selectedDialogueIndex].choiceNodes)
            node.DrawNode();
        foreach (SpeakerNode node in dialogues[selectedDialogueIndex].speakerNodes)
            node.DrawNode();    
    }

    private void DrawConnections()
    {
        Connection toRemove = null;
        if (dialogues[selectedDialogueIndex].connectionsList != null)
        {
            foreach (Connection connection in dialogues[selectedDialogueIndex].connectionsList)
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
        connection.inKnob.connections.Remove(connection.id);
        connection.outKnob.connections.Remove(connection.id);
        dialogues[selectedDialogueIndex].connectionsList.Remove(connection);
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
        ProcessNodesEvents(e, dialogues[selectedDialogueIndex].dialogueNodes);
        ProcessNodesEvents(e, dialogues[selectedDialogueIndex].choiceNodes);
        ProcessNodesEvents(e, dialogues[selectedDialogueIndex].speakerNodes);
        ProcessNodesEvents(e, dialogues[selectedDialogueIndex].startNodes);
        ProcessNodesEvents(e, dialogues[selectedDialogueIndex].endNodes);
    }

    private void ProcessNodesEvents<T>(Event e, List<T> nodes) where T : Node
    {
        if (nodes == null)
            return;
        foreach (Node node in nodes)
        {
            bool guiChanged = node.ProcessEvents(e);

            if (guiChanged)
                GUI.changed = true;
        }
    }

    private void ShowContextMenu(Vector2 position)
    {
        GenericMenu contextMenu = new GenericMenu();
        foreach (NodeType nodeType in Enum.GetValues(typeof(NodeType)))
        {
            if (nodeType == NodeType.BaseNode || 
            (nodeType == NodeType.StartNode && dialogues[selectedDialogueIndex].startNodes.Count > 0))
                continue;
            contextMenu.AddItem(new GUIContent(nodeType.ToString()), false, () => OnClickAddNode(position, nodeType));
        }
        contextMenu.ShowAsContext();
    }

    private void OnClickAddNode(Vector2 position, NodeType nodeType = NodeType.BaseNode)
    {
        switch (nodeType)
        {
            case NodeType.BaseNode:
                break;
            case NodeType.StartNode:
                StartNode stNode = new StartNode(0, position, 200, 100, null, null);
                stNode.Init(OnClickInKnob, OnClickOutKnob, OnClickRemoveNode);
                dialogues[selectedDialogueIndex].startNodes.Add(stNode);
                break;
            case NodeType.SpeakerNode:
                SpeakerNode sNode = new SpeakerNode(dialogues[selectedDialogueIndex].lastNodeId, position, 215, 175, null, null, "", null);
                sNode.Init(OnClickInKnob, OnClickOutKnob, OnClickRemoveNode);
                dialogues[selectedDialogueIndex].speakerNodes.Add(sNode);
                dialogues[selectedDialogueIndex].lastNodeId++;
                break;
            case NodeType.DialogueNode:
                DialogueNode dNode = new DialogueNode(dialogues[selectedDialogueIndex].lastNodeId, position, 215, 175, null, null, "");
                dNode.Init(OnClickInKnob, OnClickOutKnob, OnClickRemoveNode);
                dialogues[selectedDialogueIndex].dialogueNodes.Add(dNode);
                dialogues[selectedDialogueIndex].lastNodeId++;
                break;
            case NodeType.ChoiceNode:
                ChoiceNode cNode = new ChoiceNode(dialogues[selectedDialogueIndex].lastNodeId, position, 300, 100, null, null, new List<string>());
                cNode.Init(OnClickInKnob, OnClickOutKnob, OnClickRemoveNode);
                cNode.OnRemoveChoice = OnRemoveChoice;
                dialogues[selectedDialogueIndex].choiceNodes.Add(cNode);
                dialogues[selectedDialogueIndex].lastNodeId++;
                break;
            case NodeType.EndNode:
                EndNode eNode = new EndNode(dialogues[selectedDialogueIndex].lastNodeId, position, 200, 100, null, null);
                eNode.Init(OnClickInKnob, OnClickOutKnob, OnClickRemoveNode);
                dialogues[selectedDialogueIndex].endNodes.Add(eNode);
                dialogues[selectedDialogueIndex].lastNodeId++;
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
            if (selectedOutKnob.ownerNodeId != selectedInKnob.ownerNodeId
                && selectedOutKnob.allowedConnections.Contains(GetNodeType(selectedInKnob.ownerNodeId)) 
                && selectedInKnob.allowedConnections.Contains(GetNodeType(selectedOutKnob.ownerNodeId))
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
            Connection connection = dialogues[selectedDialogueIndex].connectionsList.FirstOrDefault(x => x.inKnob == selectedInKnob);
            if (connection == null)
                return;
            RemoveConnection(connection);
            selectedOutKnob = connection.outKnob;
            selectedInKnob = null;
            GUI.changed = true;
        }
    }

    private NodeType GetNodeType(int id)
    {
        if (id == 0)
            return NodeType.StartNode;
        
        if (dialogues[selectedDialogueIndex].dialogueNodes.Find(x => x.id == id) != null)
            return NodeType.DialogueNode;

        if (dialogues[selectedDialogueIndex].speakerNodes.Find(x => x.id == id) != null)
            return NodeType.SpeakerNode;

        if (dialogues[selectedDialogueIndex].choiceNodes.Find(x => x.id == id) != null)
            return NodeType.ChoiceNode;

        if (dialogues[selectedDialogueIndex].endNodes.Find(x => x.id == id) != null)
            return NodeType.EndNode;
        
        return NodeType.BaseNode;
    }

    private void OnClickOutKnob(ConnectionKnob outKnob)
    {
        selectedOutKnob = outKnob;

        if (selectedInKnob != null)
        {
            if (selectedOutKnob.ownerNodeId != selectedInKnob.ownerNodeId
                && selectedOutKnob.allowedConnections.Contains(GetNodeType(selectedInKnob.ownerNodeId))
                && selectedInKnob.allowedConnections.Contains(GetNodeType(selectedOutKnob.ownerNodeId))
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
        if (dialogues[selectedDialogueIndex].connectionsList == null)
            dialogues[selectedDialogueIndex].connectionsList = new List<Connection>();

        Connection connection = new Connection(dialogues[selectedDialogueIndex].lastConnectionId, selectedInKnob, selectedOutKnob, RemoveConnection);
        dialogues[selectedDialogueIndex].lastConnectionId++;
        selectedInKnob.connections.Add(connection.id);
        selectedOutKnob.connections.Add(connection.id);
        dialogues[selectedDialogueIndex].connectionsList.Add(connection);
    }
    
    private void ClearConnectionSelection()
    {
        selectedInKnob = null;
        selectedOutKnob = null;
    }

    private void OnClickRemoveNode(Node node)
    {
        if (dialogues[selectedDialogueIndex].connectionsList != null)
        {
            List<Connection> connectionsToRemove = new List<Connection>();

            foreach (Connection connection in dialogues[selectedDialogueIndex].connectionsList)
            {
                if (node.inKnobs.Contains(connection.inKnob) || node.outKnobs.Contains(connection.outKnob))
                    connectionsToRemove.Add(connection);
            }

            foreach (Connection connection in connectionsToRemove)
            {
                RemoveConnection(connection);
            }
        }

        switch (node.nodeType)
        {
            case NodeType.StartNode:
                dialogues[selectedDialogueIndex].startNodes.Remove(node as StartNode);
                break;
            case NodeType.SpeakerNode:
                dialogues[selectedDialogueIndex].speakerNodes.Remove(node as SpeakerNode);
                break;
            case NodeType.DialogueNode:
                dialogues[selectedDialogueIndex].dialogueNodes.Remove(node as DialogueNode);
                break;
            case NodeType.ChoiceNode:
                dialogues[selectedDialogueIndex].choiceNodes.Remove(node as ChoiceNode);
                break;
            case NodeType.EndNode:
                dialogues[selectedDialogueIndex].endNodes.Remove(node as EndNode);
                break;
            default:
                break;
        }
    }

    private void OnRemoveChoice(int connectionID)
    {
        dialogues[selectedDialogueIndex].connectionsList.Remove(dialogues[selectedDialogueIndex].connectionsList.Find(x => x.id == connectionID));
    }

    private void OnDrag(Vector2 delta)
    {
        OnDrag(delta, dialogues[selectedDialogueIndex].choiceNodes);
        OnDrag(delta, dialogues[selectedDialogueIndex].dialogueNodes);
        OnDrag(delta, dialogues[selectedDialogueIndex].startNodes);
        OnDrag(delta, dialogues[selectedDialogueIndex].endNodes);
        OnDrag(delta, dialogues[selectedDialogueIndex].speakerNodes);
    }

    private void OnDrag<T>(Vector2 delta, List<T> nodes) where T : Node
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
