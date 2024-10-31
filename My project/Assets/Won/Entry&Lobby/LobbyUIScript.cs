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
    public GameObject OptionUI;
    public GameObject TitleGameManager;

    public void OnClickPlay() 
    {
        TitleGameManagerScript TGM = GameObject.Find("GameManager").GetComponent<TitleGameManagerScript>();
        TGM.NextStep();
    }
    public void OnClickChange()
    {
        if (!Inventory.activeSelf)
        {
            Inventory.SetActive(true);
        }
    }
    public void OnClickOption()
    {
        if (!OptionUI.activeSelf)
        {
            OptionUI.SetActive(true);
        }
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
        PlayerName.text = PhotonNetwork.NickName;
    }
}
