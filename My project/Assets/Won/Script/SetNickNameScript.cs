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

    void Start()
    {
        SetName();
    }
    void SetName() 
    {
        PlayerName.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
    }
}
