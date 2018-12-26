using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ServerConnection : MonoBehaviour {
    
    public delegate void ReceiveCallback(byte[] buffer);
    public void SetReceiveCallBack(ReceiveCallback cb)
    {
        receiveCallback = cb;
    }

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

    private async Task CreateConnection()
    {
        /*
         * 꽤 오랜시간 찾아본 결과 비동기IO를 하는 여러가지 방법이 있는 것 같음. 
         * ManualResetEvent와 Begin~~() / End~~()함수를 이용해서 하는 방법도 있는 것 같은데
         * await과 ~~~Async함수를 사용해서 해봤음...
         * 우선 메소드 안에 await을 사용하려면 해당 함수의 수식어로 async가 꼭 들어가야 한다고 함..
         * 그래서 위에도 async를 썼는데 리턴값을 Task로 하는 코드가 많이 보이는데 void로 바꿔도 상관이 없걸랑
         * 뭔 차인가 궁금해서 검색해봤는데 일단 exception 발생 시에 차이가 존재하는 거 같음..
         * 여튼 아래에 await socket.ConnectAsync(ADDR, PORT) 함수를 보면
         * 저 줄이 실행이 되면 연결이 붙을 때까지 다른 걸 실행할 수 있게 한다고 함
         * 그리고 연결이 완료되면 await 아래줄부터 실행할 수 있도록 흐름이 돌아오나봄
         */
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
        await nStream.ReadAsync(buffer, 0, BUF_SIZE);
        receiveCallback(buffer);
    }

}
