using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using UnityEngine;

public class ServerConnection : MonoBehaviour {

    public static ServerConnection GetInstance()
    {
        // not thread-safe yet.
        // and not sure if it should be a singleton class
        // Should have a look at the "DontDestroyOnLoad" function.
        if (instance == null)
        {
            instance = new ServerConnection();
        }

        return instance;
    }

    private void Awake()
    {
        CreateConnection();
        WriteToServer(); //test
        StartListeningThread();
    }

    //test
    public void WriteToServer()
    {
        string msg = "abcde\0";
        theStream = clientSocket.GetStream();
        StreamWriter writer = new StreamWriter(theStream);
        writer.Write(msg);
        writer.Flush();
    }

    private const string ADDR = "127.0.0.1";
    private const int PORT = 9910;
    private static ServerConnection instance;
    private TcpClient clientSocket;
    private NetworkStream theStream;

    private ServerConnection() { }


    private void CreateConnection()
    {
        clientSocket = new TcpClient(ADDR, PORT);
        StartListeningThread();
    }

    private void StartListeningThread()
    {
        
    }

}
