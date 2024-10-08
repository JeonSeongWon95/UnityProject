using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class EndSceneGameManager : MonoBehaviourPunCallbacks
{
    private float GameEndTimer;
    private GameObject Player;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Player = PhotonNetwork.Instantiate("WinnerCharacter", Vector3.zero, Quaternion.identity);
        }

    }

    // Update is called once per frame
    void Update()
    {
        GameEndTimer += Time.deltaTime;

        if (GameEndTimer > 8.0f)
        {
            GameEndTimer = 0.0f;
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("Title");
        }
    }

}
