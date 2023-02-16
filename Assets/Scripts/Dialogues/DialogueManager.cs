using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;

    [SerializeField] private GameObject speakerIcon;
    [SerializeField] private GameObject dialogueText;

    [SerializeField] private GameObject speakerName;

    [SerializeField] private GameObject continueButton;

    private int currentId;
    // Start is called before the first frame update
    void Start()
    {
        speakerIcon.SetActive(true);
        speakerName.SetActive(true);
        dialogueText.SetActive(true);

        if (dialogue == null)
        {
            Debug.Log("No dialogue");
            Close();
            return;   
        }
        currentId = dialogue.GetFirstNode();
        SetData(currentId);
    }

    public void SetData(int nodeId)
    {
        if (nodeId == -1)
        {
            Close();
            return;
        }

        speakerIcon.GetComponent<Image>().sprite = dialogue.GetSpeakerIcon(nodeId);
        speakerName.GetComponent<TextMeshProUGUI>().text = dialogue.GetSpeakerName(nodeId);

        dialogueText.GetComponent<TextMeshProUGUI>().text = dialogue.GetDialogueText(nodeId);
    }

    public void Continue()
    {
        currentId = dialogue.GetNextNode(currentId);
        SetData(currentId);
    }

    public void Close()
    {
        speakerIcon.SetActive(false);
        speakerName.SetActive(false);
        dialogueText.GetComponent<TextMeshProUGUI>().text = "Dialogue End";
    }
}
