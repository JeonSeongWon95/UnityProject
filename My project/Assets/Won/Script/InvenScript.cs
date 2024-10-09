using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenScript : MonoBehaviour
{
    public GameObject GameManager;

    private TitleGameManagerScript TitleGameManagerScr;
    void Start()
    {
        TitleGameManagerScr = GameManager.GetComponent<TitleGameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickedRedSkin()
    {
        TitleGameManagerScr.SetNewSkin(TitleGameManagerScript.eSkinColor.Red);
    }
    public void OnClickedGreenSkin()
    {
        TitleGameManagerScr.SetNewSkin(TitleGameManagerScript.eSkinColor.Green);
    }
    public void OnClickedBlueSkin()
    {
        TitleGameManagerScr.SetNewSkin(TitleGameManagerScript.eSkinColor.Blue);
    }
    public void OnClickedWhiteSkin()
    {
        TitleGameManagerScr.SetNewSkin(TitleGameManagerScript.eSkinColor.White);
    }
    public void OnClickedYellowSkin()
    {
        TitleGameManagerScr.SetNewSkin(TitleGameManagerScript.eSkinColor.Yellow);
    }
    public void OnClickedPurpleSkin()
    {
        TitleGameManagerScr.SetNewSkin(TitleGameManagerScript.eSkinColor.Purple);
    }
    public void OnClickedExitButton() 
    {
        Destroy(gameObject);
    }
}
