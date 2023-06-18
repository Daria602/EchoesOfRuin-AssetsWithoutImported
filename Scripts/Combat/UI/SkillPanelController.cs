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

    public static SkillPanelController instance;
    public SimpleTooltip[] tooltips;
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
        //skillPanel.SetActive(false);
    }

    private void Update()
    {
        if (afterStart)
        {
            
            skills = pc.skills;
            Debug.Log("Skill panel thing " + skills.Count);
            AssignSkillsToButtons();
            AssignTooltips();
            afterStart = false;
        }
        
        if (isInCombat)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        if (isInCombat != setInCombat)
        {
            skillPanelVisibility = !skillPanelVisibility;
            skillPanel.SetActive(skillPanelVisibility);
            isInCombat = setInCombat;
        }
        
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
                        tooltips[index].infoLeft = skills[index].description;
                        tooltips[index].infoRight = skills[index].name;
                    }
                    index++;
                    
                }
            }
        }
    }
    public void AssignSkillsToButtons()
    {
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

    public void SetButtonInactive(bool activity, int buttonIndex, int cooldown)
    {
        slots[buttonIndex].interactable = activity;
        if (cooldown == 0)
        {
            slots[buttonIndex].GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
        else
        {
            slots[buttonIndex].GetComponentInChildren<TextMeshProUGUI>().text = cooldown.ToString();
        }
        
        
    }

    public void UpdateCooldown(int buttonIndex, int cooldown)
    {
        if (cooldown == 0)
        {
            SetButtonInactive(true, buttonIndex, 0);
        }
        else
        {
            slots[buttonIndex].GetComponentInChildren<TextMeshProUGUI>().text = cooldown.ToString();
        }
        
    }
}
