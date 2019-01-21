using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts;
using Google.Protobuf.Packet.Room;
using MapField = Google.Protobuf.Collections.MapField<string, string>;

public class WaitingRoomUIMgr : MonoBehaviour {

    private RoomContext roomContext;
    private PacketManager packetManager;
    private WaitingRoomUIPainter painter;

    public void OnStartButtonClicked()
    {
        Data request;
        lock(Locks.lockForRoomContext)
        {
            Tuple<string, string>[] keyValPairs =
            {
                MakeKeyValuePair(MessageTypeStrings.CONTENT_TYPE, MessageTypeStrings.START_GAME),
                MakeKeyValuePair(MessageTypeStrings.ROOMID, roomContext.RoomId.ToString())
            };
            request = GetDataInstanceAfterSettingDataMap(keyValPairs);
        }
        packetManager.PackMessage(protoObj: request);
    }

    public void OnReadyButtonClicked()
    {
        packetManager.PackMessage(MessageType.READY_EVENT);
    }

    public void OnLeaveButtonClicked()
    {
        if (CheckReadyState())
            return;

        packetManager.PackMessage(MessageType.LEAVE_GAMEROOM);
        SceneManager.LoadScene(PathStrings.SCENE_GAMELOBBY);
    }

    public void OnTeamChangeButtonClicked()
    {
        if (CheckReadyState())
            return;

        packetManager.PackMessage(MessageType.TEAM_CHANGE);
    }

    public void OnChatEndEdit(InputField chatField)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string message = chatField.text;
            if (message.Length != 0)
            {
                Tuple<string, string>[] keyValPairs =
                {
                    MakeKeyValuePair(MessageTypeStrings.CONTENT_TYPE, MessageTypeStrings.CHAT_MESSAGE),
                    MakeKeyValuePair(MessageTypeStrings.ROOMID, roomContext.RoomId.ToString()),
                    MakeKeyValuePair(MessageTypeStrings.CHAT_STRING, message)
                };
                Data request = GetDataInstanceAfterSettingDataMap(keyValPairs);
            }
            chatField.text = "";
            chatField.ActivateInputField();            
        }   
    }

    private void Start()
    {
        roomContext = RoomContext.GetInstance();
        packetManager = GameObject.Find(ElementStrings.PACKETMANAGER).GetComponent<PacketManager>();
        packetManager.SetHandleMessage(PopMessage);
        painter = (WaitingRoomUIPainter)ScriptableObject.CreateInstance(ElementStrings.WAITINGROOMUIPAINTER);
        painter.Init(roomContext);
        packetManager.PackMessage(MessageType.SEEK_MYPOSITION);
    }
    
    private void ProcessChatMessageReceived(string response)
    {
        painter.AddMessageToChatWindow(response);
    }

    private Data GetDataInstanceAfterSettingDataMap(params Tuple<string, string>[] keyValuePairs)
    {
        Data request = new Data();
        MapField dataMap = request.DataMap;
        foreach (var pair in keyValuePairs)
        {
            dataMap.Add(pair.Item1, pair.Item2);
        }
        return request;
    }

    private Tuple<string, string> MakeKeyValuePair(string key, string value)
    {
        return new Tuple<string, string>(key, value);
    }

    private void PopMessage(object obj, Type type)
    {
        if(type.Name == MessageTypeStrings.INT32)
        {
            int messageType = (int)obj;
            if (messageType == MessageType.START_GAME)
            {
                Debug.Log("GAME START!!");
                SceneManager.LoadScene(PathStrings.SCENE_INGAME);
            }
        }
        else if(type.Name == MessageTypeStrings.DATA)
        {
            MapField response = ((Data)obj).DataMap;
            string contentType = response[MessageTypeStrings.CONTENT_TYPE];
            switch(contentType)
            {
                case MessageTypeStrings.MY_POSITION:
                    lock(Locks.lockForRoomContext)
                    {
                        roomContext.SetMyPosition(int.Parse(response["position"]));
                        painter.Draw();
                    }
                    break;
                case MessageTypeStrings.REJECT_START_GAME:
                    painter.DisplayErrorMessage(response["errorMessage"]);
                    break;
                default:
                    break;
            }
        }
        else if (type.Name == MessageTypeStrings.ROOMINFO)
        {
            lock(Locks.lockForRoomContext)
            {
                //roomInfo가 오는 모든 경우
                RoomInfo rInfo = (RoomInfo)obj;
                roomContext.SetRoomInfo(rInfo);
                //내 pos바뀔 가능성이 있는 경우 -> leave, teamchange
                packetManager.PackMessage(MessageType.SEEK_MYPOSITION);
            }
        }
        else
        {
            Debug.Log("type unidentified. please check : " + type.Name);
        }
    }

    private bool CheckReadyState()
    {
        bool isReady;
        lock (Locks.lockForRoomContext)
        {
            int myPos = roomContext.GetMyPosition();
            Client clnt = myPos < 8 ? roomContext.GetRedteamClient(myPos) : roomContext.GetBlueteamClient(myPos - 8);
            isReady = clnt.Ready;
        }
        
        if(isReady)
        {
            string msg = "준비상태를 먼저 해제해주세요.";
            painter.DisplayErrorMessage(msg);
        }
        return isReady;
    }
} 
