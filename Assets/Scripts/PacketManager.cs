using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Google.Protobuf;
using Google.Protobuf.Packet.Room;

public class PacketManager : MonoBehaviour {

    public delegate void HandleMessage(object obj, Type type);

    public void SetHandleMessage(HandleMessage hm)
    {
        this.handleMessage = hm;
    }

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
    private Dictionary<int, Type> invTypeTable;
    private ServerConnection connection;
    private CodedOutputStream cos;
    private byte[] sendBuffer;

    // UI Manager Component(들)에게 Data를 넘기기위한 Delegate
    private HandleMessage handleMessage;

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
        initTypeTable(); // TypeTable 및 Inverse TypeTable 초기화
        //********* DICTIONARY INITIALIZE NEEDED *********
        
        sendBuffer = new byte[BUF_SIZE];
        cos = new CodedOutputStream(sendBuffer);
    }

    private void initTypeTable()
    {
        typeTable = new Dictionary<Type, int>() {
            
        };
        invTypeTable = new Dictionary<int, Type>() {
            {0, typeof(RoomList) }
        };
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
        /*
         *  현재 Message Format(테스트용)은 다음과 같다.
         *  [type:4byte][length:4byte][message_body:실제 메시지 길이만큼]
         */
        int type = BitConverter.ToInt32(buffer, 0);
        int length = BitConverter.ToInt32(buffer, 4);
        //Debug.Log(string.Format("{0}, {1}", type, length));
        try
        {
            // Inverse Type Table에서 type에 대응되는 Type값 찾아오기
            // 만약 type key가 없을 경우, Exception 발생
            Type t = invTypeTable[type];
            Deserialize(buffer, 8, length, t);
        }
        catch(KeyNotFoundException knfe)
        {
            Debug.Log(knfe.Message);
        }
    }

    private void Deserialize(byte[] buffer, int start, int length, Type type)
    {
        Debug.Log("Deserialize Start!");
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
            // Deserialize가 문제없이 완료될 경우, UI Manager에게 전송
            handleMessage(obj, type);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
