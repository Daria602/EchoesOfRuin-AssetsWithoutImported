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
    [SerializeField] private GameObject choicesUIPanel;
    [SerializeField] private GameObject continueButton;
    public GameObject scrollRectInput;
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

        Button button = continueButton.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);

        
    }

    private void Update()
    {
        if (!dialogueIsActive)
        {
            return;
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON, Inventory inventory = null)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsActive = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
        currentStory.ObserveVariable("choseToFight", (variableName, newValue) =>
        {
            // Print the new value
            ExitDialogueMode();
            FindObjectOfType<PlayerController>().StartFight();
        });

        currentStory.ObserveVariable("choseToTrade", (variableName, newValue) =>
        {
            // Print the new value
            ExitDialogueMode();
            TradeManager.GetInstance().TriggerTrade(inventory);
        });
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
            Debug.Log("Story can continue");
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
        }
        else
        {
            Debug.Log("Story cannot continue");
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

        if (currentChoices.Count > 0)
        {
            continueButton.SetActive(false);
            choicesUIPanel.SetActive(true);
            for (int i = index; i < choicesUI.Length; i++)
            {
                choicesUI[i].gameObject.SetActive(false);
            }
        }
        else 
        {
            choicesUIPanel.SetActive(false);
            continueButton.SetActive(true);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        Debug.Log("Choice index is " + choiceIndex);
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
        scrollRectInput.GetComponent<ScrollRect>().verticalNormalizedPosition = 0.95f;
        
    }

    void TaskOnClick()
    {
        ContinueStory();
        scrollRectInput.GetComponent<ScrollRect>().verticalNormalizedPosition = 0.95f;
    }
}
