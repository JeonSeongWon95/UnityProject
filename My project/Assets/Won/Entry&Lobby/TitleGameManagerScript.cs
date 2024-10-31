using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon.StructWrapping;

public class TitleGameManagerScript : GameManager
{
    public enum eStep
    {
        Title,
        Lobby,
        Loading,
        InGame
    }

    private eStep gamestep = eStep.Title;
    public GameObject[] TitleHUD;
    public GameObject LobbyCharacter;
    public GameObject LoadingCharacter;

    private int MaxClientCount = 1;
    private string UserName;


    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            StartCoroutine(ConnectToPhotonMasterServer());
            TitleHUD[(int)eStep.Title].SetActive(true);
        }
        else
        {
            base.LoadSkin();
            gamestep = eStep.Lobby;
            PlayGameLogic();
        }
    }

    IEnumerator ConnectToPhotonMasterServer()
    {

        for(int i = 0; i < 5; i++) 
        {
           PhotonNetwork.ConnectUsingSettings();

           yield return new WaitForSeconds(1f);

            if (PhotonNetwork.IsConnected)            
            {
                yield break;
            }
        }

    }

    public void PlayGameLogic()
    {
        switch (gamestep)
        {
            case eStep.Title:
                break;

            case eStep.Lobby:
                JoinLobby();
                LobbyCharacter.SetActive(true);
                TitleHUD[(int)eStep.Lobby].SetActive(true);
                base.SetCharacterRender(LobbyCharacter);

                LobbyUIScript LobbyUIScr = TitleHUD[(int)eStep.Lobby].GetComponent<LobbyUIScript>();
                LobbyUIScr.LoadName();

                break;

            case eStep.Loading:
                LobbyCharacter.SetActive(false);
                TitleHUD[(int)eStep.Lobby].SetActive(false);

                LoadingCharacter.SetActive(true);
                TitleHUD[(int)eStep.Loading].SetActive(true);

                base.SetCharacterRender(LoadingCharacter);
                JoinGameRoom();

                break;

            case eStep.InGame:
                SaveUserData();
                StartCoroutine(StartGame());
                break;

            default:
                break;
        }
    }

    public void NextStep()
    {
        gamestep++;
        PlayGameLogic();
    }

    void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    void JoinGameRoom()
    {
        PhotonNetwork.JoinOrCreateRoom("RoomOne", new RoomOptions { MaxPlayers = MaxClientCount }, null);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        UpdatePlayerUI();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        UpdatePlayerUI();
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(8.0f);
        SceneManager.LoadScene("PlayScene");
    }

    private void UpdatePlayerUI()
    {
        if (TitleHUD[(int)eStep.Loading].activeSelf)
        {

            Text[] texts = TitleHUD[(int)eStep.Loading].GetComponentsInChildren<Text>();

            if (texts.Length > 0)
            {
                foreach (Text text in texts)
                {
                    if (text.name == "TotalPlayerCount_Text")
                    {
                        text.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
                        break;
                    }
                }

                if (MaxClientCount == PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    NextStep();
                }
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        UpdatePlayerUI();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        UpdatePlayerUI();
    }

    void SaveUserData()
    {
        ResetUserData();
        ExitGames.Client.Photon.Hashtable customProperties = PhotonNetwork.LocalPlayer.CustomProperties;

        if (customProperties.ContainsKey("Skin"))
        {
            customProperties["Skin"] = (int)SkinColor;
            PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
        }
        else
        {
            ExitGames.Client.Photon.Hashtable PlayerSkin = new ExitGames.Client.Photon.Hashtable();
            PlayerSkin["Skin"] = (int)SkinColor;
            PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerSkin);
        }
    }

    public void SetUserName(string NewName)
    {
        UserName = NewName;
        PhotonNetwork.NickName = UserName;
    }

    public void SetNewSkin(eSkinColor NewColor)
    {
        SkinColor = NewColor;
        base.ChangeSkin();
    }

    void ResetUserData()
    {
        ExitGames.Client.Photon.Hashtable customProperties = PhotonNetwork.LocalPlayer.CustomProperties;

        if (customProperties.ContainsKey("IsWinner"))
        {
            customProperties.Remove("IsWinner");
            PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
        }
    }

    

}
