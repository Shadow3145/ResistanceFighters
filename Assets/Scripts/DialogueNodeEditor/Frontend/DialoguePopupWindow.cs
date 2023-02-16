using UnityEditor;
using UnityEngine;
using System.Linq;
using System;

public class DialoguePopupWindow : PopupWindowContent
{
    public string newDialogueName = "";
    private Action<DialoguePopupWindow> OnClosePopup;
    public override Vector2 GetWindowSize()
    {
        return new Vector2(150, 120);
    }

    public DialoguePopupWindow(Action<DialoguePopupWindow> OnClose)
    {
        this.OnClosePopup = OnClose;
    }


    public override void OnGUI(Rect rect)
    {
        GUI.Label(new Rect(10, 25, 130, 20), "Create new dialogue");


        if (GUI.Button(new Rect(130, 0, 20, 20), "X"))
            PopupWindow.mouseOverWindow.Close();

        newDialogueName = EditorGUI.TextField(new Rect(25, 50, 100, 20), newDialogueName);

        if (newDialogueName != "" && !Resources.FindObjectsOfTypeAll<Dialogue>().Select(x => x.name).Contains(newDialogueName))
            if (GUI.Button(new Rect(25, 80, 100, 30), "Create"))
            {
                Dialogue dialogue = ScriptableObject.CreateInstance<Dialogue>();
                dialogue.lastConnectionId = 0;
                dialogue.lastNodeId = 1;
                AssetDatabase.CreateAsset(dialogue, "Assets/Scripts/DialogueNodeEditor/Database/" + newDialogueName + ".asset");
                OnClosePopup(this);
                PopupWindow.mouseOverWindow.Close();
            }
            

    }
}
