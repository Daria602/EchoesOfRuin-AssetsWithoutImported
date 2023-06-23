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
        //ActionPoint[] ap = actionPointsUI.GetComponentsInChildren<ActionPoint>();
        //for (int i = 0; i < ap.Length; i++)
        //{
        //    ap[i].ToggleActive(i < actionPointsLeft);
        //}
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

    private void ClearPortraits()
    {
        //Portrait[] portraits = portraitsGrid.GetComponentsInChildren<Portrait>();
        //if (portraits != null && portraits.Length > 0)
        //{
        //    foreach (Portrait portrait in portraits)
        //    {
        //        portrait.SelfDestroy();
        //    }
        //}
    }

    public void PlayerEndsTurn()
    {
        CombatManager.GetInstance().PlayerEndsTurn();
    }
}
