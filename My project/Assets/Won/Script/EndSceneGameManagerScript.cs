using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using System.Reflection;
using ExitGames.Client.Photon.StructWrapping;

public class EndSceneGameManagerScript : GameManager
{
    private Player Winner;
    public GameObject EndHUD;

    void Start()
    {
        EndHUD.SetActive(true);
        FindWinnerPlayer();
        StartCoroutine(GameEnd());
    }

    IEnumerator GameEnd() 
    {
        yield return new WaitForSeconds(8.0f);

        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Title");
    }

    void FindWinnerPlayer()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("IsWinner") && (bool)player.CustomProperties["IsWinner"])
            {
                Winner = player;
                break;
            }
        }

        if (Winner != null)
        {
            SetWinnerNickNameScript SWNN = EndHUD.GetComponent<SetWinnerNickNameScript>();
            SWNN.SetName(Winner.NickName);

            ExitGames.Client.Photon.Hashtable properties;
            properties = Winner.CustomProperties;
            properties.TryGetValue("Skin", out SkinColor);
            ChangeSkin();

        }
    }

}
