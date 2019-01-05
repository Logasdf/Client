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
    private ServerConnection connection;
    private CodedOutputStream cos;
    private byte[] sendBuffer;
    // UI Manager Component(들)에게 Data를 넘기기위한 Delegate
    private HandleMessage handleMessage;


    public void SetHandleMessage(HandleMessage hm)
    {
        this.handleMessage = hm;
    }

    public void PackMessage(int type = -1, IMessage protoObj = null)
    {
        Debug.Log("PackMessage Callback Method");
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
        Debug.Log("Serialize Message Body!!");

        int type = MessageType.typeTable[protoObj.GetType()];
        int byteLength = protoObj.CalculateSize();

        cos.WriteFixed32((uint)type);
        cos.WriteFixed32((uint)byteLength);
        protoObj.WriteTo(cos);
    }
  
    private void Awake()
    {
        //Debug.Log(this.ToString() + " Awake()");
    }

    private void Start()
    {
        //Debug.Log("This is a PacketManager's Start()");
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        AttachToServerAsDelegate();
        //********* DICTIONARY INITIALIZE NEEDED *********

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

    private void UnpackMessage(byte[] buffer)
    {   
        //메시지가 수신되었을 때 실행되는 콜백함수 
        Debug.Log("UnpackMessage Callback Method");

        CodedInputStream cis = new CodedInputStream(buffer, 0, 8);
        //int type = BitConverter.ToInt32(buffer, 0);
        //int length = BitConverter.ToInt32(buffer, 4);
        int type = (int)cis.ReadFixed32();
        int length = (int)cis.ReadFixed32();
        object body = null;

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
                Debug.Log(knfe.Message);
                return;
            }
        }
    }

    private object DeserializeMessageBody(byte[] buffer, int start, int length, Type type)
    {
        Debug.Log("Deserialize Message Body Start!");
        // Type 객체에 맞게 Instance 생성해주는 함수.
        // 예를 들어 Type 객체가 RoomList의 Type일 경우, RoomList객체가 생성되는 것.
        // 아래 코드에서는 object로 반환받은 이유는 동적으로 Type casting할 방법이 없어서임.
        object obj = Activator.CreateInstance(type);
        try
        {
            CodedInputStream cis = new CodedInputStream(buffer, start, length);
            // 해당 Type의 method 중에 "MergeFrom"이면서, CodedInputStream을 파라미터로 받는 method을 찾는다.
            // .proto로 생성된 Class type들은 모두 MergeFrom(CodedInputStream)가 구현되어 있기 때문에
            // .proto로 생성된 Class type은 굳이 type casting하지 않고도 파싱을 할 수 있음.
            MethodInfo parseMethod = type.GetMethod("MergeFrom", new Type[] { typeof(CodedInputStream) });
            // 찾은 메소드를 실제 실행시키는 코드, 2nd param은 메소드의 인자로 활용된다.
            // 현재 찾은 메소드가 CodedInputStream을 파라미터로 하는 MergeFrom 메소드이므로 2nd param으로 cis을 넘김.
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
