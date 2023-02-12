using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    public Dictionary<int, Node> nodes = new Dictionary<int, Node>();
    public List<Connection> connections = new List<Connection>();

}
