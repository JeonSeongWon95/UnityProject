using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField InputField_Name = null;
    public GameObject GameManager = null;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickConnectButton() 
    {
        if (InputField_Name == null)
            return;

        if(InputField_Name.text.Length > 0) 
        {
            if (GameManager == null)
                return;

            TitleGameManagerScript TitleGameManagerScr = GameManager.GetComponent<TitleGameManagerScript>();
            TitleGameManagerScr.SetUserName(InputField_Name.text);
            TitleGameManagerScr.NextStep();

            Destroy(gameObject);
        }

    }
}
