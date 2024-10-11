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
    void Start()
    {
        if (ConnectServer())
        {
            Debug.Log("Connect Chat Server");
            ReceiveMassage();
        }
    }
    void Update()
    {

    }

    private bool ConnectServer()
    {
        Int32 port = 7777;
        string host = "127.0.0.1";

        client = new TcpClient(host, port);
        Socket socket = client.Client;
        socket.Blocking = false;
        stream = client.GetStream();

        if (stream == null)
        {
            return false;
        }

        return true;
    }

    public async void SendMassage(string Massage)
    {
        byte[] dataToSend = Encoding.ASCII.GetBytes(Massage);
        await stream.WriteAsync(dataToSend, 0, dataToSend.Length);
    }
    private async void ReceiveMassage()
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

                for (int i = 0; i< Message.Length; ++i) 
                {
                    if(Message[i] == '9' && Message[i+1] == '5' && Message[i+2] == '0') 
                    {
                        for(int j = 0; j < Message[i - 1]; ++j) 
                        {
                            NickName += Message[j];
                        }
                        for(int k = i + 2; k < Message.Length; ++k) 
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
}
