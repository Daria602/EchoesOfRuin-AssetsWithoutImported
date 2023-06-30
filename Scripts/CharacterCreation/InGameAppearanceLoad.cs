using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameAppearanceLoad : MonoBehaviour, ILoadingData
{
    public GameObject maleCharacter;
    public GameObject femaleCharacter;
    public void LoadGameData(CharacterData characterData)
    {
        if (characterData.isMale)
        {
            maleCharacter.GetComponent<CharacterCreation>().LoadGameData(characterData);
            maleCharacter.SetActive(true);
        }
        else
        {
            femaleCharacter.GetComponent<CharacterCreation>().LoadGameData(characterData);
            femaleCharacter.SetActive(true);
        }
        //throw new System.NotImplementedException();
    }

    public void SaveGameData(ref CharacterData characterData)
    {
        // throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
