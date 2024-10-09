using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon.StructWrapping;

public class SetNickNameScript : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public Text PlayerName;
    public Renderer CharacterRender;
    public enum eSkinColor
    {
        Red,
        Yellow,
        Purple,
        Blue,
        Green,
        White
    }

    private eSkinColor SkinColor;

    void Start()
    {
        SetName();
        SetCharacterSkin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SetName() 
    {
        PlayerName.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
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

        properties.TryGetValue("Skin",out SkinColor);
        ChangeSkin();
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

    public void SetText(Text NewName) 
    {
        PlayerName = NewName;
        SetName();
    }
}
