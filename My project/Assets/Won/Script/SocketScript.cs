using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class SocketScript : MonoBehaviour
{
    public ChatUIScript ChatUIScr;
    private TcpClient client;
    private NetworkStream stream;
    private bool isConnected = false;

    void Start()
    {
        if (ConnectServer())
        {
            Debug.Log("Connect Chat Server");
            isConnected = true;
            ReceiveMessage();
        }
    }
    void Update()
    {
        if (!isConnected)
        {
            if (ConnectServer())
            {
                Debug.Log("Connected to chat server.");
                isConnected = true;
                ReceiveMessage();
            }
        }

    }

    private bool ConnectServer()
    {
        Int32 port = 7777;
        string host = "127.0.0.1";

        client = new TcpClient(host, port);
        stream = client.GetStream();

        if (stream == null)
        {
            return false;
        }

        return true;
    }

    public async void SendMessageToServer(string ChatMessage)
    {
        if (ChatMessage == "")
            return;

        byte[] dataToSend = Encoding.ASCII.GetBytes(ChatMessage);

        try
        {
            await stream.WriteAsync(dataToSend, 0, dataToSend.Length);
            Debug.Log("Message sent successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error sending message: {ex.Message}");
        }
    }
    private async void ReceiveMessage()
    {
        byte[] Buffer = new byte[1024];

        while (true)
        {
            int RecvCount = await stream.ReadAsync(Buffer, 0, Buffer.Length);

            if (RecvCount > 0)
            {
                string Message = Encoding.ASCII.GetString(Buffer, 0, RecvCount);
                string NickName = "";
                string text = "";

                for (int i = 0; i < Message.Length; ++i) 
                {
                    if(Message[i] == '9' && Message[i+1] == '5' && Message[i+2] == '0') 
                    {
                        for(int j = 0; j < i; ++j) 
                        {
                            NickName += Message[j];
                        }
                        for(int k = i + 3; k < Message.Length; ++k) 
                        {
                            text += Message[k];
                        }

                        ChatUIScr.AddText(NickName, text);
                        break;
                    }
                }
            }
            else
            {
                break;
            }

        }
    }

    public void SetChatUI(ChatUIScript NewChatUI) 
    {
        ChatUIScr = NewChatUI;
    }

    public void OnDisconnectedToChatServer()
    {
        client.Close();
    }

    public bool IsConnected()
    {
        return client.Connected;
    }
}
