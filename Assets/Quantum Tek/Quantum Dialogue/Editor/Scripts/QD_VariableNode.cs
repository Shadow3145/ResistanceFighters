using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

namespace QuantumTek.QuantumDialogue.Editor
{
    [System.Serializable]
    public class QD_VariableNode : QD_Node
    {
        public new string WindowTitle => "Variables";
        public QD_Variable Data;

        public QD_VariableNode(int id, QD_NodeType type, float x = 0, float y = 0) : base(id, type, x, y)
        {
            Type = QD_NodeType.Variables;
            Window = new Rect(0, 0, 300, 100);
            Inputs = new List<QD_Knob>
            {};
            Outputs = new List<QD_Knob>
            {
                new QD_Knob(0, "Parent", QD_KnobType.Output, 35, false)
            };
            AllowedInputs = new List<QD_ConnectionRule>
            {
            };
            AllowedOutputs = new List<QD_ConnectionRule>
            {
                new QD_ConnectionRule(QD_NodeType.Message, 2, QD_NodeType.Variables, 0),
                new QD_ConnectionRule(QD_NodeType.Choice, 1, QD_NodeType.Variables, 0),
                new QD_ConnectionRule(QD_NodeType.Choice, 2, QD_NodeType.Variables, 0),
                new QD_ConnectionRule(QD_NodeType.Choice, 3, QD_NodeType.Variables, 0),
                new QD_ConnectionRule(QD_NodeType.Choice, 4, QD_NodeType.Variables, 0),
                new QD_ConnectionRule(QD_NodeType.Choice, 5, QD_NodeType.Variables, 0)

            };
        }

        public override void DrawWindow(int id)
        {
            QD_Node node = QD_DialogueEditor.db.GetNode(ID);
            EditorGUI.BeginChangeCheck();
            /*List<FieldInfo> variables = new List<FieldInfo>();
            foreach (var variable in Data.Variables)
                variables.Add(variable);*/

            List<Object> objects = new List<Object>();
            foreach (var obj in Data.Objects)
                objects.Add(obj);

            List<Component> scripts = new List<Component>();
            foreach (var c in Data.Scripts)
                scripts.Add(c);

            int count = Data.Objects.Count;
            if (GUI.Button(new Rect(140, 60 + count * 70, 20, 20), "+", QD_DialogueEditor.skin.button) && Data.Variables.Count < 6)
            {
                EditorUtility.SetDirty(QD_DialogueEditor.db);
                EditorUtility.SetDirty(QD_DialogueEditor.db.DataDB);
                node.Window.height += 70;
                /*Data.Variables.Add(null);
                variables.Add(null);*/
                Data.Objects.Add(null);
                objects.Add(null);
                Data.Scripts.Add(null);
                scripts.Add(null);
                QD_DialogueEditor.db.DataDB.SetVariable(Data.ID, Data);
            }

            for (int i = count - 1; i >= 0; --i)
            {
                EditorGUI.LabelField(new Rect(5, 40 + i * 70, 65, 40), "Variable " + (i + 1), QD_DialogueEditor.skin.label);
                objects[i] = EditorGUI.ObjectField(new Rect(75, 40 + i * 70, 95, 20), objects[i], typeof(GameObject));
                

                if (objects[i] != null)
                {
                    var scs = (objects[i] as GameObject).GetComponents<Component>();
                    string[] names = scs.Select(x => x.GetType().Name).ToArray();
                    int index = scripts[i] == null
                        ? 0
                        : ArrayUtility.IndexOf(names, scripts[i].GetType().Name);
                    scripts[i] = scs[EditorGUI.Popup(new Rect(175, 40 + i * 70, 95, 20), index, names)];
                }
                
                /*if (scripts[i] != null)
                {
                    var fields = scripts[i].GetType().GetFields();
                    if (fields.Length > 0)
                    {
                        string[] names = fields.Select(x => x.Name).ToArray();

                        int index = variables[i] == null
                            ? 0
                            : ArrayUtility.IndexOf(names, variables[i].Name);
                        variables[i] = fields[EditorGUI.Popup(new Rect(75, 40 + (i + 1) * 30, 195, 20), index, names)];
                    }
                }*/

                if (i == count - 1)
                {
                    if (GUI.Button(new Rect(275, 42.5f + i * 70, 20, 20), "-", QD_DialogueEditor.skin.button))
                    {
                        EditorUtility.SetDirty(QD_DialogueEditor.db);
                        EditorUtility.SetDirty(QD_DialogueEditor.db.DataDB);
                        //variables.RemoveAt(i);
                        objects.RemoveAt(i);
                        scripts.RemoveAt(i);
                        QD_DialogueEditor.editor.selectedNode = node;
                        node.Window.height -= 70;
                        QD_DialogueEditor.db.DataDB.SetVariable(Data.ID, Data);
                    }
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(QD_DialogueEditor.db);
                EditorUtility.SetDirty(QD_DialogueEditor.db.DataDB);
              //  Data.Variables = variables;
                Data.Objects = objects;
                Data.Scripts = scripts;
                QD_DialogueEditor.db.DataDB.SetVariable(Data.ID, Data);
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
            QD_DialogueEditor.db.DataDB.SetVariable(Data.ID, Data);
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
            QD_DialogueEditor.db.DataDB.SetVariable(Data.ID, Data);
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
            QD_DialogueEditor.db.DataDB.SetVariable(Data.ID, Data);
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
            QD_DialogueEditor.db.DataDB.SetVariable(Data.ID, Data);
        }
    }
}