using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : CombatController
{
    bool isPreparingToAffect = false;
    private Interactable interactable;
    Vector3 pointInScene;
    private void Update()
    {
        if (IsInCombat)
        {
            Debug.Log("Player is in combat");
            
            if (isPreparingToAffect)
            {
                // do the animation and circle around
                WaitForClick();
                Debug.Log("Waiting for a click");
            }
            else if (isPerformingAction)
            {
                //Debug.Log(skills[actionIndex].name);
                Debug.Log("Performing the action");
                doAction();

            }
            
        }
    }
    private void WaitForClick()
    {
        if (LeftMouseClicked())
        {

            PlayerController.ClickType clickType = gameObject.GetComponent<PlayerController>().GetClickType(out pointInScene, out interactable);
            if (skills[actionIndex].name == "Move")
            {
                // Move if possible
                float distance;
                if (gameObject.GetComponent<PlayerMovement>().GetDistance(pointInScene, out distance))
                {
                    Debug.Log("Distance: " + distance.ToString() + "; Cost: " + Mathf.FloorToInt(distance / Constants.DISTANCE_COST_UNIT).ToString());
                    if (distance > gameObject.GetComponent<PlayerMovement>().maximumTravelDistance)
                    {
                        CancelAction();
                        return;
                    }

                }
                CancelPrepare();
                gameObject.GetComponent<PlayerMovement>().IsAllowedToMove = true;
                gameObject.GetComponent<PlayerMovement>().MovePlayer(pointInScene);
                isPreparingToAffect = false;
                isPerformingAction = true;
                doAction = Move;

            }
            else
            {
                if (clickType == PlayerController.ClickType.Interact)
                {
                    // TODO: check if Interractable is enemy (and not a potion bottle, for example)

                    // check for distance, attack and stop performing

                    // Is in distance
                    if (Vector3.Distance(transform.position, pointInScene) <= skills[actionIndex].minDistance)
                    {
                        CancelPrepare();
                        isPreparingToAffect = false;
                        isPerformingAction = true;
                        doAction = Attack;
                    }
                    else
                    {
                        CancelAction();
                    }
                }
                else
                {
                    isPerformingAction = false;
                }
            }
        }
        else if (RightMouseClicked())
        {
            CancelAction();
        }
    }

    private bool LeftMouseClicked()
    {
        return Input.GetMouseButtonDown(0);
    }

    private bool RightMouseClicked()
    {
        return Input.GetMouseButtonDown(1);
    }

    private void CancelPrepare()
    {
        gameObject.GetComponent<Animator>().SetBool(skills[actionIndex].prepareAnimationBoolName, false);
        gameObject.GetComponent<Animator>().SetBool(skills[actionIndex].affectAnimationBoolName, true);
    }

    private void CancelAction()
    {
        isPreparingToAffect = false;
        isPerformingAction = false;
        finishedPerforming = true;
        gameObject.GetComponent<Animator>().SetBool(skills[actionIndex].prepareAnimationBoolName, false);
        gameObject.GetComponent<Animator>().SetBool(skills[actionIndex].affectAnimationBoolName, false);
        skills[actionIndex].RemoveEffect();
    }

    private void PerformAction(int skillIndex)
    {
        isPerformingAction = false;
        isPreparingToAffect = true;
        finishedPerforming = false;
        actionIndex = skillIndex;
        // Sets the visual effect around the character
        skills[skillIndex].SetEffect(gameObject);
        GetComponent<Animator>().SetBool(skills[skillIndex].prepareAnimationBoolName, true);
        Debug.Log("Performing skill " + skillIndex);
        Debug.Log("Animation is set to " + GetComponent<Animator>().GetBool(skills[skillIndex].prepareAnimationBoolName));
    }

    public void ButtonIndexClicked(int buttonIndex)
    {
        // TODO: create something to check what actions are displayed
        // For example, character can have 11 skills
        // But the bar can display only 10
        // If the first set of 10 is displayed and the character clicks on button_0
        // actionIndex = 0, 
        // else actionIndex should recalculate
        int skillIndex = 0;
        PerformAction(skillIndex);
    }

}