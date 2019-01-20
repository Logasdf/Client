using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Google.Protobuf;
using Assets.Scripts;

public class PacketManager : MonoBehaviour
{
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

        if (protoObj == null)
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
        connection = GameObject.Find("Connection").GetComponent<ServerConnection>();
        connection.SetReceiveCallBack(UnpackProcess);
    }

    private void UnpackProcess(byte[] buffer, int readBytes)
    {
        bool hasBody, hasMore = true;
        int type, length, start = 0;

        while (hasMore)
        {
            type = length = 0;
            hasBody = UnpackHeader(buffer, start, ref type, ref length);
            start += 8;
            if (hasBody)
            {
                UnpackMessage(buffer, start, type, length);
                start += length;
            }

            hasMore = (readBytes > start) ? true : false;
        }
    }

    private bool UnpackHeader(byte[] buffer, int start, ref int type, ref int length)
    {
        CodedInputStream cis = new CodedInputStream(buffer, start, 8);
        type = (int)cis.ReadFixed32();
        length = (int)cis.ReadFixed32();

        if (length != 0)
            return true;

        handleMessage(type, typeof(int));
        return false;
    }

    private void UnpackMessage(byte[] buffer, int start, int type, int length)
    {
        object body = null;
        try
        {
            body = DeserializeMessageBody(buffer, start, length, MessageType.invTypeTable[type]);
            handleMessage(body, MessageType.invTypeTable[type]);
        }
        catch (KeyNotFoundException knfe)
        {
            Debug.Log(string.Format("{0}/{1}", knfe.Message, type));
            return;
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