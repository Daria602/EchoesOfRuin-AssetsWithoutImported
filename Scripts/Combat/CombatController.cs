using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : MonoBehaviour
{
    public int characterId;
    public bool m_isInCombat = false;
    public int[] skillsIds = null;
    public bool hasWeapon = true;
    public Weapon weapon;
    public Transform rightHand;

    public bool isPerformingAction = false;
    public bool finishedPerforming = true;
    public int actionIndex = 0;
    public bool charactersTurn = false;
    [HideInInspector()]
    public List<Skill> skills = new List<Skill>();

    

    protected delegate void DoAction();
    protected DoAction doAction;

    public bool endedTurn = true;





    int actionPointsLeft = 3;

    public bool IsInCombat 
    { 
        get => m_isInCombat; 
        set => CombatStateIsChanged(value); 
    }

    private void Awake()
    {
        for (int i = 0; i < skillsIds.Length; i++)
        {
            var skillInstantiated = ScriptableObject.Instantiate(Constants.GetInstance().skillMap[skillsIds[i]]);
            skills.Add(skillInstantiated);
        }
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
            Debug.Log("Got into Attack");
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

    protected float CalculateAffectedDamage()
    {
        float baseDamage = 0f;
        if (skills[actionIndex].requiresWeapon)
        {
            // TODO: calculate base on weapon
        }
        else
        {
            baseDamage = Random.Range((int)skills[actionIndex].baseDamageMin, (int)skills[actionIndex].baseDamageMax);
        }

        // TODO: apply stats here
        Debug.Log("Base damage was: " + baseDamage);
        baseDamage = ApplyAttributes(baseDamage);
        Debug.Log("Damage now is: " + baseDamage);
        //ApplyAbilities(out baseDamage);


        return Mathf.Round(baseDamage);
    }

    private float ApplyAttributes(float baseDamage)
    {
        List<float> additions = new List<float>();
        for (int i = 0; i < skills[actionIndex].affectedByAttributes.Length; i++)
        {
            switch (skills[actionIndex].affectedByAttributes[i])
            {
                case Constants.AffectedByAttributes.Strength:
                    break;
                case Constants.AffectedByAttributes.Agility:
                    break;
                case Constants.AffectedByAttributes.Intelligence:
                    additions.Add(
                        baseDamage * gameObject.GetComponent<Stats>().intelligence * 0.05f
                        );
                    break;
                case Constants.AffectedByAttributes.Constitution:
                    break;
                case Constants.AffectedByAttributes.Wits:
                    break;
            }
        }

        foreach (float addition in additions)
        {
            baseDamage += addition;
        }

        return Mathf.Round(baseDamage);
    }

    //private void ApplyAbilities(out float damage)
    //{

    //}


}
