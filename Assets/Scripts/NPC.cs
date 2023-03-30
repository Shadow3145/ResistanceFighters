using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private List<Dialogue> dialogues;
    [SerializeField] private DialogueManager manager;
    private int currentDialogue = 0;
    
    public void OpenDialogue()
    {
        manager.gameObject.SetActive(true);
        manager.OpenDialogue(dialogues[currentDialogue]);

        currentDialogue = Mathf.Min(currentDialogue+1, dialogues.Count - 1);
    }
}
