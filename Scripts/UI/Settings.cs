using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Settings : MonoBehaviour
{
    Resolution[] screenRes;
    [SerializeField]
    private TMP_Dropdown resDropdown;

    void Start()
    {
        AddResolutionsToDropdown();
    }


    private void AddResolutionsToDropdown()
    {
        int currentResIndex = 0;
        screenRes = Screen.resolutions;
        resDropdown.ClearOptions();
        List<string> runtimeResOptions = new List<string>();
        for (int i = 0; i < screenRes.Length; i++)
        {
            string resOption = screenRes[i].width + "x" + screenRes[i].height;
            runtimeResOptions.Add(resOption);
            if (screenRes[i].height == Screen.currentResolution.height &&
                screenRes[i].width == Screen.currentResolution.width)
            {
                currentResIndex = i;
            }
        }
        resDropdown.AddOptions(runtimeResOptions);
        resDropdown.value = currentResIndex;
        resDropdown.RefreshShownValue();
    }

    public void ScreenSizeChange(bool isGameFullScreen)
    {
        Screen.fullScreen = isGameFullScreen;
    }

    private bool IsScreenFullSize()
    {
       return Screen.fullScreen;
    }

    public void ResolutionChange(int resIndex)
    {
        Resolution res = screenRes[resIndex];
        Screen.SetResolution(res.width, res.height, IsScreenFullSize());
    }

}
