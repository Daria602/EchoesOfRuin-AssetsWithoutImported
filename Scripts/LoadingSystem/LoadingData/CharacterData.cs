using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    // Appearance related
    public bool isMale;
    public bool darkSkin;
    public int currentHeadIndex;
    public int currentHairIndex;
    public int currentHairColor;

    // Attributes
    public int availableAttributePoints;
    public int strength;
    public int agility;
    public int intelligence;
    public int constitution;
    public int wits;

    // Abilities
    public int availableAbilityPoints;
    public int oneHanded;
    public int twoHanded;
    public int dualWielding;
    public int ranged;
    public int fireAffinity;
    public int airAffinity;
    public int waterAffinity;
    public int earthAffinity;

    // Creating initial values
    public CharacterData()
    {
        this.isMale = true;
        this.darkSkin = false;
        this.currentHeadIndex = 0;
        this.currentHairIndex = 0;
        this.currentHairColor = 0;

        // Attributes
        this.availableAttributePoints = 5;
        this.strength = 10;
        this.agility = 10;
        this.intelligence = 10;
        this.constitution = 10;
        this.wits = 10;

        // Abilities
        this.availableAbilityPoints = 3;
        this.oneHanded = 0;
        this.twoHanded = 0;
        this.dualWielding = 0;
        this.ranged = 0;
        this.fireAffinity = 0;
        this.airAffinity = 0;
        this.waterAffinity = 0;
        this.earthAffinity = 0;
}

}
