using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    private Dialogue dialogue;

    [SerializeField] private GameObject speakerIcon;
    [SerializeField] private GameObject dialogueText;
    [SerializeField] private ChoiceUIManager choices;

    [SerializeField] private GameObject speakerName;

    [SerializeField] private GameObject continueButton;

    private int currentId;
    private NodeType currentType;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void OpenDialogue(Dialogue dialogue)
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
        this.dialogue = dialogue;
        (currentId, currentType) = dialogue.GetFirstNode();
        SetData(currentId, currentType);
    }

    public void SetData(int nodeId, NodeType type)
    {
        if (nodeId == -1 || !(type == NodeType.ChoiceNode || type == NodeType.DialogueNode))
        {
            Close();
            return;
        }    

        int speaker = dialogue.GetSpeaker(nodeId);
        SetSpeakerData(speaker);
        if (type == NodeType.DialogueNode)
        {
            SetDialogueData(nodeId);
            return;
        }

        if (type == NodeType.ChoiceNode)
        {
            SetChoiceData(nodeId);
        }        
    }

    private void SetSpeakerData(int nodeId)
    {
        speakerIcon.GetComponent<Image>().sprite = dialogue.GetSpeakerIcon(nodeId);
        speakerName.GetComponent<TextMeshProUGUI>().text = dialogue.GetSpeakerName(nodeId);
    }

    private void SetDialogueData(int nodeId)
    {
        continueButton.SetActive(true);
        dialogueText.SetActive(true);
        choices.SetInactive();
        dialogueText.GetComponent<TextMeshProUGUI>().text = dialogue.GetDialogueText(nodeId);
    }

    private void SetChoiceData(int nodeId)
    {
        continueButton.SetActive(false);
        dialogueText.SetActive(false);
        choices.enabled = true;
        List<string> choiceOptions = dialogue.GetChoices(nodeId);
        choices.SetChoices(choiceOptions);
    }

    public void Continue(int choiceIndex=-1)
    {
        (currentId, currentType) = dialogue.GetNextNode(currentId, choiceIndex);
        SetData(currentId, currentType);
    }

    public void Close()
    {
        speakerIcon.SetActive(false);
        speakerName.SetActive(false);
        choices.enabled = false;
        dialogueText.SetActive(true);
        dialogueText.GetComponent<TextMeshProUGUI>().text = "Dialogue End";
    }
}
