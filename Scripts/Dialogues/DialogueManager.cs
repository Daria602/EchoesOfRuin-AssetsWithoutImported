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

    private GameObject currentNPC;
    public GameObject player;

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

    public void EnterDialogueMode(TextAsset inkJSON, GameObject npc)
    {
        // Stop the movement for both NPC and Player
        currentNPC = npc;
        currentNPC.GetComponent<NPCMovement>().IsAllowedToMove = false;
        currentNPC.GetComponent<Animator>().SetBool("isWalking", false);
        player.GetComponent<PlayerMovement>().IsAllowedToMove = false;
        currentNPC.transform.LookAt((player.transform.position + currentNPC.transform.position) / 2);
        player.transform.LookAt((player.transform.position + currentNPC.transform.position) / 2);

        currentStory = new Story(inkJSON.text);
        currentStory.variablesState["charisma"] = player.GetComponent<Stats>().abilities.charisma;
        currentStory.variablesState["npcName"] = currentNPC.name;
        Debug.Log("Charisma value is " + currentStory.variablesState["charisma"]);
        dialogueIsActive = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
        currentStory.ObserveVariable("choseToFight", (variableName, newValue) =>
        {
            // Print the new value
            ExitDialogueMode();
            player.GetComponent<PlayerController>().StartFight();
        });

        currentStory.ObserveVariable("choseToTrade", (variableName, newValue) =>
        {
            // Print the new value
            ExitDialogueMode();
            TradeManager.GetInstance().TriggerTrade(currentNPC.GetComponent<Inventory>());
        });
        currentStory.ObserveVariable("choseTheQuest", (variableName, newValue) =>
        {
            ExitDialogueMode();
            Debug.Log(newValue);
            GiveTheQuest();
            ExitTrade();
            //TradeManager.GetInstance().TriggerTrade(currentNPC.GetComponent<Inventory>());
        });
    }

    public void GiveTheQuest()
    {
        if (QuestManager.GetInstance().AddQuest(currentNPC.GetComponent<DialogueTrigger>().questId))
        {
            Constants.GetInstance().questMap[currentNPC.GetComponent<DialogueTrigger>().questId].isActive = true;
            UpdateJournal();
        }
        
    }

    public void UpdateJournal()
    {
        QuestManager.GetInstance().UpdateJournal();
    }

    private void ExitDialogueMode()
    {
        
        
        dialogueIsActive = false;
        dialoguePanel.SetActive(false);

        dialogueText.text = "";
    }

    public void ExitTrade()
    {
        currentNPC.GetComponent<NPCMovement>().IsAllowedToMove = true;
        currentNPC.GetComponent<Animator>().SetBool("isRunning", true);
        player.GetComponent<PlayerMovement>().IsAllowedToMove = true;
        currentNPC = null;
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            //Debug.Log("Story can continue");
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
        }
        else
        {
            //Debug.Log("Story cannot continue");
            ExitDialogueMode();
            currentNPC.GetComponent<NPCMovement>().IsAllowedToMove = true;
            //currentNPC.GetComponent<Animator>().SetBool("isWalking", true);
            player.GetComponent<PlayerMovement>().IsAllowedToMove = true;
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
