using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManagerScript : MonoBehaviourPunCallbacks
{
    public bool IsGameEnd = false;
    public GameObject Character;
    public GameObject[] StartPositions;

    private float GameEndTimer = 0.0f;
 
    void Start()
    {
        PhotonNetwork.Instantiate("Player", StartPositions[0].transform.position, StartPositions[0].transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGameEnd) 
        {
            Debug.Log("GameEnd");
            GameEndTimer += Time.deltaTime;

            if (GameEndTimer > 5.0f) 
            {
                GameEndTimer = 0.0f;
                SceneManager.LoadScene("EndScene");
            }
        }
    }


}
