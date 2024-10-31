using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownScript : MonoBehaviour
{
    public Text Number;

    public void ChangeNumber(int NewNumber) 
    {
        Number.text = NewNumber.ToString();
    }
}
