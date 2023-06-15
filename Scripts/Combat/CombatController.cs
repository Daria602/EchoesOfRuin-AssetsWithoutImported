using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : MonoBehaviour
{
    
    public bool m_isInCombat = false;
    public Skill[] skills = null;
    public bool hasWeapon = true;
    public Weapon weapon;
    public Transform rightHand;

    public bool isPerformingAction = false;
    public bool finishedPerforming = true;
    public int actionIndex = 0;

    

    protected delegate void DoAction();
    protected DoAction doAction;



    int actionPointsLeft = 3;

    public bool IsInCombat 
    { 
        get => m_isInCombat; 
        set => CombatStateIsChanged(value); 
    }

    public void DoneCasting()
    {
        Debug.LogWarning("Hellooooo");
    }

    protected void Move()
    {
        // Move gets a special treatment
        if (gameObject.GetComponent<BaseMovement>().IsAllowedToMove)
        {
            if (gameObject.GetComponent<BaseMovement>().HasArrived())
            {
                gameObject.GetComponent<BaseMovement>().IsAllowedToMove = false;
                isPerformingAction = false;
                gameObject.GetComponent<Animator>().SetBool(skills[actionIndex].affectAnimationBoolName, false);

            }

        }
        
    }

    protected void Attack()
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

    private void CombatStateIsChanged(bool value)
    {
        if (value && hasWeapon)
        {
            GameObject go = Instantiate(weapon.weaponPrefab, rightHand);
        }
        m_isInCombat = value;
        ResetMovement();
    }

    private void ResetMovement()
    {
        gameObject.GetComponent<BaseMovement>().IsAllowedToMove = !m_isInCombat;

    }


}
