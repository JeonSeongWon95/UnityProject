using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject GM = GameObject.Find("GameManager");
            GM.GetComponent<GameManagerScript>().photonView.RPC("EndGame", Photon.Pun.RpcTarget.All);
            Debug.Log("GameEnd Is true");
        }
    }
}
