using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon.StructWrapping;

public class TitleGameManagerScript : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public enum eStep
    {
        Title,
        Lobby,
        Loading,
        InGame
    }
    public enum eSkinColor
    {
        Red,
        Yellow,
        Purple,
        Blue,
        Green,
        White
    }

    private eStep gamestep = eStep.Title;
    private eSkinColor SkinColor;

    public GameObject LobbyCharacter;
    public GameObject LobbyUI;
    public GameObject LoadingCharacter;
    public GameObject LoadingUI;
    public GameObject TitleImage;
    private Renderer CharacterRender;
    private GameObject SpawnLobbyCharacter = null;
    private GameObject SpawnLobbyUI = null;
    private GameObject SpawnLoadingCharacter = null;
    private GameObject SpawnLoadingUI = null;
    private int MaxClientCount = 3;
    private float GameStartTimer = 0.0f;
    private bool IsStartGame = false;
    private string UserName;

    public Vector3 LobbyCharacterSpawnPosition;
    public Vector3 LoadingCharacterSpawnPosition;
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            GameObject SpawnTitleImage = Instantiate(TitleImage);
            TitleUIScript TitleUIScr = SpawnTitleImage.GetComponent<TitleUIScript>();
            TitleUIScr.GameManager = gameObject;
        }
        else
        {
            ExitGames.Client.Photon.Hashtable properties = PhotonNetwork.LocalPlayer.CustomProperties;
            properties.TryGetValue("Skin", out SkinColor);
            gamestep = eStep.Lobby;
            PlayGameLogic();
        }
    }

    public override void OnConnectedToMaster()
    {
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
    }
    void Update()
    {

        if (!PhotonNetwork.IsConnected)
        {
            Start();
        }

        if (IsStartGame)
        {
            StartGame();
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
                SpawnLobbyCharacter = Instantiate(LobbyCharacter, LobbyCharacterSpawnPosition, transform.rotation);
                SpawnLobbyUI = Instantiate(LobbyUI);

                SetCharacterRender(SpawnLobbyCharacter);
                ChangeSkin();

                LobbyUIScript LobbyUIScr = SpawnLobbyUI.GetComponent<LobbyUIScript>();
                SpawnLobbyUI.GetComponent<LobbyUIScript>().TitleGameManager = gameObject;
                LobbyUIScr.LoadName();
                break;

            case eStep.Loading:
                Destroy(SpawnLobbyCharacter);
                Destroy(SpawnLobbyUI);

                SpawnLoadingCharacter = Instantiate(LoadingCharacter, LoadingCharacterSpawnPosition, transform.rotation);
                SpawnLoadingUI = Instantiate(LoadingUI);
                SetCharacterRender(SpawnLoadingCharacter);
                JoinGameRoom();
                break;

            case eStep.InGame:
                Debug.Log("InGameStep Start");
                SaveUserData();
                IsStartGame = true;
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

    private void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
    }

    private void JoinGameRoom()
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

    private void StartGame()
    {

        GameStartTimer += Time.deltaTime;

        if (GameStartTimer > 8.0f)
        {
            SceneManager.LoadScene("PlayScene");
        }

    }

    private void UpdatePlayerUI()
    {
        if (SpawnLoadingUI != null)
        {

            Text[] texts = SpawnLoadingUI.GetComponentsInChildren<Text>();
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
        ChangeSkin();
    }

    private void ChangeSkin() 
    {
        Debug.Log("Chanage Color is " + SkinColor);

        switch (SkinColor) 
        {

            case eSkinColor.Red:
                CharacterRender.material.color = Color.red;
                break;
            case eSkinColor.Yellow:
                CharacterRender.material.color = Color.yellow;
                break;
            case eSkinColor.Purple:
                CharacterRender.material.color = new Color(255, 0, 255);
                break;
            case eSkinColor.Blue:
                CharacterRender.material.color = Color.blue;
                break;
            case eSkinColor.Green:
                CharacterRender.material.color = Color.green;
                break;
            case eSkinColor.White:
                CharacterRender.material.color = Color.white;
                break;
            default:
                break;

        }
    }

    void SetCharacterRender(GameObject NewCharacter) 
    {
        CharacterRender = NewCharacter.GetComponent<GetCharacterRenderScript>().GetRender();
        ChangeSkin();
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
