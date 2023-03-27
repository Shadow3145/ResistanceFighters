using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private List<Dialogue> dialogues;
    private int currentDialogue = 0;
    
    public void OpenDialogue()
    {
        DialogueManager.instance.gameObject.SetActive(true);
        DialogueManager.instance.OpenDialogue(dialogues[currentDialogue]);
        
        currentDialogue = Random.Range(1, dialogues.Count);
    }
}
