using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatUI : MonoBehaviour
{
    // singleton package
    private static CombatUI instance;
    public static CombatUI GetInstance() { return instance; }
    private void Awake()
    {
        if (instance != null)
        {

        }
        instance = this;
    }

    //public GameObject actionPointsUI;
    //public GameObject enemyTurnUI;
    //public GameObject endTurnButton;
    //public GameObject portraitsGrid;
    //public GameObject portraitPrefab;
    public GameObject playerCombatUI;
    public GameObject enemyCombatUI;
    public TextMeshProUGUI enemyName;

    public GameObject actionPointsUI;

    public void EnablePlayerUI()
    {
        enemyCombatUI.SetActive(false);
        playerCombatUI.SetActive(true);
        //actionPointsUI.SetActive(true);
        //endTurnButton.SetActive(true);
        //enemyTurnUI.SetActive(false);
    }
    public void EnableEnemyUI(int characterId)
    {
        string characterName = Constants.GetInstance().characters[characterId].name;
        enemyName.text = characterName;
        enemyCombatUI.SetActive(true);
        playerCombatUI.SetActive(false);
        //actionPointsUI.SetActive(false);
        //endTurnButton.SetActive(false);
        //enemyTurnUI.SetActive(true);
    }
    public void ShowEnemySpeech(int characterId)
    {
        Debug.Log("argh");
    }

    public void HideEnemySpeech(int characterId)
    {
        Debug.Log("no argh");
    }

    public void UpdateActionPointsUI(int actionPointsLeft)
    {
        // Debug.Log("Got here");
        ActionPoint[] ap = actionPointsUI.GetComponentsInChildren<ActionPoint>();
        for (int i = 0; i < ap.Length; i++)
        {
            ap[i].ToggleActive(i < actionPointsLeft);
        }
    }

    public void CombatOver()
    {
        //actionPointsUI.SetActive(false);
        //endTurnButton.SetActive(false);
        //enemyTurnUI.SetActive(false);
        //ClearPortraits();
    }

    public void EnableUI(int characterId)
    {
        if (characterId == Constants.PLAYER_ID)
        {
            // enable action point UI
            EnablePlayerUI();
            //UpdateActionPointsUI(3);
        }
        else
        {
            // say that it's enemies turn
            EnableEnemyUI(characterId);
        }
    }

    public void ClearPortraits()
    {
        Portrait[] portraits = portraitsGrid.GetComponentsInChildren<Portrait>();
        if (portraits != null && portraits.Length > 0)
        {
            foreach (Portrait portrait in portraits)
            {
                portrait.SelfDestroy();
            }
        }
    }

    public void PlayerEndsTurn()
    {
        CombatManager.GetInstance().PlayerEndsTurn();
    }

    public void UpdateCooldownsOnSkills(List<Skill> skills)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            SkillPanelController.GetInstance().UpdateCooldown(i, skills[i].cooldown);
        }
    }

    public void UpdatePortraits(List<int> participantsIds)
    {
        foreach (int participantId in participantsIds)
        {

        }
    }
    public GameObject portraitPrefab;
    public GameObject portraitsGrid;
    public void CreatePortraits(List<int> participantsIds)
    {
        foreach (int characterId in participantsIds)
        {
            GameObject go = Instantiate(portraitPrefab, portraitsGrid.GetComponent<RectTransform>());
            Portrait portrait = go.GetComponent<Portrait>();
            portrait.SetImage(GetCharacterPortrait(characterId));
            portrait.SetHealthMinMax(0, GetCharacterMaxHealth(characterId));
            portrait.SetHealthValue(GetCharacterHealth(characterId));
            portrait.SetName(GetCharacterName(characterId));
            if (characterId == 0)
            {
                portrait.SetAsAlly();
            }
            else
            {
                portrait.SetAsEnemy();
            }
        }
    }

    private Sprite GetCharacterPortrait(int characterId)
    {
        return Constants.GetInstance().characters[characterId].GetComponent<CharacterAppearance>().image;
    }

    private int GetCharacterHealth(int characterId)
    {
        return Constants.GetInstance().characters[characterId].GetComponent<Health>().currentHealth;
    }

    private int GetCharacterMaxHealth(int characterId)
    {
        return Constants.GetInstance().characters[characterId].GetComponent<Health>().CurrentMaxHealth;
    }

    private string GetCharacterName(int characterId)
    {
        return Constants.GetInstance().characters[characterId].name == "PlayerInGame" ? "You" : Constants.GetInstance().characters[characterId].name;
    }

    //public void ShowPortraits()
    //{
    //    // clear the previous ones
    //    //ClearPortraits();

    //    if (m_isCombatGoing)
    //    {
    //        // Set new ones
    //        for (int i = 0; i < characterInitiativeList.Count; i++)
    //        {
    //            int currentTurnIndex;
    //            if (i + turnIndex >= characterInitiativeList.Count)
    //            {
    //                currentTurnIndex = characterInitiativeList.Count - (i + turnIndex);
    //            }
    //            else
    //            {
    //                currentTurnIndex = i + turnIndex;
    //            }
    //            int characterId = characterInitiativeList[currentTurnIndex].characterId;
    //            //Sprite characterPortrait = Constants.GetInstance().characters[characterId].GetComponent<CharacterAppearance>().image;
    //            int currentHealth = Constants.GetInstance().characters[characterId].GetComponent<Health>().currentHealth;
    //            int maxHealth = Constants.GetInstance().characters[characterId].GetComponent<Health>().CurrentMaxHealth;
    //            string characterName = Constants.GetInstance().characters[characterId].name == "PlayerInGame" ? "You" : Constants.GetInstance().characters[characterId].name;
    //            GameObject go = Instantiate(portraitPrefab, portraitsGrid.GetComponent<RectTransform>());
    //            Portrait portrait = go.GetComponent<Portrait>();
    //            portrait.SetImage(characterPortrait);
    //            portrait.SetHealthMinMax(0, maxHealth);
    //            portrait.SetHealthValue(currentHealth);
    //            portrait.SetName(characterName);
    //            if (characterId == 0)
    //            {
    //                portrait.SetAsAlly();
    //            }
    //            else
    //            {
    //                portrait.SetAsEnemy();
    //            }
    //        }
    //    }

    //}
}
