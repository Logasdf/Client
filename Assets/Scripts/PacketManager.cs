using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Google.Protobuf;
using Assets.Scripts;

public class PacketManager : MonoBehaviour
{
    public delegate void HandleMessage(object obj, Type type);
    public PacketManager Instance { get { return instance; } }

    private static PacketManager instance;
    private ServerConnection connection;
    private HandleMessage handleMessage;
    private byte[] sendBuffer;
    private byte[] backupBuffer;
    private CodedOutputStream cos;
    private const int SENDBUF_SIZE = 4096;
    private const int BACKUPBUF_SIZE = 512;
    private bool backupBufferHasData;
    private int backupBufferCurIdx;

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
        sendBuffer = new byte[SENDBUF_SIZE];
        backupBuffer = new byte[BACKUPBUF_SIZE];
        backupBufferHasData = false;
        backupBufferCurIdx = 0;
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
        int type = 0, length = 0, start = 0, remainBytes = readBytes;
        
        if(backupBufferHasData)
        {
            if(backupBufferCurIdx < 8) //헤더 정보조차 다 오지 않은 상태를 백업해둔 경우
            {
                int preferredAmount = 8 - backupBufferCurIdx;
                Array.Copy(buffer, 0, backupBuffer, backupBufferCurIdx, preferredAmount);

                backupBufferCurIdx += preferredAmount;
                start += preferredAmount;

                hasBody = UnpackHeader(backupBuffer, 0, ref type, ref length);
                if(hasBody)
                {
                    UnpackMessage(buffer, start, type, length);
                    start += length;
                }
            }
            else //헤더 이상의 정보가 있음
            {
                UnpackHeader(backupBuffer, 0, ref type, ref length); // 무조건 있어야함 바디가, 없으면 들어있을 이유 X
                //부족한 byte수를 계산해야함
                int preferredAmount = length - (backupBufferCurIdx - 8);
                Array.Copy(buffer, 0, backupBuffer, backupBufferCurIdx, preferredAmount);
                UnpackMessage(backupBuffer, 8, type, length);
                backupBufferCurIdx += preferredAmount;
                start += preferredAmount;
            }
            Array.Clear(backupBuffer, 0, backupBufferCurIdx);
            backupBufferCurIdx = 0;
            backupBufferHasData = false;
        }

        while (hasMore)
        {
            type = length = 0;
            if(remainBytes < 8) // 헤더도 못읽을 경우에 현재 인덱스부터 remainBytes만큼 백업버퍼에 저장
            {
                Array.Copy(buffer, start, backupBuffer, backupBufferCurIdx, remainBytes);
                backupBufferCurIdx += remainBytes;
                backupBufferHasData = true;
                return;
            }
            
            hasBody = UnpackHeader(buffer, start, ref type, ref length);
            remainBytes -= 8;
            start += 8;

            if (hasBody)
            {
                if(remainBytes < length)
                {
                    start -= 8;
                    remainBytes += 8;
                    Array.Copy(buffer, start, backupBuffer, backupBufferCurIdx, remainBytes);
                    backupBufferCurIdx += remainBytes;
                    backupBufferHasData = true;
                    return;
                }
                UnpackMessage(buffer, start, type, length);
                start += length;
                remainBytes -= length;
            }
            hasMore = remainBytes > 0 ? true : false;
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