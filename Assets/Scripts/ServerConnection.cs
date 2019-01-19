using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using System;
using Assets.Scripts.Connection;

public class ServerConnection : MonoBehaviour
{
    public delegate void ReceiveCallback(byte[] buffer, int readBytes);

    private const string ADDR = "127.0.0.1";
    private const int PORT = 9910;
    private TcpClient socket;
    private NetworkStream nStream;
    private ReceiveCallback receiveCallback;
    private static ServerConnection instance;
    private bool isEnd;

    public void SetReceiveCallBack(ReceiveCallback cb)
    {
        receiveCallback += cb;
    }

    public async Task SendMessage(byte[] msg, int size)
    {
        if (nStream == null)
            return;

        try
        {
            await nStream.WriteAsync(msg, 0, size);
        }
        catch (InvalidOperationException ioe)
        {
            Debug.Log(ioe.Message);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
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
        if (nStream != null)
            nStream.Close();
        if (socket != null)
            socket.Close();
    }

    private async Task CreateConnection()
    {
        socket = new TcpClient();
        //await socket.ConnectAsync(ADDR, PORT);
        await socket.ConnectAsync(ServerInfo.IP, Int32.Parse(ServerInfo.Port));
        nStream = socket.GetStream();
        StartListeningThread();
    }

    private async Task StartListeningThread()
    {
        const int BUF_SIZE = 1024;
        byte[] buffer = new byte[BUF_SIZE];

        while (!isEnd)
        {
            int readBytes = await nStream.ReadAsync(buffer, 0, BUF_SIZE);
            receiveCallback(buffer, readBytes);
            Array.Clear(buffer, 0, BUF_SIZE);
        }
    }
}