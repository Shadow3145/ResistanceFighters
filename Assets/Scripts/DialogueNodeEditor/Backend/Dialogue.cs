using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueData
{
    public int lastNodeId;
    public int lastConnectionId;
    public List<DialogueNodeData> dialogueNodes;
    public List<SpeakerNodeData> speakerNodes;
    public List<ChoiceNodeData> choiceNodes;
    public List<NodeData> startNodes;
    public List<NodeData> endNodes;

    public DialogueData(int lastNodeId, int lastConnectionId, List<DialogueNode> dialogueNodes, List<SpeakerNode> speakerNodes,
        List<ChoiceNode> choiceNodes, List<StartNode> startNodes, List<EndNode> endNodes)
    {
        this.lastNodeId = lastNodeId;
        this.lastConnectionId = lastConnectionId;
        this.dialogueNodes = new List<DialogueNodeData>();
        this.speakerNodes = new List<SpeakerNodeData>();
        this.choiceNodes = new List<ChoiceNodeData>();
        this.startNodes = new List<NodeData>();
        this.endNodes = new List<NodeData>();
        foreach (DialogueNode dNode in dialogueNodes)
            this.dialogueNodes.Add(new DialogueNodeData(dNode.id, dNode.rect, dNode.inKnobs, dNode.outKnobs, dNode.dialogue));
        foreach (SpeakerNode sNode in speakerNodes)
            this.speakerNodes.Add(new SpeakerNodeData(sNode.id, sNode.rect, sNode.inKnobs, sNode.outKnobs, sNode.name, sNode.icon));
        foreach (ChoiceNode cNode in choiceNodes)
            this.choiceNodes.Add(new ChoiceNodeData(cNode.id, cNode.rect, cNode.inKnobs, cNode.outKnobs, cNode.choices));
        foreach (StartNode sNode in startNodes)
            this.startNodes.Add(new NodeData(sNode.id, sNode.rect, sNode.inKnobs, sNode.outKnobs));
        foreach (EndNode eNode in endNodes)
            this.endNodes.Add(new NodeData(eNode.id, eNode.rect, eNode.inKnobs, eNode.outKnobs));       
    }
}

[CreateAssetMenu(menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    public int lastNodeId;
    public int lastConnectionId;
    
    private Dictionary<int, Node> nodes = new Dictionary<int, Node>();

    public List<DialogueNode> dialogueNodes = new List<DialogueNode>();
    public List<SpeakerNode> speakerNodes = new List<SpeakerNode>();
    public List<ChoiceNode> choiceNodes = new List<ChoiceNode>();
    public List<StartNode> startNodes = new List<StartNode>();
    public List<EndNode> endNodes = new List<EndNode>();


    public List<Connection> connectionsList = new List<Connection>();

    private Dictionary<int, Connection> connections = new Dictionary<int, Connection>();

    public void SaveData()
    {
        DialogueData dialogueData = new DialogueData(lastNodeId, lastConnectionId, dialogueNodes, speakerNodes, choiceNodes, startNodes, endNodes);
        string dialogue = JsonUtility.ToJson(dialogueData);
        System.IO.File.WriteAllText("Assets/Resources/Dialogues/" + name + ".json", dialogue);
    }

    public void LoadData()
    {
        string dialogue = System.IO.File.ReadAllText("Assets/Resources/Dialogues/" + name + ".json");
        DialogueData dialogueData = JsonUtility.FromJson<DialogueData>(dialogue);
        lastNodeId = dialogueData.lastNodeId;
        lastConnectionId = dialogueData.lastConnectionId;

        dialogueNodes = new List<DialogueNode>();
        speakerNodes = new List<SpeakerNode>();
        choiceNodes = new List<ChoiceNode>();
        startNodes = new List<StartNode>();
        endNodes = new List<EndNode>();

        foreach (DialogueNodeData d in dialogueData.dialogueNodes)
            dialogueNodes.Add(new DialogueNode(d.id, d.rect.position, d.rect.width, d.rect.height, d.inKnobs, d.outKnobs, d.dialogue));
        foreach (SpeakerNodeData s in dialogueData.speakerNodes)
            speakerNodes.Add(new SpeakerNode(s.id, s.rect.position, s.rect.width, s.rect.height, s.inKnobs, s.outKnobs, s.speakerName, s.icon));
        foreach (ChoiceNodeData c in dialogueData.choiceNodes)
            choiceNodes.Add(new ChoiceNode(c.id, c.rect.position, c.rect.width, c.rect.height, c.inKnobs, c.outKnobs, c.choices));
        foreach (NodeData n in dialogueData.startNodes)
            startNodes.Add(new StartNode(n.id, n.rect.position, n.rect.width, n.rect.height, n.inKnobs, n.outKnobs));
        foreach (NodeData n in dialogueData.endNodes)
            endNodes.Add(new EndNode(n.id, n.rect.position, n.rect.width, n.rect.height, n.inKnobs, n.outKnobs));
    }

    private void CreateNodeDictionary()
    {
        foreach (Node node in dialogueNodes)
            nodes.Add(node.id, node);
        foreach (Node node in speakerNodes)
            nodes.Add(node.id, node);
        foreach (Node node in choiceNodes)
            nodes.Add(node.id, node);
        foreach (Node node in startNodes)
            nodes.Add(node.id, node);
        foreach (Node node in endNodes)
            nodes.Add(node.id, node);
    }

    private void CreateConnectionsDictionary()
    {
        foreach (Connection connection in connectionsList)
            connections.Add(connection.id, connection);
    }

    private int GetConnectionId(int nodeId, int knobIndex)
    {
        if (nodes[nodeId].outKnobs == null || knobIndex >= nodes[nodeId].outKnobs.Count ||
            nodes[nodeId].outKnobs[knobIndex].connections == null || nodes[nodeId].outKnobs[knobIndex].connections.Count <= 0 || 
            !connections.ContainsKey(nodes[nodeId].outKnobs[knobIndex].connections[0]))
            return -1;
        return nodes[nodeId].outKnobs[knobIndex].connections[0];
    }

    private int GetConnectionId(ConnectionKnob knob)
    {
        if (knob.connections.Count <= 0 || !connections.ContainsKey(knob.connections[0]))
            return -1;
        return  knob.connections[0];
    }

    public (int, NodeType) GetFirstNode()
    {
        CreateNodeDictionary();
        CreateConnectionsDictionary();
        if (nodes.Count == 0 || connections.Count == 0 || !nodes.ContainsKey(0))
        {
            Debug.Log(nodes.Count.ToString() + " " + connections.Count.ToString());
            return (-1, NodeType.BaseNode);
        }
        int connectionId = GetConnectionId(0, 0);
        if (connectionId == -1)
        {
            Debug.Log("Invalid Connection");
            return (-1, NodeType.BaseNode);
        }

        ConnectionKnob knob = connections[nodes[0].outKnobs[0].connections[0]].inKnob;
        Node node = nodes[knob.ownerNodeId];

        return (node.id, node.nodeType);
    }

    public (int, NodeType) GetNextNode(int id, int choiceIndex= -1)
    {
        if (!nodes.ContainsKey(id))
            return (-1, NodeType.BaseNode);
        
        if (choiceIndex == -1)
        {
            int connectionID = GetConnectionId(id, 0);
            if (connectionID == -1)
                return (-1, NodeType.BaseNode);
            int newId = connections[connectionID].inKnob.ownerNodeId;
            if (!nodes.ContainsKey(newId) || nodes[newId].nodeType == NodeType.EndNode)
                return (-1, NodeType.BaseNode);
            else
                return (newId, nodes[newId].nodeType);
        }
        else
        {
            int connectionID = GetConnectionId(id, choiceIndex);
            if (connectionID == -1)
                return (-1, NodeType.BaseNode);
            int newId = connections[connectionID].inKnob.ownerNodeId;
            if (!nodes.ContainsKey(newId) || nodes[newId].nodeType == NodeType.EndNode)
                return (-1, NodeType.BaseNode);
            else
                return (newId, nodes[newId].nodeType);
        }
    }

    public string GetDialogueText(int id)
    {
        if (!nodes.ContainsKey(id) || nodes[id].nodeType != NodeType.DialogueNode)
            return null;
        DialogueNode n = dialogueNodes.Find(x => x.id == id);
        if (n == null)
            return null;
        return n.dialogue;
    }

    public List<string> GetChoices(int id)
    {
        if (!nodes.ContainsKey(id) || nodes[id].nodeType != NodeType.ChoiceNode)
            return null;
        ChoiceNode n = choiceNodes.Find(x => x.id == id);
        if (n == null)
            return null;
        return n.choices;
    }

    public int GetSpeaker(int id)
    {
        if (!nodes.ContainsKey(id))
            return -1;
        
        ConnectionKnob speakerKnob = nodes[id].inKnobs.Find(x => x.subType == ConnectionKnobSubType.Speaker);
        if (speakerKnob == null)
            return -1;
        int connectionID = GetConnectionId(speakerKnob);
        if (connectionID == -1)
            return -1;
        return connections[connectionID].outKnob.ownerNodeId;
    }

    public Sprite GetSpeakerIcon(int id)
    {
        if (!nodes.ContainsKey(id) || nodes[id].nodeType != NodeType.SpeakerNode)
            return null;
        SpeakerNode n = speakerNodes.Find(x => x.id == id);
        if (n == null)
            return null;
        return n.icon;
    }

    public string GetSpeakerName(int id)
    {
        if (!nodes.ContainsKey(id) || nodes[id].nodeType != NodeType.SpeakerNode)
            return null;
        
        SpeakerNode n = speakerNodes.Find(x => x.id == id);
        if (n == null)
            return null;
        return n.name;
    }
}
