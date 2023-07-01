using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameAppearanceLoad : MonoBehaviour, ILoadingData
{
    public GameObject maleCharacter;
    public GameObject femaleCharacter;
    [Header("Female")]
    public RuntimeAnimatorController femaleAnimator;
    public Avatar femaleAvatar;
    public Transform femaleRightHand;
    [Header("Male")]
    public RuntimeAnimatorController maleAnimator;
    public Avatar maleAvatar;
    public Transform maleRightHand;
    public void LoadGameData(CharacterData characterData)
    {
        if (characterData.isMale)
        {
            maleCharacter.GetComponent<CharacterCreation>().LoadGameData(characterData);
            maleCharacter.SetActive(true);

            GetComponent<Animator>().runtimeAnimatorController = maleAnimator;
            GetComponent<Animator>().avatar = maleAvatar;

            GetComponent<CharacterController>().rightHand = maleRightHand;
        }
        else
        {
            femaleCharacter.GetComponent<CharacterCreation>().LoadGameData(characterData);
            femaleCharacter.SetActive(true);
            GetComponent<Animator>().runtimeAnimatorController = femaleAnimator;
            GetComponent<Animator>().avatar = femaleAvatar;


            GetComponent<CharacterController>().rightHand = femaleRightHand;
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
