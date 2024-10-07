using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject PlayerCharacter;
    public Vector3 SpawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Waiting for Connect....");

        //PhotonNetwork.ConnectUsingSettings();
        //Photon Server에 접속하는 함수
    }

/*    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Connected Server");
        PhotonNetwork.JoinLobby();
        //Lobby에 접속
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        PhotonNetwork.JoinOrCreateRoom("Test", null, null);
        //Lobby에 있는 방 중 특정 방으로 접속 || 없다면 생성
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        GameObject Client = PhotonNetwork.Instantiate(PlayerCharacter.name, SpawnPosition, Quaternion.identity);
        Client.GetComponent<PlayerSetup>().IsLocalPlayer();
    }*/
}
