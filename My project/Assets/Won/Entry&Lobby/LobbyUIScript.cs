using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyUIScript : MonoBehaviourPunCallbacks
{
    public Text PlayerName;
    public GameObject Inventory;
    public GameObject TitleGameManager;

    private GameObject SpawnInventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickPlay() 
    {
        TitleGameManagerScript TGM = GameObject.Find("GameManager").GetComponent<TitleGameManagerScript>();
        TGM.NextStep();
    }
    public void OnClickChange()
    {
        SpawnInventory = Instantiate(Inventory);
        SpawnInventory.GetComponent<InvenScript>().GameManager = TitleGameManager;
    }
    public void OnClickOption()
    {
        Debug.Log("Option Click!");
    }
    public void OnClickMoveRight()
    {
        Debug.Log("MoveRight Click!");
    }
    public void OnClickMoveLeft()
    {
        Debug.Log("MoveLeft Click!");
    }

    public void OnClickExit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void LoadName() 
    {
        Debug.Log("Function Start LoadName");
        Debug.Log("PhotonNickName is " + PhotonNetwork.NickName);
        PlayerName.text = PhotonNetwork.NickName;
    }
}
