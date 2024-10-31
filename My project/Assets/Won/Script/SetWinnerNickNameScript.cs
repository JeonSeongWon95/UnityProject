using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SetWinnerNickNameScript : MonoBehaviour
{
    public Text WinnerName;
    public void SetName(string NewName) 
    {
        WinnerName.text = NewName;
    }
}
