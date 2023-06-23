using System.Collections;
using System.Collections.Generic;
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

    public GameObject actionPointsUI;
    public GameObject enemyTurnUI;
    public GameObject endTurnButton;
    public GameObject portraitsGrid;
    public GameObject portraitPrefab;

    public void EnableActionPointsUI()
    {
        actionPointsUI.SetActive(true);
        endTurnButton.SetActive(true);
        enemyTurnUI.SetActive(false);
    }
    public void EnableEnemyTurnUI()
    {
        actionPointsUI.SetActive(false);
        endTurnButton.SetActive(false);
        enemyTurnUI.SetActive(true);
    }

    public void UpdateActionPointsUI(int actionPointsLeft)
    {
        ActionPoint[] ap = actionPointsUI.GetComponentsInChildren<ActionPoint>();
        for (int i = 0; i < ap.Length; i++)
        {
            ap[i].ToggleActive(i < actionPointsLeft);
        }
    }

    public void CombatOver()
    {
        actionPointsUI.SetActive(false);
        endTurnButton.SetActive(false);
        enemyTurnUI.SetActive(false);
        ClearPortraits();
    }

    public void EnableUI()
    {
        //if (characterInitiativeList[0].characterId == Constants.PLAYER_ID)
        //{
        //    // enable action point UI
        //    EnableActionPointsUI();
        //    UpdateActionPointsUI(3);
        //}
        //else
        //{
        //    // say that it's enemies turn
        //    EnableEnemyTurnUI();
        //}
    }

    private void ClearPortraits()
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
}
