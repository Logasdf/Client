using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using UnityEngine;

public class ServerConnection : MonoBehaviour {
    
    public delegate void ReceiveCallback();
    public void SetReceiveCallBack(ReceiveCallback cb)
    {
        receiveCallback = cb;
    }

    private const string ADDR = "127.0.0.1";
    private const int PORT = 9910;
    private TcpClient clientSocket;
    private NetworkStream theStream;
    private ReceiveCallback receiveCallback;
    private static ServerConnection instance;

    private void Start()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        CreateConnection();
    }

    private void CreateConnection()
    {
        //clientSocket = new TcpClient(ADDR, PORT);
        StartListeningThread();
        //TODO : send the request to get the room list.
        
    }

    private void StartListeningThread()
    {
        receiveCallback();
    }

}
