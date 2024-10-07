using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class TitleGameManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public enum Step
    {
        Title,
        Lobby,
        Loading,
        InGame
    }

    private Step gamestep = Step.Title;

    public GameObject LobbyCharacter;
    public GameObject LobbyUI;
    public GameObject LoadingCharacter;
    public GameObject LoadingUI;


    private GameObject SpawnLobbyCharacter = null;
    private GameObject SpawnLobbyUI = null;
    private GameObject SpawnLoadingCharacter = null;
    private GameObject SpawnLoadingUI = null;
    private int MaxClientCount = 2;
    private float GameStartTimer = 0.0f;
    private bool IsStartGame = false;

    public Vector3 LobbyCharacterSpawnPosition;
    public Vector3 LoadingCharacterSpawnPosition;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master");
    }


    // Update is called once per frame
    void Update()
    {
        if (IsStartGame) 
        {
            StartGame();
        }
    }

    public void PlayGameLogic()
    {
        switch (gamestep)
        {
            case Step.Title:
                break;

            case Step.Lobby:
                GameObject TUI = GameObject.Find("TitleUI");
                Destroy(TUI);

                JoinLobby();
                SpawnLobbyCharacter = Instantiate(LobbyCharacter, LobbyCharacterSpawnPosition, transform.rotation);
                SpawnLobbyUI = Instantiate(LobbyUI);
                Debug.Log("Step Change Lobby");
                break;

            case Step.Loading:
                Destroy(SpawnLobbyCharacter);
                Destroy(SpawnLobbyUI);

                SpawnLoadingCharacter = Instantiate(LoadingCharacter, LoadingCharacterSpawnPosition, transform.rotation);
                SpawnLoadingUI = Instantiate(LoadingUI);
                JoinGameRoom();
                break;

            case Step.InGame:
                IsStartGame = true;
                break;

            default:
                break;
        }
    }

    public void NextStep()
    {
        Debug.Log("Function call -> Next GameStep");
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
        Debug.Log("Join Lobby");
    }

    private void JoinGameRoom()
    {
        PhotonNetwork.JoinOrCreateRoom("RoomOne", new RoomOptions { MaxPlayers = MaxClientCount }, null);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Create Room");
        base.OnCreatedRoom();
        UpdatePlayerUI();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Join Room");
        base.OnJoinedRoom();
        UpdatePlayerUI();
    }

    private void StartGame()
    {

        GameStartTimer += Time.deltaTime;

        if (GameStartTimer > 8.0f)
        {
            Destroy(SpawnLoadingCharacter);
            Destroy(SpawnLoadingUI);
            SceneManager.LoadScene("PlayScene");
        }

    }

    private void UpdatePlayerUI()
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

        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount.ToString());
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

}
