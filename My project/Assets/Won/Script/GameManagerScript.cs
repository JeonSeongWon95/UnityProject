using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using static TitleGameManagerScript;
using static PlayGameManagerScript;
using ExitGames.Client.Photon.StructWrapping;

public class PlayGameManagerScript : MonoBehaviourPunCallbacks
{
    public bool IsGameEnd = false;
    public Vector3 GoalPosition;
    public GameObject GameEndUI;
    public Renderer CharacterRender = null;

    private float GameEndTimer = 0.0f;

    public enum eSkinColor
    {
        Red,
        Yellow,
        Purple,
        Blue,
        Green,
        White
    }

    private GameObject Player;
    private eSkinColor SkinColor;

    void Start()
    {
        Player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.Euler(0, 90, 0));
        PlayerScript PlayerScr = Player.GetComponent<PlayerScript>();
        PlayerScr.enabled = true;
        PlayerScr.LocalPlayerSet();
        SetCharacterRender(Player);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Goal", GoalPosition, Quaternion.identity);
        }

    }

    // Update is called once per frame
    void Update()
    {
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

    void SetCharacterRender(GameObject NewCharacter)
    {
        Debug.Log("Function Call -> SetCharacterRender");
        CharacterRender = NewCharacter.GetComponent<GetCharacterRenderScript>().GetRender();

        if(CharacterRender == null) 
        {
            Debug.Log("CharacterRender is Null");
        }

        ExitGames.Client.Photon.Hashtable properties = PhotonNetwork.LocalPlayer.CustomProperties;
        properties.TryGetValue("Skin", out SkinColor);

        Debug.Log("SkinColor is " + SkinColor.ToString());
        ChangeSkin();
    }

    private void ChangeSkin()
    {
        Debug.Log("Present Color is " + SkinColor);

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
