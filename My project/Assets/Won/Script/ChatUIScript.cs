using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class ChatUIScript : MonoBehaviourPunCallbacks
{
    public InputField inputField;
    public GameObject ChatBox;
    public GameObject TextBox;
    public SocketScript socketScript;
    private bool IsActiveChat = false;

    void Start()
    {

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!IsActiveChat)
            {
                InputMessage();
            }
            else
            {
                SendMessage();
            }
        }
    }

    void SendMessage()
    {
        string Message = PhotonNetwork.NickName;
        Message += "950";
        Message += inputField.text;
        socketScript.SendMessage(Message);
        IsActiveChat = false;
        inputField.DeactivateInputField();
    }
    void InputMessage()
    {
        inputField.ActivateInputField();
        IsActiveChat = true;
    }
    public void AddText(string Nickname, string Massge)
    {
        GameObject CreateTextBox = Instantiate(TextBox);
        Text FindTextBox =  CreateTextBox.GetComponent<Text>();
        FindTextBox.text = Nickname + " : " + Massge;
        CreateTextBox.transform.SetParent(ChatBox.transform);
    }
    public void SetSocketScript(SocketScript NewsocketScript) 
    {
        socketScript = NewsocketScript;
    }
}
