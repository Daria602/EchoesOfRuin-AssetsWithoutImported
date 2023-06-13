using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoadingData
{
    void LoadGameData(CharacterData characterData);
    void SaveGameData(ref CharacterData characterData);
}
