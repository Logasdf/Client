using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using UnityEngine;

public class ServerConnection : MonoBehaviour {
    
    public delegate void LobbyUIMgrCallBack();
    public void SetLobbyUIMgrCallBack(LobbyUIMgrCallBack cb)
    {
        lobbyUIMgrCallBack = cb;
    }

    private const string ADDR = "127.0.0.1";
    private const int PORT = 9910;
    private TcpClient clientSocket;
    private NetworkStream theStream;
    private LobbyUIMgrCallBack lobbyUIMgrCallBack;

    private void Start()
    {
        CreateConnection();
    }

    private void CreateConnection()
    {
        //clientSocket = new TcpClient(ADDR, PORT);
        StartListeningThread();
        lobbyUIMgrCallBack(); // test
    }

    private void StartListeningThread()
    {
        
    }

}
