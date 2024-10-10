using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetupScript : MonoBehaviour
{
    public Movement Playermovement;
    public GameObject Playercamera;

    // Start is called before the first frame update
    public void IsLocalPlayer() 
    {
        Playermovement.enabled = true;
        Playercamera.SetActive(true);
    }
}
