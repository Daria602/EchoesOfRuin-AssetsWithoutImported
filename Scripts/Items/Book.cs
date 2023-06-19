using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Book", menuName = "Inventory/Create New Book")]
public class Book : Item
{
    public Skill skill;

    public override void Use()
    {
        base.Use();
        SkillPanelController.GetInstance().PromptLearningSkill(skill.skillId, this);
    }
}
