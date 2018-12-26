using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

public class PacketManager : MonoBehaviour {

    public void SerializeAndSend(IMessage protoObj)
    {
        int type = typeTable[protoObj.GetType()];
        int byteLength = protoObj.CalculateSize();

        Debug.Log(type);
        Debug.Log(byteLength);
        cos.WriteInt32(type);
        cos.WriteInt32(byteLength);
        protoObj.WriteTo(cos);

        connection.SendMessage(sendBuffer, sendBuffer.Length);
    }

    private const int BUF_SIZE = 2048;
    private static PacketManager instance;
    private Dictionary<Type, int> typeTable;
    private ServerConnection connection;
    private CodedOutputStream cos;
    private byte[] sendBuffer;

    private void Start()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        AttachToServerAsDelegate();
        typeTable = new Dictionary<Type, int>();
        //********* DICTIONARY INITIALIZE NEEDED *********
        
        sendBuffer = new byte[BUF_SIZE];
        cos = new CodedOutputStream(sendBuffer);
    }

    private void AttachToServerAsDelegate()
    {
        //콜백함수 등록
        connection = GameObject.Find("Connection").GetComponent<ServerConnection>();
        connection.SetReceiveCallBack(UnpackMessage);
    }

    private void UnpackMessage(byte[] buffer)
    {   //test
        //메시지가 수신되었을 때 실행되는 콜백함수 
        int type = BitConverter.ToInt32(buffer, 0);
        int length = BitConverter.ToInt32(buffer, 4);
        
    }
}
