using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    public int lastNodeId;
    public int lastConnectionId;
    public List<Node> nodesList = new List<Node>();
    
    private Dictionary<int, Node> nodes = new Dictionary<int, Node>();

    public Dictionary<int, DialogueNodeData> dialogueNodes = new Dictionary<int, DialogueNodeData>();
    public Dictionary<int, SpeakerNodeData> speakerNodes = new Dictionary<int, SpeakerNodeData>();
    public Dictionary<int, ChoiceNodeData> choiceNodes = new Dictionary<int, ChoiceNodeData>();

    public List<Connection> connectionsList = new List<Connection>();

    private Dictionary<int, Connection> connections = new Dictionary<int, Connection>();

    private void CreateNodeDictionary()
    {
        foreach (Node node in nodesList)
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
        if (!nodes.ContainsKey(id) || nodes[id].nodeType != NodeType.DialogueNode || !dialogueNodes.ContainsKey(id))
            return null;

        return dialogueNodes[id].dialogue;
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
        if (!nodes.ContainsKey(id) || nodes[id].nodeType != NodeType.SpeakerNode || !speakerNodes.ContainsKey(id))
            return null;
        
        return speakerNodes[id].icon;
    }

    public string GetSpeakerName(int id)
    {
        if (!nodes.ContainsKey(id) || nodes[id].nodeType != NodeType.SpeakerNode || !speakerNodes.ContainsKey(id))
            return null;
        
        return speakerNodes[id].name;
    }
}
