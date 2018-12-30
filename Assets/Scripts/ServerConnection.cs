using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ServerConnection : MonoBehaviour {
    
    // Callback method declaration...
    public delegate void ReceiveCallback(byte[] buffer);

    // Setting Callback method 
    public void SetReceiveCallBack(ReceiveCallback cb)
    {
        //Debug.Log("SetReceiveCallBack....");
        receiveCallback += cb;

        //Debug.Log(string.Format("the num of callback methods: {0}", receiveCallback.GetInvocationList().GetLength(0)));
    }

    // asynchronously send a message to the server
    public async Task SendMessage(byte[] msg, int size)
    {
        if (nStream == null)
            return;

        await nStream.WriteAsync(msg, 0, size);
        Debug.Log("Send Completed");
    }
    
    private const string ADDR = "127.0.0.1";
    private const int PORT = 9910;
    private TcpClient socket;
    private NetworkStream nStream;
    private ReceiveCallback receiveCallback;
    private static ServerConnection instance;

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

        instance = this;
        DontDestroyOnLoad(gameObject);
        // Try to Connect to the sever...
        CreateConnection();
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
        await nStream.ReadAsync(buffer, 0, BUF_SIZE);
        Debug.Log("Recv End....");
        receiveCallback(buffer);
    }

}
