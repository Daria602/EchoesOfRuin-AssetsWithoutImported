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

    bool isPreparingToAffect = false;

    private delegate void DoAction();
    DoAction doAction;

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

    private Vector3 pointToMove;
    private Interactable interactable;

    private void Move()
    {
        // Move gets a special treatment
        if (gameObject.GetComponent<PlayerMovement>().IsAllowedToMove)
        {
            if (gameObject.GetComponent<PlayerMovement>().HasArrived())
            {
                gameObject.GetComponent<PlayerMovement>().IsAllowedToMove = false;
                isPerformingAction = false;
                gameObject.GetComponent<Animator>().SetBool(skills[actionIndex].affectAnimationBoolName, false);
                skills[actionIndex].RemoveEffect();

            }

        }
        
    }

    private void Attack()
    {
        if (finishedPerforming)
        {
            finishedPerforming = false;
            // Deal damage here
            isPerformingAction = false;
            gameObject.GetComponent<Animator>().SetBool(skills[actionIndex].affectAnimationBoolName, false);
            skills[actionIndex].RemoveEffect();

        }
    }

    

    private void WaitForClick()
    {
        if (LeftMouseClicked())
        {
            //Vector3 point;
            //Interactable interactable;
            PlayerController.ClickType clickType = gameObject.GetComponent<PlayerController>().GetClickType(out pointToMove, out interactable);
            if (skills[actionIndex].name == "Move")
            {
                // Move if possible
                float distance;
                if (gameObject.GetComponent<PlayerMovement>().GetDistance(pointToMove, out distance))
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
                gameObject.GetComponent<PlayerMovement>().MovePlayer(pointToMove);
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
                    if (Vector3.Distance(transform.position, pointToMove) <= skills[actionIndex].minDistance)
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

    private void FinishedPerforming()
    {
        // Move gets a special treatment
        if (skills[actionIndex].name == "Move" && gameObject.GetComponent<PlayerMovement>().IsAllowedToMove)
        {
            if (gameObject.GetComponent<PlayerMovement>().HasArrived())
            {
                gameObject.GetComponent<PlayerMovement>().IsAllowedToMove = false;
                isPerformingAction = false;
                gameObject.GetComponent<Animator>().SetBool(skills[actionIndex].prepareAnimationBoolName, false);

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


}
