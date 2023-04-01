#if UNITY_EDITOR
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
            AlchemyContentGeneratorWindowEditor.ShowWindow(0);
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
        if (GUILayout.Button("Open Editor"))
            AlchemyContentGeneratorWindowEditor.ShowWindow(-1);
    }
}

[CustomEditor(typeof(PotionGenerator))]
public class PotionGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Generate Recipes"))
            FindObjectOfType<PotionGenerator>().GeneratePotionRecipes();
        if (GUILayout.Button("Delete Generated Recipes"))
            FindObjectOfType<PotionGenerator>().Delete();
    }
}
#endif
