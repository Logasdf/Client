using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Net;

public class ServerConnection : MonoBehaviour {
    
    // Callback method declaration...
    public delegate void ReceiveCallback(byte[] buffer, int readBytes);

    // Setting Callback method 
    public void SetReceiveCallBack(ReceiveCallback cb)
    {
        //Debug.Log("SetReceiveCallBack....");
        receiveCallback += cb;
    }

    // asynchronously send a message to the server
    public async Task SendMessage(byte[] msg, int size)
    {
        if (nStream == null)
            return;

        await nStream.WriteAsync(msg, 0, size);
        Debug.Log("Send Completed, size : " + size);
    }
    
    private const string ADDR = "127.0.0.1";
    private const int PORT = 9910;
    private TcpClient socket;
    private NetworkStream nStream;
    private ReceiveCallback receiveCallback;
    private static ServerConnection instance;

    private bool isEnd;
    private void Awake()
    {
        //Debug.Log(this.ToString() + " Awake()");
    }

    private void Start()
    {
        //Debug.Log("This is a ServerConnection's Start()");
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        isEnd = false;
        instance = this;
        DontDestroyOnLoad(gameObject);
        // Try to Connect to the sever...
        CreateConnection();
    }

    private void OnDestroy()
    {
        Debug.Log("ServerConnection OnDestroy()");
        isEnd = true;
        nStream.Close();
        //socket.Close();
    }

    private async Task CreateConnection()
    {
        socket = new TcpClient();
        await socket.ConnectAsync(ADDR, PORT);
        Debug.Log("connect completed");
        nStream = socket.GetStream();
        Debug.Log("stream setting completed");
        StartListeningThread();
    }

    private async Task StartListeningThread()
    {
        //test
        const int BUF_SIZE = 2048;
        byte[] buffer = new byte[BUF_SIZE];
        Debug.Log("Recv Start....");
        while(!isEnd)
        {
            int readBytes = await nStream.ReadAsync(buffer, 0, BUF_SIZE);
            Debug.Log(string.Format("Read Bytes From Server: {0}", readBytes));
            receiveCallback(buffer, readBytes);
            Array.Clear(buffer, 0, BUF_SIZE);
        }
    }
}
