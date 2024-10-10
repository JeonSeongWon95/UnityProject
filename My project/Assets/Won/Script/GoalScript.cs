using Photon.Pun;
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
            PlaySceneGameManagerScript PlaySceneGameManagerScr = GM.GetComponent<PlaySceneGameManagerScript>();
            PlaySceneGameManagerScr.photonView.RPC("EndGame", Photon.Pun.RpcTarget.All);

            PhotonView WinnerPhotonView = other.gameObject.GetComponent<PhotonView>();
            ExitGames.Client.Photon.Hashtable WinnerPlayer = new ExitGames.Client.Photon.Hashtable();
            WinnerPlayer["IsWinner"] = true;
            WinnerPhotonView.Owner.SetCustomProperties(WinnerPlayer);

            Debug.Log("GameEnd Is true");
        }
    }
}
