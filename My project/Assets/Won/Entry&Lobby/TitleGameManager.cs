using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleGameManager : MonoBehaviour
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

    public Vector3 LobbyCharacterSpawnPosition;
    public Vector3 LoadingCharacterSpawnPosition;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

                SpawnLobbyCharacter = Instantiate(LobbyCharacter, LobbyCharacterSpawnPosition, transform.rotation);
                SpawnLobbyUI = Instantiate(LobbyUI);
                Debug.Log("Step Change Lobby");
                break;

            case Step.Loading:
                Destroy(SpawnLobbyCharacter);
                Destroy(SpawnLobbyUI);

                SpawnLoadingCharacter = Instantiate(LoadingCharacter, LoadingCharacterSpawnPosition, transform.rotation);
                SpawnLoadingUI = Instantiate(LoadingUI);
                break;

            case Step.InGame:
                Destroy(SpawnLoadingCharacter);
                Destroy(SpawnLoadingUI);
                SceneManager.LoadScene("PlayScene");
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

}
