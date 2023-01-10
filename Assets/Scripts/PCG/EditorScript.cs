using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProceduralGenerationManager))]
public class EditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Delete Generated"))
            ProceduralGenerationManager.DeleteDefault();
        else if (GUILayout.Button("Open Generator Window"))
            ContentGeneratorEditorWindow.ShowWindow();
    }
}
