using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon.StructWrapping;
using System;
using Photon.Pun;
using Photon.Realtime;

public class PlaySceneGameManagerScript : GameManager
{
    public Vector3 GoalPosition;
    public GameObject GameEndUI;
    public GameObject ChatUI;
    public CountDownScript CountDownUIScr;
    public SocketScript SocketScr;
    public Transform[] SpawnPosition;
    public int Count = 5;
    public PhotonView PV;

    private float GameStartTimer = 0.0f;
    private GameObject Player;
    private bool IsGameStart = false;
    private ChatUIScript ChatUIScr;
    void Awake()
    {
        SpawnAndSetLocalPlayer();

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Goal", GoalPosition, Quaternion.identity);
        }

    }
    void Update()
    {
        if (IsGameStart == false)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameStartTimer += Time.deltaTime;

                if (GameStartTimer >= 1.0f)
                {
                    photonView.RPC("RPC_CountDown", Photon.Pun.RpcTarget.All);
                    GameStartTimer = 0;
                }
            }

            if (Count == 0)
            {
                photonView.RPC("RPC_GameStart", Photon.Pun.RpcTarget.All);
            }

        }
    }

    IEnumerator EndGame() 
    {
        GameEndUI.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene("EndScene");
    }

    [PunRPC]
    public void RPC_EndGame()
    {
        StartCoroutine(EndGame());
    }

    [PunRPC]
    public void RPC_CountDown()
    {
        Count -= 1;
        CountDownUIScr.ChangeNumber(Count);

        if (Count <= 0)
        {
            IsGameStart = true;
        }
    }

    [PunRPC]
    public void RPC_GameStart() 
    {
        CountDownUIScr.gameObject.SetActive(false);
    }

    void SpawnAndSetLocalPlayer() 
    {
        Transform SpawnTransform = SpawnPosition[UnityEngine.Random.Range(0, SpawnPosition.Length)];
        Player = PhotonNetwork.Instantiate("Player", SpawnTransform.position, Quaternion.Euler(0, 90, 0));

        base.SetCharacterRender(Player);
        base.LoadSkin();
        base.ChangeSkin();

        ChatUI.SetActive(true);
        PlayerScript PlayerScr = Player.GetComponent<PlayerScript>();
        ChatUIScr = ChatUI.GetComponent<ChatUIScript>();
        ChatUIScr.SetPlayerScript(PlayerScr);
        PlayerScr.enabled = true;
        PlayerScr.LocalPlayerSet();
    }

    public bool GetIsGameStart() 
    {
        return IsGameStart;
    }

    void SetCharacterSkin()
    {
        ExitGames.Client.Photon.Hashtable properties;

        if (PV.IsMine)
        {
            properties = PhotonNetwork.LocalPlayer.CustomProperties;
        }
        else
        {
            properties = PV.Owner.CustomProperties;
        }

        properties.TryGetValue("Skin", out SkinColor);
        ChangeSkin();
    }



}
