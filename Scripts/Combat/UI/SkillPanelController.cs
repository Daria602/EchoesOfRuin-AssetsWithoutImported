using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SkillPanelController : MonoBehaviour
{
    public GameObject skillPanel;
    public List<Skill> skills;
    public Button[] slots;
    public Sprite defaultIcon;
    public bool isInCombat = false;
    public bool setInCombat = false;
    public bool skillPanelVisibility = false;
    public PlayerCombat pc;
    private bool afterStart = true;
    public SimpleTooltip[] tooltips;


    public GameObject learnSkillPanel;
    public TextMeshProUGUI skillName;
    private int skillToLearnId = -1;
    public Item skillToLearnInventory = null;
    public GameObject player;

    public static SkillPanelController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static SkillPanelController GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        tooltips = skillPanel.GetComponentsInChildren<SimpleTooltip>();
        learnSkillPanel.SetActive(false);
        MakeSkillsUnclickable();
    }

    private void Update()
    {
        //if (afterStart)
        //{
            
            skills = pc.skills;
            //Debug.Log("Skill panel thing " + skills.Count);
            AssignSkillsToButtons();
            AssignTooltips();
           // afterStart = false;
        //}
        
        if (isInCombat)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        //if (isInCombat != setInCombat)
        //{
        //    skillPanelVisibility = !skillPanelVisibility;
        //    skillPanel.SetActive(skillPanelVisibility);
        //    isInCombat = setInCombat;
        //}
        
    }

    public void DisableSkillPanel()
    {
        skillPanel.SetActive(false);
    }

    public void AssignTooltips()
    {
        if (tooltips.Length > 0)
        {
            if (skills.Count > 0)
            {
                int index = 0;
                foreach (Button slot in slots)
                {
                    if (index < skills.Count)
                    { 
                        string finalString = skills[index].GetTooltip();
                        tooltips[index].infoLeft = finalString;
                    }
                    index++;
                    
                }
            }
        }
    }
    public void AssignSkillsToButtons()
    {
        //Debug.Log("Got to asign skills and the length is " + skills.Count);
        if (skills.Count == 0)
        {
            Debug.LogError("Object " + transform.name + " doesn't have any skills assigned");
            return;
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < skills.Count)
            {
                // set skill icon
                slots[i].GetComponent<Image>().sprite = skills[i].skillIcon;

            } 
            else
            {
                // set default icon
                slots[i].GetComponent<Image>().sprite = defaultIcon;
            }
        }
    }

    public void SetSkillIcon(ref Image imageUI, Skill skill)
    {
        //Debug.Log(imageUI.name);
        imageUI.sprite = skill.skillIcon;
    }

    public void SetButtonActive(int buttonIndex)
    {
        if (skills[buttonIndex].requiresWeapon)
        {
            if (player.GetComponent<CombatController>().hasWeapon)
            {
                if (player.GetComponent<CombatController>().GetWeapon().weaponType == skills[buttonIndex].weaponTypeRequired)
                {
                    slots[buttonIndex].interactable = true;
                }
            }
        }
        else if (!skills[buttonIndex].requiresWeapon) 
        {
            slots[buttonIndex].interactable = true;
        }
        
        slots[buttonIndex].GetComponentInChildren<TextMeshProUGUI>().text = "";

    }

    public void SetButtonInactive(int buttonIndex, int cooldown)
    {
        slots[buttonIndex].interactable = false;
        slots[buttonIndex].GetComponentInChildren<TextMeshProUGUI>().text = 
            (cooldown == 0) ? "" : cooldown.ToString();
    }


    //public void SetButtonInactive(bool activity, int buttonIndex, int cooldown)
    //{
    //    slots[buttonIndex].interactable = activity;
    //    //Debug.Log("Should be: " + activity + ", is actually: " + slots[buttonIndex].interactable);
    //    if (cooldown == 0)
    //    {
    //        slots[buttonIndex].GetComponentInChildren<TextMeshProUGUI>().text = "";
    //    }
    //    else
    //    {
    //        slots[buttonIndex].GetComponentInChildren<TextMeshProUGUI>().text = cooldown.ToString();
    //    }
        
        
    //}

    public void UpdateCooldown(int buttonIndex, int cooldown)
    {
        if (cooldown == 0)
        {
            SetButtonActive(buttonIndex);
        }
        else
        {
            SetButtonInactive(buttonIndex, cooldown);
        }

    }

    public void MakeSkillsUnclickable()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            SetButtonInactive(i, 0);
        }
    }

    //public void MakeSkillsClickable()
    //{
    //    PlayerCombat playerCombat = FindObjectOfType<PlayerCombat>();
    //    Weapon weapon = playerCombat.GetWeapon();

    //    for (int i = 0; i < skills.Count; i++)
    //    {
    //        if (i < slots.Length)
    //        {
    //            if (skills[i].requiresWeapon)
    //            {
    //                Debug.Log("Skill with name: " + skills[i].skillName + "; Requires weapon" + skills[i].requiresWeapon +
    //                    "; Weapon type required: " + skills[i].weaponTypeRequired +
    //                    "; Weapon is actually of type: " + weapon.weaponType);
    //                if (weapon.weaponType == skills[i].weaponTypeRequired)
    //                {
    //                    Debug.Log("Weapon type matches");
    //                    SetButtonActive(i);
    //                }
    //                else SetButtonInactive(i, 0);

    //            }
    //            else
    //            {
    //                Debug.Log("Setting to inactive: " + skills[i].skillName);
    //                SetButtonInactive(i, 0);
    //            }
    //        }

    //    }
    //}

    public void PromptLearningSkill(int id, Item item)
    {
        learnSkillPanel.SetActive(true);
        skillName.text = Constants.GetInstance().skillMap[id].skillName;
        Color textColor = Color.magenta;
        //switch (Constants.GetInstance().skillMap[id].skillBelongsTo)
        //{
        //    case Constants.SkillBelongsTo.Axe:
        //        textColor = Color.red;
        //        break;
        //    case Constants.SkillBelongsTo.Bow: 
        //        textColor = Color.green;
        //        break;
        //    case Constants.SkillBelongsTo.Fire:
        //        textColor = Color.yellow;
        //        break;
        //    case Constants.SkillBelongsTo.Water:
        //        textColor = Color.blue;
        //        break;
        //    case Constants.SkillBelongsTo.Earth:
        //        Debug.Log("Got here");
        //        textColor = new Color(0.4245283f, 0.1902367f, 0.1902367f); // brown
        //        break;
        //    case Constants.SkillBelongsTo.Air:
        //        textColor = Color.grey;
        //        break;
        //    case Constants.SkillBelongsTo.Other:
        //        textColor = Color.magenta;
        //        break;
        //}
        skillName.color = textColor;
        skillToLearnId = id;
        skillToLearnInventory = item;
    }

    public void UpdateSkillbar()
    {
        skills = pc.skills;
        AssignSkillsToButtons();
        AssignTooltips();
    }

    public void CancelLearning()
    {
        learnSkillPanel.SetActive(false);
        skillName.text = "";
        skillToLearnId = -1;
    }

    public void ConfirmLearning()
    {
        learnSkillPanel.SetActive(false);
        skillName.text = "";
        if (skillToLearnId == -1 || Constants.GetInstance().skillMap[skillToLearnId] == null)
        {
            Debug.LogError("This skill doesn't exist in the map or in the context of this object");
        }
        else
        {
            pc.AddNewSkill(skillToLearnId);
            skillToLearnId = -1;
            skills = pc.skills;
            AssignSkillsToButtons();
            AssignTooltips();
            InventoryManager.GetInstance().Remove(skillToLearnInventory);
        }
        
    }
}
