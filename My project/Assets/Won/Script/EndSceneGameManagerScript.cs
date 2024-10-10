using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Reflection;
using ExitGames.Client.Photon.StructWrapping;

public class EndSceneGameManagerScript : MonoBehaviourPunCallbacks
{
    public enum eSkinColor
    {
        Red,
        Yellow,
        Purple,
        Blue,
        Green,
        White
    }

    private float GameEndTimer;
    private GameObject SpawnEndHUD;
    private Player Winner;
    private eSkinColor SkinColor;

    public Renderer CharacterRender;
    public GameObject EndHUD;

    void Start()
    {
        SpawnEndHUD = Instantiate(EndHUD);
        FindWinnerPlayer();
    }
    void Update()
    {
        GameEndTimer += Time.deltaTime;

        if (GameEndTimer > 8.0f)
        {
            GameEndTimer = 0.0f;
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("Title");
        }
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
            SetWinnerNickNameScript SWNN = SpawnEndHUD.GetComponent<SetWinnerNickNameScript>();
            SWNN.SetName(Winner.NickName);

            ExitGames.Client.Photon.Hashtable properties;
            properties = Winner.CustomProperties;
            properties.TryGetValue("Skin", out SkinColor);
            ChangeSkin();

        }
    }
    private void ChangeSkin()
    {
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

}
