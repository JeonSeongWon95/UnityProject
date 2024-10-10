using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionScript : MonoBehaviour
{
    public Dropdown Resolution_DropDown;
    public int ResolutionData;
    private Resolution[] resolutions;
    private bool IsFullScreen = false;
    void Start()
    {
        Resolution_DropDown.ClearOptions();
        resolutions = Screen.resolutions;

        foreach (Resolution resolution in resolutions)
        {
            Dropdown.OptionData Data = new Dropdown.OptionData();
            Data.text = resolution.width + " X " + resolution.height;
            Resolution_DropDown.options.Add(Data);

            if (Screen.width == resolution.width && Screen.height == resolution.height)
            {
                Resolution_DropDown.value = Resolution_DropDown.options.Count - 1;
            }
        }

        RefreshDropDown();
    }
    void Update()
    {
        
    }

    public void GetResolutionData(int Index) 
    {
        Debug.Log("Present Resolution Data : " + Index);
        ResolutionData = Index;
    }

    public void ChangeResolution() 
    {
        Debug.Log("Resolution is " + resolutions[ResolutionData].width + " X " + resolutions[ResolutionData].height + IsFullScreen);
        Screen.SetResolution(resolutions[ResolutionData].width, resolutions[ResolutionData].height, IsFullScreen);
    }

    public void RefreshDropDown() 
    {
        Resolution_DropDown.RefreshShownValue();
    }

    public void SetIsFullScreen(bool NewValue) 
    {
        Debug.Log("Chanager IsFullScreen : " + NewValue);
        IsFullScreen = NewValue;
    }

    public void OnClickExitButton() 
    {
        gameObject.SetActive(false);
    }
}
