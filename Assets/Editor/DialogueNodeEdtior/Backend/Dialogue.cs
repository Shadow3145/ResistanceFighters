using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    public List<Node> nodes = new List<Node>();
    public List<Connection> connections = new List<Connection>();
}
