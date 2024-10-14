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
    public GameObject ChatUI;
    public SocketScript SocketScr;

    private float GameEndTimer = 0.0f;
    private float GameStartTimer = 0.0f;
    private GameObject Player;
    private bool IsGameStart = false;
    private GameObject SpawnChatUI;
    private ChatUIScript ChatUIScr;
    void Start()
    {
        Player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.Euler(0, 90, 0));
        SpawnChatUI = Instantiate(ChatUI);
        PlayerScript PlayerScr = Player.GetComponent<PlayerScript>();
        ChatUIScr = SpawnChatUI.GetComponent<ChatUIScript>();
        ChatUIScr.SetSocketScript(SocketScr);
        ChatUIScr.SetPlayerScript(PlayerScr);
        SocketScr.SetChatUI(ChatUIScr);
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
