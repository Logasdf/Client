using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Google.Protobuf;
using Google.Protobuf.Packet.Room;
using Assets.Scripts;

public class PacketManager : MonoBehaviour {

    public delegate void HandleMessage(object obj, Type type);


    private const int BUF_SIZE = 2048;
    private static PacketManager instance;
    public PacketManager Instance { get { return instance; } }
    private ServerConnection connection;
    private CodedOutputStream cos;
    private byte[] sendBuffer;
    private HandleMessage handleMessage;


    public void SetHandleMessage(HandleMessage hm)
    {
        this.handleMessage = hm;
    }

    public void PackMessage(int type = -1, IMessage protoObj = null)
    {
        ClearBuffer();
        cos = new CodedOutputStream(sendBuffer);

        if(protoObj == null)
        {
            cos.WriteFixed32((uint)type);
            cos.WriteFixed32(0);
        }
        else
        {
            SerializeMessageBody(cos, protoObj);
        }
        connection.SendMessage(sendBuffer, (int)cos.Position);
    }

    private void SerializeMessageBody(CodedOutputStream cos, IMessage protoObj)
    {
        int type = MessageType.typeTable[protoObj.GetType()];
        int byteLength = protoObj.CalculateSize();

        cos.WriteFixed32((uint)type);
        cos.WriteFixed32((uint)byteLength);
        protoObj.WriteTo(cos);
    }

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        AttachToServerAsDelegate();
        sendBuffer = new byte[BUF_SIZE];
    }

    private void ClearBuffer()
    {
        Array.Clear(sendBuffer, 0, sendBuffer.Length);
    }

    private void AttachToServerAsDelegate()
    {
        //콜백함수 등록
        connection = GameObject.Find("Connection").GetComponent<ServerConnection>();
        connection.SetReceiveCallBack(UnpackMessage);
    }

    private void UnpackMessage(byte[] buffer, int readBytes)
    {   
        CodedInputStream cis = new CodedInputStream(buffer, 0, 8);

        int type = (int)cis.ReadFixed32();
        int length = (int)cis.ReadFixed32();
        object body = null;
        bool hasMore = readBytes > 8 + length ? true : false;

        if(length == 0)
        {
            handleMessage(type, typeof(int));
        }
        else
        {
            try
            {
                body = DeserializeMessageBody(buffer, 8, length, MessageType.invTypeTable[type]);
                handleMessage(body, MessageType.invTypeTable[type]);
            }
            catch (KeyNotFoundException knfe)
            {
                Debug.Log(string.Format("{0}/{1}/{2}", knfe.Message, type, readBytes));
                return;
            }
        }
        
        if(hasMore)
        {
            Debug.Log("HasMore!!");
            int size = readBytes - (8 + length);
            byte[] tempArr = new byte[size];
            Array.Copy(buffer, 8 + length, tempArr, 0, size);
            UnpackMessage(tempArr, size);
        }
    }

    private object DeserializeMessageBody(byte[] buffer, int start, int length, Type type)
    {
        object obj = Activator.CreateInstance(type);
        try
        {
            CodedInputStream cis = new CodedInputStream(buffer, start, length);
            MethodInfo parseMethod = type.GetMethod("MergeFrom", new Type[] { typeof(CodedInputStream) });
            parseMethod.Invoke(obj, new object[] { cis });
            return obj;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return null;
        }
    }
}
