using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace QuantumTek.QuantumDialogue.Editor
{
    [System.Serializable]
    public class QD_ChoiceNode : QD_Node
    {
        public new string WindowTitle => "Choice";
        public QD_Choice Data;

        public QD_ChoiceNode(int id, QD_NodeType type, float x = 0, float y = 0) : base(id, type, x, y)
        {
            Type = QD_NodeType.Speaker;
            Window = new Rect(0, 0, 300, 65);
            Inputs = new List<QD_Knob>
            {
                new QD_Knob(0, "Previous Messages", QD_KnobType.Input, 7.5f, false)
            };
            Outputs = new List<QD_Knob>
            { };
            AllowedInputs = new List<QD_ConnectionRule>
            {
                new QD_ConnectionRule(QD_NodeType.Message, 1, QD_NodeType.Choice, 0),
                new QD_ConnectionRule(QD_NodeType.Variables, 0, QD_NodeType.Choice, 1),
                new QD_ConnectionRule(QD_NodeType.Variables, 0, QD_NodeType.Choice, 2),
                new QD_ConnectionRule(QD_NodeType.Variables, 0, QD_NodeType.Choice, 3),
                new QD_ConnectionRule(QD_NodeType.Variables, 0, QD_NodeType.Choice, 4),
                new QD_ConnectionRule(QD_NodeType.Variables, 0, QD_NodeType.Choice, 5)

            };
            AllowedOutputs = new List<QD_ConnectionRule>
            { };
        }

        public override void DrawWindow(int id)
        {
            QD_Node node = QD_DialogueEditor.db.GetNode(ID);
            EditorGUI.BeginChangeCheck();
            List<string> fChoices = new List<string>();
            foreach (var choice in Data.Choices)
                fChoices.Add(choice);

            int count = Data.Choices.Count;
            if (GUI.Button(new Rect(140, 40 + count * 30, 20, 20), "+", QD_DialogueEditor.skin.button) && Data.Choices.Count < 6)
            {
                EditorUtility.SetDirty(QD_DialogueEditor.db);
                EditorUtility.SetDirty(QD_DialogueEditor.db.DataDB);
                int numOutputs = node.Outputs.Count;
                node.Outputs.Add(new QD_Knob(numOutputs, "Choice " + (numOutputs + 1), QD_KnobType.Output, 45 + numOutputs * 30, false));
                node.AllowedOutputs.Add(new QD_ConnectionRule(QD_NodeType.Message, 1, QD_NodeType.Choice, numOutputs));

                int numInputs = node.Inputs.Count;
                node.Inputs.Add(new QD_Knob(numInputs, "Choice " + (numInputs), QD_KnobType.Input, 45 + (numInputs-1) * 30, false));
                node.AllowedInputs.Add(new QD_ConnectionRule(QD_NodeType.Variables, numInputs, QD_NodeType.Choice, 0));

                node.Window.height += 30;
                Data.Choices.Add("");
                fChoices.Add("");
                Data.NextMessages.Add(-1);
                QD_DialogueEditor.db.DataDB.SetChoice(Data.ID, Data);
            }

            for (int i = count - 1; i >= 0; --i)
            {
                EditorGUI.LabelField(new Rect(5, 40 + i * 30, 65, 20), "Choice " + (i + 1), QD_DialogueEditor.skin.label);
                fChoices[i] = EditorGUI.TextField(new Rect(75, 40 + i * 30, 195, 20), fChoices[i], QD_DialogueEditor.skin.textField);

                if (i == count - 1)
                {
                    if (GUI.Button(new Rect(275, 42.5f + i * 30, 20, 20), "-", QD_DialogueEditor.skin.button))
                    {
                        EditorUtility.SetDirty(QD_DialogueEditor.db);
                        EditorUtility.SetDirty(QD_DialogueEditor.db.DataDB);
                        fChoices.RemoveAt(i);
                        QD_DialogueEditor.editor.selectedNode = node;
                        QD_DialogueEditor.editor.DetachNodes(i, QD_KnobType.Output);
                        Data.NextMessages.RemoveAt(i);
                        node.Outputs.RemoveAt(i);
                        node.Inputs.RemoveAt(i + 1);
                        node.Window.height -= 30;
                        QD_DialogueEditor.db.DataDB.SetChoice(Data.ID, Data);
                    }
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(QD_DialogueEditor.db);
                EditorUtility.SetDirty(QD_DialogueEditor.db.DataDB);
                Data.Choices = fChoices;
                QD_DialogueEditor.db.DataDB.SetChoice(Data.ID, Data);
            }

            GUI.DragWindow();
        }

        /// <summary>
        /// The function called to modify the node's data whenever a node connects.
        /// </summary>
        /// <param name="dialogue">The dialogue data.</param>
        /// <param name="connectionType">The type of the connecting node.</param>
        /// <param name="connectionID">The id of the connecting node.</param>
        /// <param name="connectionKnobID">The id of the connecting knob.</param>
        /// <param name="knobID">The id of this node's knob.</param>
        /// <param name="knobType">The type of this node's knob.</param>
        public override void OnConnect(QD_Dialogue dialogue, QD_NodeType connectionType, int connectionID, int connectionKnobID, int knobID, QD_KnobType knobType)
        {
            Data.OnConnect(dialogue, connectionType, connectionID, connectionKnobID, knobID, knobType);
            QD_DialogueEditor.db.DataDB.SetChoice(Data.ID, Data);
        }

        /// <summary>
        /// The function called to modify the node's data whenever a node disconnects.
        /// </summary>
        /// <param name="dialogue">The dialogue data.</param>
        /// <param name="connectionType">The type of the connecting node.</param>
        /// <param name="connectionID">The id of the connected node.</param>
        public override void OnDisconnect(QD_Dialogue dialogue, QD_NodeType connectionType, int connectionID)
        {
            Data.OnDisconnect(dialogue, connectionType, connectionID);
            QD_DialogueEditor.db.DataDB.SetChoice(Data.ID, Data);
        }

        /// <summary>
        /// The function called to modify the node's data whenever a node disconnects.
        /// </summary>
        /// <param name="dialogue">The dialogue data.</param>
        /// <param name="connectionType">The type of the connecting node.</param>
        /// <param name="connectionID">The id of the connected node.</param>
        /// <param name="knobID">The id of the knob.</param>
        /// <param name="knobType">The type of the knob.</param>
        public override void OnDisconnect(QD_Dialogue dialogue, QD_NodeType connectionType, int connectionID, int knobID, QD_KnobType knobType)
        {
            Data.OnDisconnect(dialogue, connectionType, connectionID, knobID, knobType);
            QD_DialogueEditor.db.DataDB.SetChoice(Data.ID, Data);
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
        public override void OnDisconnect(QD_Dialogue dialogue, QD_NodeType connectionType, int connectionID, int connectionKnobID, int knobID, QD_KnobType knobType)
        {
            Data.OnDisconnect(dialogue, connectionType, connectionID, connectionKnobID, knobID, knobType);
            QD_DialogueEditor.db.DataDB.SetChoice(Data.ID, Data);
        }
    }
}