using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace QuantumTek.QuantumDialogue
{
    [System.Serializable]
    public class QD_Variable
    {
        public int ID;
        public List<FieldInfo> Variables = new List<FieldInfo>();
        public List<Object> Objects = new List<Object>();
        public List<Component> Scripts = new List<Component>();
        public List<int> NextMessages = new List<int>();

        public QD_Variable(int id)
        { ID = id; }

        /// <summary>
        /// The function called to modify the node's data whenever a node connects.
        /// </summary>
        /// <param name="dialogue">The dialogue data.</param>
        /// <param name="connectionType">The type of the connecting node.</param>
        /// <param name="connectionID">The id of the connecting node.</param>
        /// <param name="connectionKnobID">The id of the connecting knob.</param>
        /// <param name="knobID">The id of this node's knob.</param>
        /// <param name="knobType">The type of this node's knob.</param>
        public void OnConnect(QD_Dialogue dialogue, QD_NodeType connectionType, int connectionID, int connectionKnobID, int knobID, QD_KnobType knobType)
        {
            if (knobID >= NextMessages.Count)
                return;
            NextMessages[knobID] = connectionID;
        }

        /// <summary>
        /// The function called to modify the node's data whenever a node disconnects.
        /// </summary>
        /// <param name="dialogue">The dialogue data.</param>
        /// <param name="connectionType">The type of the connecting node.</param>
        /// <param name="connectionID">The id of the connected node.</param>
        public void OnDisconnect(QD_Dialogue dialogue, QD_NodeType connectionType, int connectionID)
        {
            int count = NextMessages.Count;
            for (int i = 0; i < count; ++i)
                if (NextMessages[i] == connectionID)
                    NextMessages[i] = -1;
        }

        /// <summary>
        /// The function called to modify the node's data whenever a node disconnects.
        /// </summary>
        /// <param name="dialogue">The dialogue data.</param>
        /// <param name="connectionType">The type of the connecting node.</param>
        /// <param name="connectionID">The id of the connected node.</param>
        /// <param name="knobID">The id of the knob.</param>
        /// <param name="knobType">The type of the knob.</param>
        public void OnDisconnect(QD_Dialogue dialogue, QD_NodeType connectionType, int connectionID, int knobID, QD_KnobType knobType)
        {
            if (knobID >= NextMessages.Count)
                return;
            if (knobType == QD_KnobType.Output)
            {
                int count = NextMessages.Count;
                for (int i = 0; i < count; ++i)
                    if (NextMessages[i] == connectionID)
                        NextMessages[i] = -1;
            }
        }

        /// <summary>
        /// The function called to modify the node's data whenever a node disconnects.
        /// </summary>
        /// <param name="dialogue">The dialogue data.</param>
        /// <param name="connectionType">The type of the connecting node.</param>
        /// <param name="connectionID">The id of the connected node.</param>
        /// <param name="connectionKnobID">The id of the connected knob.</param>
        /// <param name="knobID">The id of the knob.</param>
        /// <param name="knobType">The type of the knob.</param>
        public void OnDisconnect(QD_Dialogue dialogue, QD_NodeType connectionType, int connectionID, int connectionKnobID, int knobID, QD_KnobType knobType)
        {
            OnDisconnect(dialogue, connectionType, connectionID, knobID, knobType);
        }
    }
}