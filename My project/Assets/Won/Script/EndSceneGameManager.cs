using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Unity.VisualScripting;

public class EndSceneGameManager : MonoBehaviourPunCallbacks
{
    private float GameEndTimer;
    private GameObject Player;
    private GameObject SpawnEndHUD;

    public GameObject EndHUD;
    void Start()
    {
        SpawnEndHUD = PhotonNetwork.Instantiate("EndHUD", Vector3.zero, Quaternion.identity);

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

    void FindPlayerNameText() 
    {
        SetNickNameScript SetNickNameScr = Player.GetComponent<SetNickNameScript>();
        Text[] Playertexts = SpawnEndHUD.GetComponentsInChildren<Text>();

        foreach (Text text in Playertexts)
        {
            if (text.name == "PlayerName(Clone)")
            {
                SetNickNameScr.SetText(text);
                break;
            }
        }
    }

}
