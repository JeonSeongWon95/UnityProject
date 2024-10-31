using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUIScript : MonoBehaviour
{
    public InputField InputField_Name;
    public TitleGameManagerScript TitleGameManagerScr;

    public void OnClickConnectButton() 
    {
        if (InputField_Name == null)
            return;

        if(InputField_Name.text.Length > 0) 
        {
            if (TitleGameManagerScr == null)
                return;

            TitleGameManagerScr.SetUserName(InputField_Name.text);
            TitleGameManagerScr.NextStep();

            gameObject.SetActive(false);
        }

    }
}
