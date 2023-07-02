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
    public GameObject portraitPrefab;
    public GameObject portraitsGrid;
    
    public GameObject distanceUI;
    public TextMeshProUGUI distanceNumberText;
    public TextMeshProUGUI moveApCostText;

    public GameObject youDiedMenu;

    public Transform player;

    public void EnablePlayerUI()
    {
        enemyCombatUI.SetActive(false);
        playerCombatUI.SetActive(true);
        //actionPointsUI.SetActive(true);
    }
    public void EnableEnemyUI(int characterId)
    {
        string characterName = Constants.GetInstance().characters[characterId].name;
        enemyName.text = characterName;
        enemyCombatUI.SetActive(true);
        playerCombatUI.SetActive(false);
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

    public void DisableUI()
    {
        enemyCombatUI.SetActive(false);
        playerCombatUI.SetActive(false);
        actionPointsUI.SetActive(false);
        distanceUI.SetActive(false);
        ClearPortraits();
        UpdateCooldownsOnSkills(player.GetComponent<CombatController>().skills);
    }

    public void ClearPortraits()
    {
       // Debug.Log("Got to clear the portraints");
        Portrait[] portraits = portraitsGrid.GetComponentsInChildren<Portrait>();
        
        if (portraits != null && portraits.Length > 0)
        {
            foreach (Portrait portrait in portraits)
            {
                portrait.SelfDestroy();
            }
        }
        // Debug.Log("Portraits length" + portraits.Length);
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

    public void UpdateForImpossibleSkills(List<Skill> skills, int actionPointsLeft)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].cost > actionPointsLeft)
            {
                SkillPanelController.GetInstance().slots[i].interactable = false;
            }
            //SkillPanelController.GetInstance().UpdateCooldown(i, skills[i].cooldown);
        }
    }

    public void UpdatePortraits(List<int> participantsIds)
    {
        foreach (int participantId in participantsIds)
        {

        }
    }
    
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

    public void ShowDistance(float maxDistance, bool showAPCost)
    {
        Vector3? point = InputManager.GetInstance().GetMouseWorldPosition();
        if (point == null)
        {
            Debug.Log("Vector3 point is null");
        }
        else
        {
            if (!distanceUI.activeSelf)
            {
                distanceUI.SetActive(true);
            }
            float distance = Vector3.Distance((Vector3)point, player.position);
            distanceNumberText.text = distance.ToString("F2");
            if (distance <= maxDistance)
            {
                distanceNumberText.color = Color.green;
            }
            else
            {
                distanceNumberText.color = Color.red;
            }
            if (showAPCost)
            {
                // show ap cost
                ShowMoveAPCost(distance);
            }
            else
            {
                // disable ap cost ui
                HideMoveAPCost();
            }
        }
    }

    public void DisableDistance()
    {
        if (distanceUI.activeSelf)
        {
            distanceUI.SetActive(false);
        }
    }

    private void ShowMoveAPCost(float distance)
    {
        if (!moveApCostText.gameObject.activeSelf)
        {
            moveApCostText.gameObject.SetActive(true);
        }
        int cost = (int)(distance / Constants.DISTANCE_COST_UNIT);
        moveApCostText.text = cost + " AP";
        if (cost <= FindObjectOfType<PlayerCombat>().actionPointsLeft)
        {
            moveApCostText.color = Color.green;
            distanceNumberText.color = Color.green;
        }
        else
        {
            moveApCostText.color = Color.red;
            distanceNumberText.color = Color.red;
        }
    }
    private void HideMoveAPCost()
    {
        if (moveApCostText.gameObject.activeSelf)
        {
            moveApCostText.gameObject.SetActive(false);
        }
    }

    
    public void ShowPlayerDeath()
    {
        youDiedMenu.SetActive(true);
    }
}
