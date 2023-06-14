using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skills/Create New Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public Sprite skillIcon;
    public int cost;
    public bool hasVisuals = false;
    public GameObject visualEffectPrefab = null;
    private GameObject visualEffect = null;
    public string prepareAnimationBoolName = "";
    public string affectAnimationBoolName = "";
    public float minDistance = 0;
    public string description;

    public void SetEffect(GameObject character)
    {
        if (hasVisuals)
        {
            if (visualEffect == null)
            {
                visualEffect = Instantiate(visualEffectPrefab, character.transform);
            }
        }
    }

    public void RemoveEffect()
    {
        if (visualEffect != null)
        {
            Destroy(visualEffect);
            
        }
    }
}
