using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Chat.Demo;
using System;

public class PlaySceneGameManagerScript : MonoBehaviourPunCallbacks
{
    public bool IsGameEnd = false;
    public Vector3 GoalPosition;
    public GameObject GameEndUI;
    public GameObject ChatUI;
    public GameObject CountDownUI;
    public SocketScript SocketScr;
    public Transform[] SpawnPosition;
    public int Count = 5;

    private float GameEndTimer = 0.0f;
    private float GameStartTimer = 0.0f;
    private GameObject Player;
    private bool IsGameStart = false;
    private GameObject SpawnChatUI;
    private CountDownScript CountDownUIScr;
    private ChatUIScript ChatUIScr;
    void Start()
    {
        SpawnAndSetLocalPlayer();
        CountDownUIScr = Instantiate(CountDownUI).GetComponent<CountDownScript>();

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Goal", GoalPosition, Quaternion.identity);
        }

    }
    void Update()
    {
        if (IsGameStart == false && PhotonNetwork.IsMasterClient)
        {
            GameStartTimer += Time.deltaTime;

            if (GameStartTimer >= 1.0f)
            {
                photonView.RPC("CountDown", Photon.Pun.RpcTarget.All);

                if (Count == 0)
                {
                    CountDownUIScr.gameObject.SetActive(false);
                }
                GameStartTimer = 0;
            }
        }


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

    [PunRPC]
    public void CountDown()
    {
        Count -= 1;
        CountDownUIScr.ChangeNumber(Count);

        if (Count <= 0)
        {
            IsGameStart = true;
        }
    }

    void SpawnAndSetLocalPlayer() 
    {
        Transform SpawnTransform = SpawnPosition[UnityEngine.Random.Range(0, SpawnPosition.Length)];
        Player = PhotonNetwork.Instantiate("Player", SpawnTransform.position, Quaternion.Euler(0, 90, 0));
        SpawnChatUI = Instantiate(ChatUI);
        PlayerScript PlayerScr = Player.GetComponent<PlayerScript>();
        ChatUIScr = SpawnChatUI.GetComponent<ChatUIScript>();
        ChatUIScr.SetSocketScript(SocketScr);
        ChatUIScr.SetPlayerScript(PlayerScr);
        SocketScr.SetChatUI(ChatUIScr);
        PlayerScr.enabled = true;
        PlayerScr.LocalPlayerSet();
    }

    public bool GetIsGameStart() 
    {
        return IsGameStart;
    }

}
