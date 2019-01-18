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
    
    public delegate bool UnpackHeader(byte[] buffer, ref int type, ref int length);
    public delegate void UnpackMessage(byte[] buffer, int type, int length);

    private const string ADDR = "127.0.0.1";
    private const int PORT = 9910;
    private TcpClient socket;
    private NetworkStream nStream;
    private UnpackHeader unpackHeader;
    private UnpackMessage unpackMessage;
    private static ServerConnection instance;
    private bool isEnd;

    public void SetUnpackFunctions(UnpackHeader headerFunc, UnpackMessage messageFunc)
    {
        unpackHeader = headerFunc;
        unpackMessage = messageFunc;
    }

    public async Task SendMessage(byte[] msg, int size)
    {
        if (nStream == null)
            return;

        await nStream.WriteAsync(msg, 0, size);
    }
   
    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        isEnd = false;
        instance = this;
        DontDestroyOnLoad(gameObject);
        CreateConnection();
    }

    private void OnDestroy()
    {
        Debug.Log("ServerConnection OnDestroy()");
        isEnd = true;
        if(nStream != null)
            nStream.Close();
        if(socket != null)
            socket.Close();
    }

    private async Task CreateConnection()
    {
        socket = new TcpClient();
        await socket.ConnectAsync(ADDR, PORT);
        nStream = socket.GetStream();
        StartListeningThread();
    }

    private async Task StartListeningThread()
    {
        byte[] buffer;
        int type, length;
        const int BUF_SIZE = 1024;
        while(!isEnd)
        {
            buffer = new byte[BUF_SIZE];
            await nStream.ReadAsync(buffer, 0, 8);
            type = length = 0;
            if (unpackHeader(buffer, ref type, ref length))
            {
                await nStream.ReadAsync(buffer, 8, length);
                unpackMessage(buffer, type, length);
            };
        }
    }
}
