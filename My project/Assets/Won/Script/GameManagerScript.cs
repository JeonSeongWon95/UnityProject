using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManagerScript : MonoBehaviourPunCallbacks
{
    public bool IsGameEnd = false;
    public Vector3 GoalPosition;
    public GameObject GameEndUI;
    private float GameEndTimer = 0.0f;

    GameObject Player;
 
    void Start()
    {
        Player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.Euler(0, 90, 0));
        PlayerScript PlayerScr = Player.GetComponent<PlayerScript>();
        PlayerScr.enabled = true;
        PlayerScr.LocalPlayerSet();

        if(PhotonNetwork.IsMasterClient)
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


}
