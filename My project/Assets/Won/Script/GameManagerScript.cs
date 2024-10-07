using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public bool IsGameEnd = false;

    private float GameEndTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
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
