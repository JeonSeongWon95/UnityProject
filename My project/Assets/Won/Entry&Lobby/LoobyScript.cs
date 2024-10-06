using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoobyScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickPlay() 
    {
        Debug.Log("Play Click!");
        
    }
    public void OnClickChange()
    {
        Debug.Log("Change Click!");
    }
    public void OnClickOption()
    {
        Debug.Log("Option Click!");
    }
    public void OnClickMoveRight()
    {
        Debug.Log("MoveRight Click!");
    }
    public void OnClickMoveLeft()
    {
        Debug.Log("MoveLeft Click!");
    }

    public void OnClickExit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
