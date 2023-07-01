using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController, ILoadingData
{

    private PlayerMovement movement;
    private PlayerCombat combat;
    public Interactable currentFocus;
    private int XP = 0;
    public int gold = 0;
    public int currentThreshold = 1000;
    //public Transform leftHand;
    //public Transform rightHand;

    private void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        combat = GetComponent<PlayerCombat>();
        stats = GetComponent<Stats>();
        UIManager.GetInstance().UpdateXPSlider(XP, 0, currentThreshold);
        UIManager.GetInstance().UpdateHealthSlider(GetComponent<PlayerHealth>().currentHealth, 0, GetComponent<PlayerHealth>().CurrentMaxHealth);
        UIManager.GetInstance().UpdateXPText("Level " + stats.characterLevel.ToString());
        UIManager.GetInstance().UpdateHealthText(GetComponent<PlayerHealth>().currentHealth.ToString());
    }

    
    

    public void CalculateNewThreshold()
    {
        currentThreshold = 500 * ((stats.characterLevel + 1) * (stats.characterLevel+1)) - (500 * (stats.characterLevel+1));
    }

    public bool PassedCurrentThreshold()
    {
        return XP >= currentThreshold;
    }

    public void AddXP(int XPValue)
    {
        
        XP += XPValue;
        

        if (PassedCurrentThreshold())
        {
            stats.characterLevel++;
            stats.availableAttributePoints += 3;
            stats.availableAbilityPoints += 3;
            CalculateNewThreshold();
            UIManager.GetInstance().UpdateXPSlider(XP - getPreviousThreshold(), 0, currentThreshold);
            UIManager.GetInstance().ShowXPRecieved(XPValue, true);
        }
        else
        {
            UIManager.GetInstance().ShowXPRecieved(XPValue, false);
        }
        UIManager.GetInstance().UpdateXPSlider(XP - getPreviousThreshold());
        UIManager.GetInstance().UpdateXPText("Level " + stats.characterLevel.ToString());
    }

    private int getPreviousThreshold() 
    {
        return 500 * ((stats.characterLevel) * (stats.characterLevel)) - (500 * (stats.characterLevel));
    }

    void Update()
    {
        //HealthSlider.value = GetComponent<PlayerHealth>().currentHealth;
        //healthText.text = GetComponent<PlayerHealth>().currentHealth.ToString();
        if (!combat.IsInCombat)
        {
           
            if (InputManager.GetInstance().LeftMouseClicked())
            {
                Interactable interactable;
                Vector3 point;
                Constants.ClickType clickType = InputManager.GetInstance().GetClickType(out point, out interactable);
                //Debug.Log(point);
                MakeClickDecision(clickType, point, interactable);
                
            }
            if (movement.HasArrived())
            {
                animator.SetBool("isRunning", false);
            }
        }
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    StartFight();
        //}
        
    }
    private void MakeClickDecision(Constants.ClickType clickType, Vector3 point, Interactable interactable)
    {
        switch (clickType)
        {
            case Constants.ClickType.UI:
                RemoveFocus();
                break;
            case Constants.ClickType.Move:
                RemoveFocus();
                movement.MovePlayer(point);
                animator.SetBool("isRunning", true);
                break;
            case Constants.ClickType.Interact:
                animator.SetBool("isRunning", true);
                movement.MovePlayer(point);
                RemoveFocus();
                SetFocus(interactable);
                break;
        }
    }

    

    public void RemoveFocus(Interactable interactable = null)
    {
        if (currentFocus != null && currentFocus != interactable)
        {
            currentFocus.IsFocus = false;
        }
    }

    public void StartFight()
    {
        
        // To toggle combat
        // if player is not in combat, running, but the combat is triggered mid run,
        // reset the animation
        if (!combat.IsInCombat)
        {
            animator.SetBool("isRunning", false);

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f);
            List<int> participants = new List<int>();
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.tag == "Attackable")
                {
                    participants.Add(hitCollider.gameObject.GetComponent<CharacterController>().characterId);
                }
            }
            if (participants.Count > 0)
            {
                
                // Player's Id is 0
                participants.Add(0);
                InitiateCombat(participants);
            }

        }
    }

    private void InitiateCombat(List<int> participantsIds)
    {
        Debug.Log("Got to Initiate combat in PlayerControiller");
        CombatManager.GetInstance().StartCombat(participantsIds);
        

    }

    void SetFocus(Interactable newFocus)
    {
        if (newFocus != currentFocus)
        {
            if (currentFocus != null)
            {
                currentFocus.OnDefocused();
            }
            currentFocus = newFocus;
        }
        newFocus.OnFocused(transform);
    }

    //void RemoveFocus()
    //{
    //    if (currentFocus != null)
    //    {
    //        currentFocus.OnDefocused();
    //    }
    //    currentFocus = null;
    //}

    public int GoldDropped()
    {
        int luck = stats.abilities.luck;

        return 3 + luck * 2;
    }


    public void LoadGameData(CharacterData characterData)
    {
        this.GetComponent<PlayerCombat>().LoadGameData(characterData);
        this.GetComponent<Stats>().LoadGameData(characterData);
        this.GetComponent<PlayerHealth>().LoadGameData(characterData);
        this.transform.position = characterData.characterPosition;
        this.gold = characterData.gold;
    }
    public void SaveGameData(ref CharacterData characterData)
    {
        this.GetComponent<PlayerCombat>().SaveGameData(ref characterData);
        this.GetComponent<Stats>().SaveGameData(ref characterData);
        this.GetComponent<PlayerHealth>().SaveGameData(ref characterData);
        characterData.characterPosition = this.transform.position;
        characterData.gold = this.gold; 
    }


}
