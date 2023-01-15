using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IngredientGenerator))]
public class IngredientEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Delete Generated"))
            IngredientGenerator.DeleteDefault();
        else if (GUILayout.Button("Open Generator Window"))
            ContentGeneratorEditorWindow.ShowWindow();
    }
}

[CustomEditor(typeof(AlchemyGeneratorManager))]
public class AlchemyGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Update Ingredients"))
            AlchemyGeneratorManager.UpdateIngredientsList();
    }
}
