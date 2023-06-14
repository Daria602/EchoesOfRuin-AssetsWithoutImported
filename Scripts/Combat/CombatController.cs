using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : MonoBehaviour
{
    private bool m_isInCombat = false;
    public bool isPlayer;
    public Skill[] skills = null;
    public Button[] skillButtons = null;

    public bool hasWeapon = true;
    public Weapon weapon;
    public Transform leftHand;
    public Transform rightHand;

    bool isPerformingAction = false;
    public bool finishedPerforming = true;
    int actionIndex = 0;

    public bool IsInCombat 
    { 
        get => m_isInCombat; 
        set => CombatStateIsChanged(value); 
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isInCombat)
        {
            if (isPerformingAction)
            {
                //Debug.Log(skills[actionIndex].name);

                FinishedPerforming();
                
            }
        }
    }

    private void FinishedPerforming()
    {
        // Move gets a special treatment
        if (skills[actionIndex].name == "Move" && gameObject.GetComponent<PlayerMovement>().IsAllowedToMove)
        {
            if (gameObject.GetComponent<PlayerMovement>().HasArrived())
            {
                gameObject.GetComponent<PlayerMovement>().IsAllowedToMove = false;
                isPerformingAction = false;
            }

        } 
        else
        {
            if (LeftMouseClicked())
            {
                Vector3 point;
                Interactable interactable;
                PlayerController.ClickType clickType = gameObject.GetComponent<PlayerController>().GetClickType(out point, out interactable);
                if (skills[actionIndex].name == "Move")
                {
                    // Move if possible
                    float distance;
                    if (gameObject.GetComponent<PlayerMovement>().GetDistance(point, out distance))
                    {
                        Debug.Log("Distance: " + distance.ToString() + "; Cost: " + Mathf.FloorToInt(distance / Constants.DISTANCE_COST_UNIT).ToString());
                        if (distance > gameObject.GetComponent<PlayerMovement>().maximumTravelDistance)
                        {
                            isPerformingAction = false;
                            return;
                        }
                        
                    }
                    gameObject.GetComponent<PlayerMovement>().IsAllowedToMove = true;
                    gameObject.GetComponent<PlayerMovement>().MovePlayer(point);

                }
                else
                {
                    if (clickType == PlayerController.ClickType.Interact)
                    {
                        // check for distance, attack and stop performing
                    }
                    else
                    {
                        isPerformingAction = false;
                    }
                }

            }
            else if (RightMouseClicked())
            {
                isPerformingAction = false;
            }
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

    private void CombatStateIsChanged(bool value)
    {
        if (value && hasWeapon)
        {
            GameObject go = Instantiate(weapon.weaponPrefab, leftHand);
        }
        m_isInCombat = value;
        ResetMovement();
    }

    private void ResetMovement()
    {
        if (isPlayer)
        {
            gameObject.GetComponent<PlayerMovement>().IsAllowedToMove = !m_isInCombat;
        }
        else
        {
            gameObject.GetComponent<NPCMovement>().IsAllowedToMove = !m_isInCombat;
        }
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

    private void PerformAction(int skillIndex)
    {
        isPerformingAction = true;
        finishedPerforming = false;
        actionIndex = skillIndex;
        Debug.Log("Performing skill " + skillIndex);
    }


}
