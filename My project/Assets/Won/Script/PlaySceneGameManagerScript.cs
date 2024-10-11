using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Chat.Demo;

public class PlaySceneGameManagerScript : MonoBehaviourPunCallbacks
{
    public bool IsGameEnd = false;
    public Vector3 GoalPosition;
    public GameObject GameEndUI;
    public ChatUIScript ChatUI;

    private float GameEndTimer = 0.0f;
    private float GameStartTimer = 0.0f;
    private GameObject Player;
    private bool IsGameStart = false;

    void Start()
    {
        Player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.Euler(0, 90, 0));
        PlayerScript PlayerScr = Player.GetComponent<PlayerScript>();
        SocketScript SocketScr = Player.GetComponent<SocketScript>();
        ChatUI.SetSocketScript(SocketScr);
        SocketScr.SetChatUI(ChatUI);
        PlayerScr.enabled = true;
        PlayerScr.LocalPlayerSet();
        if (PhotonNetwork.IsMasterClient) 
        {
            PhotonNetwork.Instantiate("Goal", GoalPosition, Quaternion.identity);
        }

    }
    void Update()
    {
        if (IsGameEnd) 
        {
            GameEndTimer += Time.deltaTime;
            Instantiate(GameEndUI);

            if (GameEndTimer > 5.0f) 
            {
                Debug.Log("GameEnd");
                GameEndTimer = 0.0f;
                SceneManager.LoadScene("EndScene");
            }
        }
    }

    [PunRPC]
    public void EndGame() 
    {
        IsGameEnd = true;
    }

}
