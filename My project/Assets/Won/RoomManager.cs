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
        //Photon Server�� �����ϴ� �Լ�
    }

/*    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Connected Server");
        PhotonNetwork.JoinLobby();
        //Lobby�� ����
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        PhotonNetwork.JoinOrCreateRoom("Test", null, null);
        //Lobby�� �ִ� �� �� Ư�� ������ ���� || ���ٸ� ����
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        GameObject Client = PhotonNetwork.Instantiate(PlayerCharacter.name, SpawnPosition, Quaternion.identity);
        Client.GetComponent<PlayerSetup>().IsLocalPlayer();
    }*/
}
