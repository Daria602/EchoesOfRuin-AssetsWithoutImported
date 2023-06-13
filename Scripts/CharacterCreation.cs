using CartoonHeroes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreation : MonoBehaviour
{
    // TODO: make templates for hair and etc here

    private SetCharacter setCharacter;

    [System.Serializable]
    public class BodyPart
    {
        public GameObject item;
        public string itemName;
        public MaterialWithName[] materialsApplied;
        public int currentMaterialIndex = 0;
        public bool affectedBySkinColor = false;
    }

    [System.Serializable]
    public class MaterialWithName
    {
        public Material material;
        public string name;
    }

    public BodyPart[] hairParts;
    private int currentHairIndex = 0;

    public BodyPart[] headParts;
    private int currentHeadIndex = 0;

    public BodyPart[] torsoParts;
    private int currentTorsoIndex = 0;

    public BodyPart[] legsParts;
    private int currentLegsIndex = 0;


    private void Start()
    {
        setCharacter = GetComponent<SetCharacter>();
    }

    private void Update()
    {
        // Debug.Log(currentHeadIndex);
    }

    public enum GroupTypes
    {
        Hair,
        HairMaterial,
        Head,
        HeadMaterial,
        Torso,
        TorsoMaterial,
        Legs,
        LegsMaterial,
        Skin
    }

    private bool darkSkinColor = false;


    public void ToggleSkinColor()
    {
        darkSkinColor = !darkSkinColor;

        // go through all of the items, and if they are affected by skin color, toggle them
        for (int i = 0; i < headParts.Length; i++)
        {
            if (headParts[i].affectedBySkinColor)
            {
                Debug.Log(headParts[i].item.name);
                try
                {
                    headParts[i].item.GetComponent<SkinnedMeshRenderer>().material = headParts[i].materialsApplied[darkSkinColor ? 1 : 0].material;
                }
                catch (MissingComponentException ex)
                {
                    headParts[i].item.GetComponent<MeshRenderer>().material = headParts[i].materialsApplied[darkSkinColor ? 1 : 0].material;
                }
                
            }
            
        }
        for (int i = 0; i < torsoParts.Length; i++)
        {
            if (torsoParts[i].affectedBySkinColor)
            {
                try
                {
                    int materialLength = torsoParts[i].item.GetComponent<SkinnedMeshRenderer>().materials.Length;
                    if (materialLength > 1)
                    {
                        // do for both
                        // torsoParts[i].item.GetComponent<SkinnedMeshRenderer>().materials[materialToModify] = torsoParts[i].materialsApplied[darkSkinColor ? 1 : 0].material;
                        Material[] currentMaterials = torsoParts[i].item.GetComponent<SkinnedMeshRenderer>().materials;
                        currentMaterials[1] = torsoParts[i].materialsApplied[darkSkinColor ? 1 : 0].material;
                        torsoParts[i].item.GetComponent<SkinnedMeshRenderer>().materials = currentMaterials;
                    }
                    else
                    {
                        // do for the first
                        torsoParts[i].item.GetComponent<SkinnedMeshRenderer>().material = torsoParts[i].materialsApplied[darkSkinColor ? 1 : 0].material;
                    }
                    
                }
                catch (MissingComponentException ex)
                {
                    int materialLength = torsoParts[i].item.GetComponent<MeshRenderer>().materials.Length;
                    if (materialLength > 1)
                    {
                        // do for both
                        // torsoParts[i].item.GetComponent<SkinnedMeshRenderer>().materials[materialToModify] = torsoParts[i].materialsApplied[darkSkinColor ? 1 : 0].material;
                        Material[] currentMaterials = torsoParts[i].item.GetComponent<MeshRenderer>().materials;
                        currentMaterials[1] = torsoParts[i].materialsApplied[darkSkinColor ? 1 : 0].material;
                        torsoParts[i].item.GetComponent<MeshRenderer>().materials = currentMaterials;
                    }
                    else
                    {
                        // do for the first
                        torsoParts[i].item.GetComponent<MeshRenderer>().material = torsoParts[i].materialsApplied[darkSkinColor ? 1 : 0].material;
                    }
                }

            }
        }
        for (int i = 0; i < legsParts.Length; i++)
        {
            if (legsParts[i].affectedBySkinColor)
            {
                try
                {
                    int materialLength = legsParts[i].item.GetComponent<SkinnedMeshRenderer>().materials.Length;
                    if (materialLength > 1)
                    {
                        // do for both
                        // torsoParts[i].item.GetComponent<SkinnedMeshRenderer>().materials[materialToModify] = torsoParts[i].materialsApplied[darkSkinColor ? 1 : 0].material;
                        Material[] currentMaterials = legsParts[i].item.GetComponent<SkinnedMeshRenderer>().materials;
                        currentMaterials[1] = legsParts[i].materialsApplied[darkSkinColor ? 1 : 0].material;
                        legsParts[i].item.GetComponent<SkinnedMeshRenderer>().materials = currentMaterials;
                    }
                    else
                    {
                        // do for the first
                        legsParts[i].item.GetComponent<SkinnedMeshRenderer>().material = legsParts[i].materialsApplied[darkSkinColor ? 1 : 0].material;
                    }

                }
                catch (MissingComponentException ex)
                {
                    int materialLength = legsParts[i].item.GetComponent<MeshRenderer>().materials.Length;
                    if (materialLength > 1)
                    {
                        // do for both
                        // torsoParts[i].item.GetComponent<SkinnedMeshRenderer>().materials[materialToModify] = torsoParts[i].materialsApplied[darkSkinColor ? 1 : 0].material;
                        Material[] currentMaterials = legsParts[i].item.GetComponent<MeshRenderer>().materials;
                        currentMaterials[1] = legsParts[i].materialsApplied[darkSkinColor ? 1 : 0].material;
                        legsParts[i].item.GetComponent<MeshRenderer>().materials = currentMaterials;
                    }
                    else
                    {
                        // do for the first
                        legsParts[i].item.GetComponent<MeshRenderer>().material = legsParts[i].materialsApplied[darkSkinColor ? 1 : 0].material;
                    }
                }

            }
        }

    }

    public void ActivateNextItem(ref BodyPart[] group, ref int currentIndex)
    {
        currentIndex = (currentIndex + 1 == group.Length) ? 0 : currentIndex + 1;
        SwitchActiveItem(ref group, currentIndex);

    }

    public void ActivatePreviousItem(ref BodyPart[] group, ref int currentIndex)
    {
        currentIndex = (currentIndex - 1 < 0) ? group.Length - 1 : currentIndex - 1;
        SwitchActiveItem(ref group, currentIndex);

    }

    public void SwitchActiveItem(ref BodyPart[] group, int index)
    {
        for (int i = 0; i < group.Length; i++)
        {
            group[i].item.SetActive(false);
            if (i == index)
            {
                group[i].item.SetActive(true);
            }
        }
    }

    public void ActivateNextMaterial(ref BodyPart[] bodyParts, int currentBodyPart)
    {
        // increase the index of currently applied material
        int nextMaterial = bodyParts[currentBodyPart].currentMaterialIndex;
        nextMaterial = (nextMaterial + 1 == bodyParts[currentBodyPart].materialsApplied.Length) ? 0 : nextMaterial + 1;
        bodyParts[currentBodyPart].currentMaterialIndex = nextMaterial;
        ApplyMaterial(ref bodyParts[currentBodyPart].item, bodyParts[currentBodyPart].materialsApplied[nextMaterial].material);

    }

    public void ActivatePreviousMaterial(ref BodyPart[] bodyParts, int currentBodyPart)
    {
        // increase the index of currently applied material
        int nextMaterial = bodyParts[currentBodyPart].currentMaterialIndex;
        nextMaterial = (nextMaterial - 1 < 0) ? bodyParts[currentBodyPart].materialsApplied.Length - 1 : nextMaterial - 1;
        bodyParts[currentBodyPart].currentMaterialIndex = nextMaterial;
        ApplyMaterial(ref bodyParts[currentBodyPart].item, bodyParts[currentBodyPart].materialsApplied[nextMaterial].material);

    }

    public void ApplyMaterial(ref GameObject bodyPart, Material material)
    {
        // apply new material
        try
        {
            bodyPart.GetComponent<SkinnedMeshRenderer>().material = material;
        }
        catch (MissingComponentException ex)
        {
            bodyPart.GetComponent<MeshRenderer>().material = material;
        }
    }

    public void Next(int groupIndex)
    {
        switch (groupIndex)
        {
            case (int)GroupTypes.Hair:
                ActivateNextItem(ref hairParts, ref currentHairIndex);
                break;
            case (int)GroupTypes.HairMaterial:
                ActivateNextMaterial(ref hairParts, currentHairIndex);
                break;
            case (int)GroupTypes.Head:
                ActivateNextItem(ref headParts, ref currentHeadIndex);
                break;
            case (int)GroupTypes.Torso:
                //ActivateNextItem(ref torsos, ref currentTorsoIndex);
                break;
            case (int)GroupTypes.Legs:
                //ActivateNextItem(ref legs, ref currentLegsIndex);
                break;
        }
        //Debug.Log("In Next");
        //Debug.Log("Current Head");
        //Debug.Log(currentHeadIndex);
        //Debug.Log("Current Hair");
        //Debug.Log(currentHairIndex);
        //Debug.Log("Current Color");
    }
    public void Previous(int groupIndex)
    {
        switch (groupIndex)
        {
            case (int)GroupTypes.Hair:
                ActivatePreviousItem(ref hairParts, ref currentHairIndex);
                break;
            case (int)GroupTypes.HairMaterial:
                ActivatePreviousMaterial(ref hairParts, currentHairIndex);
                break;
            case (int)GroupTypes.Head:
                ActivatePreviousItem(ref headParts, ref currentHeadIndex);
                break;
            case (int)GroupTypes.Torso:
                //ActivatePreviousItem(ref torsos, ref currentTorsoIndex);
                break;
            case (int)GroupTypes.Legs:
                //ActivatePreviousItem(ref legs, ref currentLegsIndex);
                break;
        }
        //Debug.Log("In Previous");
        //Debug.Log("Current Head");
        //Debug.Log(currentHeadIndex);
        //Debug.Log("Current Hair");
        //Debug.Log(currentHairIndex);
        //Debug.Log("Current Color");
    }

    public string[] GetAllCurrentTypeNames()
    {
        string headType = headParts[currentHeadIndex].itemName;
        string hairType = hairParts[currentHairIndex].itemName;
        string hairColor = hairParts[currentHairIndex]
            .materialsApplied[hairParts[currentHairIndex].currentMaterialIndex]
            .name;
        string skinColor = darkSkinColor ? "Dark" : "Light";
        string[] typeNames = {headType, hairType, hairColor, skinColor};
        return typeNames;
    }

    public void ResetCharacter()
    {
        // Reset head type
        currentHeadIndex = 0;
        SwitchActiveItem(ref headParts, 0);
        // Reset hair type
        currentHairIndex = 0;
        SwitchActiveItem(ref hairParts, 0);
        
        // Reset hair color for every hair type
        for (int i = 0; i < hairParts.Length; i++)
        {
            hairParts[i].currentMaterialIndex = 0;
            ApplyMaterial(ref hairParts[i].item, hairParts[i].materialsApplied[0].material);
        }

        // Reset skin color
        if (darkSkinColor)
        {
            ToggleSkinColor();
        }

    }

    public void LoadGameData(CharacterData characterData)
    {
        if (this.darkSkinColor != characterData.darkSkin)
        {
            ToggleSkinColor();
        }
        this.darkSkinColor = characterData.darkSkin;
        this.currentHeadIndex = characterData.currentHeadIndex;
        this.currentHairIndex = characterData.currentHairIndex;
        hairParts[currentHairIndex].currentMaterialIndex = characterData.currentHairColor;
        ApplyLoadedData();
    }

    private void ApplyLoadedData()
    {
        // apply the changes after loaded data has been settled loading
        SwitchActiveItem(ref headParts, currentHeadIndex);
        SwitchActiveItem(ref hairParts, currentHairIndex);
        int materialIndex = hairParts[currentHairIndex].currentMaterialIndex;
        ApplyMaterial(
            ref hairParts[currentHeadIndex].item, 
            hairParts[currentHairIndex].materialsApplied[materialIndex].material
            );
    }

    public void SaveGameData(ref CharacterData characterData)
    {
        characterData.currentHeadIndex = this.currentHeadIndex;
        characterData.currentHairIndex = this.currentHairIndex;
        characterData.currentHairColor = hairParts[currentHairIndex].currentMaterialIndex;
        characterData.darkSkin = darkSkinColor;
    }
}
