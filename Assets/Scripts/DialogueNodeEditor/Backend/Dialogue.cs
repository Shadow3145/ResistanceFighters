using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        return nodes[0].outKnobs[0].connections[0];
    }

    private int GetConnectionId(ConnectionKnob knob)
    {
        if (knob.connections.Count <= 0 || !connections.ContainsKey(knob.connections[0]))
            return -1;
        return  knob.connections[0];
    }

    public int GetFirstNode()
    {
        CreateNodeDictionary();
        CreateConnectionsDictionary();
        if (nodes.Count == 0 || connections.Count == 0 || !nodes.ContainsKey(0))
        {
            Debug.Log(nodes.Count.ToString() + " " + connections.Count.ToString());
            return -1;
        }
        int connectionId = GetConnectionId(0, 0);
        if (connectionId == -1)
        {
            Debug.Log("Invalid Connection");
            return -1;
        }

        return connections[nodes[0].outKnobs[0].connections[0]].inKnob.ownerNodeId;
    }

    public int GetNextNode(int id, int choiceIndex= -1)
    {
        if (!nodes.ContainsKey(id))
            return -1;
        
        if (choiceIndex == -1)
        {
            int connectionID = GetConnectionId(id, 0);
            if (connectionID == -1)
                return -1;
            int newId = connections[connectionID].inKnob.ownerNodeId;
            if (!nodes.ContainsKey(newId) || nodes[newId].nodeType == NodeType.EndNode)
                return -1;
            else
                return newId;
        }
        else
        {
            int connectionID = GetConnectionId(id, choiceIndex);
            if (connectionID == -1)
                return -1;
            int newId = connections[connectionID].inKnob.ownerNodeId;
            if (!nodes.ContainsKey(newId) || nodes[newId].nodeType == NodeType.EndNode)
                return -1;
            else
                return newId;
        }
    }

    public string GetDialogueText(int id)
    {
        if (!nodes.ContainsKey(id) || nodes[id].nodeType != NodeType.DialogueNode)
            return null;
        DialogueNode n = dialogueNodes.Find(x => x.id == id);
        if (n == null)
            return null;
        return n.nodeData.dialogue;
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
        return speakerNodes[id].nodeData.icon;
    }

    public string GetSpeakerName(int id)
    {
        if (!nodes.ContainsKey(id) || nodes[id].nodeType != NodeType.SpeakerNode)
            return null;
        
        SpeakerNode n = speakerNodes.Find(x => x.id == id);
        if (n == null)
            return null;
        return n.nodeData.name;
    }
}
