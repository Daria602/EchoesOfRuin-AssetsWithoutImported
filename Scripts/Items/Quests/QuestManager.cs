using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour, ILoadingData
{
    private static QuestManager instance;
    public GameObject player;
    public List<int> questIds = new List<int>(); // = [5, 7, 10]
    public List<int> questsSetToRemove = new List<int>(); // [ 7 ]
    public static QuestManager GetInstance()
    {
        return instance;
    }
    


    public GameObject journalPanel;
    public GameObject journalEntries;
    public GameObject questPrefab;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Too many QuestManager instances");
        }
        instance = this;
        journalPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleJournal();
        }

        CheckForQuestCompletion();
    }

    public void ToggleJournal()
    {
        journalPanel.SetActive(!journalPanel.activeSelf);
    }

    public bool AddQuest(int questId)
    {
        if (questIds.Contains(questId))
        {
            return false;
        }
        else
        {
            questIds.Add(questId);
            return true;
        }
        
    }

    public void UpdateJournal()
    {
        QuestUI[] questsInJournal = journalEntries.GetComponentsInChildren<QuestUI>();
        for (int i = 0; i < questsInJournal.Length; i++)
        {
            questsInJournal[i].SelfDestroy();
        }

        for (int i = 0; i < questIds.Count; i++)
        {
            GameObject go = Instantiate(questPrefab, journalEntries.transform);
            go.GetComponent<QuestUI>().SetTooltip(Constants.GetInstance().questMap[questIds[i]].questTitle, Constants.GetInstance().questMap[questIds[i]].questDescription);
            go.GetComponent<QuestUI>().SetText(Constants.GetInstance().questMap[questIds[i]].questTitle);
        }
    }

    public void CheckForQuestCompletion()
    {
        for (int i = 0; i < questIds.Count; i++)
        {
            int currentQuestId = questIds[i];
            Quest quest = Constants.GetInstance().questMap[currentQuestId];
            bool isCompleted = false;

            switch (quest.questType)
            {
                case Constants.QuestType.Kill:
                    isCompleted = CheckIfKilled(quest);
                    break;
                case Constants.QuestType.Talk:
                    isCompleted = CheckIfTalked(quest);
                    break;
                case Constants.QuestType.Explore:
                    CheckIfExplored();
                    break;
            }
            if (isCompleted)
            {
                questsSetToRemove.Add(currentQuestId);
                //Debug.Log("CompletedQuest");
            }
        }

        if (questsSetToRemove.Count > 0)
        {
            // remove the quests and update the journal
            for (int i = 0; i < questsSetToRemove.Count; i++)
            {
                int questIndex = -1;
                for (int j = 0; j < questIds.Count; j++)
                {
                    if (questsSetToRemove[i] == questIds[j])
                    {
                        questIndex = j;
                    }
                }
                // Set quest as inactive
                Constants.GetInstance().questMap[questIds[questIndex]].isActive = false;

                // grant XP
                FindObjectOfType<PlayerController>().AddXP(Constants.GetInstance().questMap[questIds[questIndex]].XPGranted);
                //Debug.Log("XP granted" + Constants.GetInstance().questMap[questIds[questIndex]].XPGranted);
                // remove the quest
                questIds.RemoveAt(questIndex);
            }
            questsSetToRemove.Clear();
            UpdateJournal();
        }
    }

    public bool CheckIfKilled(Quest quest)
    {
        var enemyIds = quest.enemyIds;
        foreach (var enemyId in enemyIds)
        {
            if (Constants.GetInstance().characters[enemyId].activeSelf)
            {
                return false;
            }
        }
        return true;
    }

    public bool CheckIfTalked(Quest quest)
    {
        GameObject personToTalkTo = Constants.GetInstance().characters[quest.idNPC];
        if (personToTalkTo.GetComponent<DialogueTrigger>().wasTalkedTo)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CheckIfExplored()
    {

    }

    public void LoadGameData(CharacterData characterData)
    {
        this.questIds = characterData.questIds;
        foreach (int questId in this.questIds)
        {
            AddQuest(questId);
        }
    }

    public void SaveGameData(ref CharacterData characterData)
    {
        characterData.questIds = this.questIds;
    }
}
