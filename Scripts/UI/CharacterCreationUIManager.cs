using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreationUIManager : MonoBehaviour
{
    public GameObject appearance;
    public GameObject attributes;
    public GameObject abilities;

    public enum NavigationButtons
    {
        Appearance,
        Attributes,
        Abilities
    }

    public void SwitchNavigation(int buttonClicked)
    {
        switch(buttonClicked)
        {
            case (int)NavigationButtons.Appearance:
                SetOneActive(0);
                break;
            case (int)NavigationButtons.Attributes:
                SetOneActive(1);
                break;
            case (int)NavigationButtons.Abilities:
                SetOneActive(2);
                break;
        }
    }

    private void SetOneActive(int panelIndex)
    {
        appearance.SetActive((panelIndex == 0) ? true : false);
        attributes.SetActive((panelIndex == 1) ? true : false);
        abilities.SetActive((panelIndex == 2) ? true : false);
    }
}
