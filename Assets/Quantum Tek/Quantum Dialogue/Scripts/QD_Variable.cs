﻿using System.Collections.Generic;
using UnityEngine;

namespace QuantumTek.QuantumDialogue
{
    [System.Serializable]
    public class VariableInfo
    {
        public Object parentObject = null;
        public int componentIndex = -1;
        public int fieldIndex = -1;
        
        public VariableInfo(Object parent, int componentIndex, int fieldIndex)
        {
            this.parentObject = parent;
            this.componentIndex = componentIndex;
            this.fieldIndex = fieldIndex;
        }

        public VariableInfo()
        {}

        public Component GetComponent()
        {
            if (parentObject == null || componentIndex == -1)
                return null;

            GameObject parent = parentObject as GameObject;

            return parent.GetComponents<Component>()[componentIndex];
        }

        public string GetValue()
        {
            if (parentObject == null || componentIndex == -1 || fieldIndex == -1)
                return null;
            Component component = GetComponent();
            return component.GetType().GetFields()[fieldIndex].GetValue(component).ToString();
        }
    }

    [System.Serializable]
    public class QD_Variable
    {
        public int ID;
        public List<VariableInfo> VariableInfos = new List<VariableInfo>();
        public int Parent = -1;

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
            if (knobType == QD_KnobType.Input)
            { }
            else if (knobType == QD_KnobType.Output)
            {
                Parent = connectionID;
            }
        }

        /// <summary>
        /// The function called to modify the node's data whenever a node disconnects.
        /// </summary>
        /// <param name="dialogue">The dialogue data.</param>
        /// <param name="connectionType">The type of the connecting node.</param>
        /// <param name="connectionID">The id of the connected node.</param>
        public void OnDisconnect(QD_Dialogue dialogue, QD_NodeType connectionType, int connectionID)
        {
            if (Parent == connectionID)
                    Parent = -1;
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
            if (knobType == QD_KnobType.Output)
            {
                if (knobID == 0 && Parent == connectionID)
                    Parent = -1;
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