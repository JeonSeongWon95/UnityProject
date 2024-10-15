using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Unity.VisualScripting;
using NUnit.Framework;
using System;

public class ChatUIScript : MonoBehaviourPunCallbacks
{
    public InputField InputField_Message;
    public GameObject ChatBox;
    public GameObject TextBox;
    private SocketScript socketScr;
    private PlayerScript PlayerScr;
    private bool IsActiveChat = false;

    void Start()
    {

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Press Enter Key");

            if (!IsActiveChat)
            {
                PlayerScr.SetIsChatActive(true);
                Debug.Log("Is Active Chat False");
                InputMessage();
            }
            else
            {
                PlayerScr.SetIsChatActive(false);
                Debug.Log("Is Active Chat true");
                SendMessage();
            }
        }
    }

    void SendMessage()
    {
        Debug.Log("Send Message Active");
        string Message = PhotonNetwork.NickName;
        Message += "950";
        Message += InputField_Message.text;
        socketScr.SendMessageToServer(Message);
        IsActiveChat = false;
        InputField_Message.text = "";
        InputField_Message.DeactivateInputField();
    }
    void InputMessage()
    {
        Debug.Log("Input Message Active");
        InputField_Message.ActivateInputField();
        IsActiveChat = true;
    }
    public void AddText(string Nickname, string Massge)
    {
        Debug.Log("Add Message Active");
        GameObject CreateTextBox = Instantiate(TextBox);
        Text FindTextBox =  CreateTextBox.GetComponent<Text>();
        FindTextBox.text = Nickname + " : " + Massge;
        CreateTextBox.transform.SetParent(ChatBox.transform);
    }
    public void SetSocketScript(SocketScript NewsocketScript) 
    {
        socketScr = NewsocketScript;
    }

    public void SetPlayerScript(PlayerScript NewplayerScript)
    {
        PlayerScr = NewplayerScript;
    }
}
