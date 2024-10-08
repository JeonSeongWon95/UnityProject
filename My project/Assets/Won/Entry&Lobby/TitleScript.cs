using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScript : MonoBehaviour
{
    // Start is called before the first frame update
    bool ChanageStep = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Return) && !ChanageStep) 
        {
            TitleGameManager TGM = GameObject.Find("GameManager").GetComponent<TitleGameManager>();
            TGM.NextStep();
            ChanageStep = true;

            Destroy(gameObject);
        }
    }
}
