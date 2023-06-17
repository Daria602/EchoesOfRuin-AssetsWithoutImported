using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private void Awake()
    {
        
    }

    private void Start()
    {
        
        //skillPanel.SetActive(false);
    }

    private void Update()
    {
        if (afterStart)
        {
            
            skills = pc.skills;
            Debug.Log("Skill panel thing " + skills.Count);
            AssignSkillsToButtons();
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
}
