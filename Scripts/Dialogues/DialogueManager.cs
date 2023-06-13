using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject[] choicesUI;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;

    private bool dialogueIsActive;

    private static DialogueManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Too many Dialogue Manager instances. Should be only one");
        }
        instance = this;
    }
    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsActive = false;
        dialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choicesUI.Length];
        int index = 0;
        foreach (GameObject choice in choicesUI)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
        
    }

    private void Update()
    {
        if (!dialogueIsActive)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsActive = true;
        dialoguePanel.SetActive(true);
        ContinueStory();


    }

    private void ExitDialogueMode()
    {
        dialogueIsActive = false;
        dialoguePanel.SetActive(false);

        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        // too many choices
        if (currentChoices.Count > choicesUI.Length)
        {
            Debug.LogError("Too many choices, aaargh!!!");
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choicesUI[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choicesUI.Length; i++)
        {
            choicesUI[i].gameObject.SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }
}
