using System;
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
    void Awake()
    {
        Resolution_DropDown.ClearOptions();
        resolutions = Screen.resolutions;

        foreach (Resolution resolution in resolutions)
        {
            float refreshRateValue = Convert.ToSingle(resolution.refreshRateRatio);

            if (refreshRateValue != 60.0f)
                continue;

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

    public void GetResolutionData(int Index) 
    {
        ResolutionData = Index;
    }

    public void ChangeResolution() 
    {
        Screen.SetResolution(resolutions[ResolutionData].width, resolutions[ResolutionData].height, IsFullScreen);
    }

    public void RefreshDropDown() 
    {
        Resolution_DropDown.RefreshShownValue();
    }

    public void SetIsFullScreen(bool NewValue) 
    {
        IsFullScreen = NewValue;
    }

    public void OnClickExitButton() 
    {
        gameObject.SetActive(false);
    }
}
