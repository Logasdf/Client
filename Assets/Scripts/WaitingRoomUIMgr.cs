using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts;
using DrawType = WaitingRoomUIPainter.DrawType;
using Google.Protobuf.Packet.Room;
using MapField = Google.Protobuf.Collections.MapField<string, string>;

public class WaitingRoomUIMgr : MonoBehaviour {

    private RoomContext roomContext;
    private PacketManager packetManager;
    private WaitingRoomUIPainter painter;
    private const int BLUEINDEXSTART = 8;

    public void OnStartButtonClicked()
    {
        Data request;
        lock(Locks.lockForRoomContext)
        {
            Tuple<string, string>[] keyValPairs =
            {
                MakeKeyValuePair(MessageTypeStrings.CONTENT_TYPE, MessageTypeStrings.START_GAME),
                MakeKeyValuePair(MessageTypeStrings.ROOMID, roomContext.GetRoomId().ToString())
            };
            request = GetDataInstanceAfterSettingDataMap(keyValPairs);
        }
        packetManager.PackMessage(protoObj: request);
    }

    public void OnReadyButtonClicked()
    {
        Data request;
        lock (Locks.lockForRoomContext)
        {
            Tuple<string, string>[] keyValPairs =
            {
                MakeKeyValuePair(MessageTypeStrings.CONTENT_TYPE, MessageTypeStrings.READY_EVENT),
                MakeKeyValuePair(MessageTypeStrings.ROOMID, roomContext.GetRoomId().ToString()),
                MakeKeyValuePair(MessageTypeStrings.POSITION, roomContext.GetMyPosition().ToString()),
                MakeKeyValuePair(MessageTypeStrings.TOREADY, !roomContext.GetClient(roomContext.GetMyPosition()).Ready ? "true" : "false")
            };
            request = GetDataInstanceAfterSettingDataMap(keyValPairs);
        }
        packetManager.PackMessage(protoObj: request);
    }

    public void OnLeaveButtonClicked()
    {
        Data request;
        lock (Locks.lockForRoomContext)
        {
            if (roomContext.IsReady())
                return;

            Tuple<string, string>[] keyValPairs =
            {
               MakeKeyValuePair(MessageTypeStrings.CONTENT_TYPE, MessageTypeStrings.LEAVE_GAMEROOM),
               MakeKeyValuePair(MessageTypeStrings.ROOMID, roomContext.GetRoomId().ToString()),
               MakeKeyValuePair(MessageTypeStrings.POSITION, roomContext.GetMyPosition().ToString())
            };
            request = GetDataInstanceAfterSettingDataMap(keyValPairs);
        }
        packetManager.PackMessage(protoObj : request);
        SceneManager.LoadScene(PathStrings.SCENE_GAMELOBBY);
    }

    public void OnTeamChangeButtonClicked()
    {
        Data request;
        lock (Locks.lockForRoomContext)
        {
            if (roomContext.IsReady())
                return;

            Tuple<string, string>[] keyValPairs =
            {
                MakeKeyValuePair(MessageTypeStrings.CONTENT_TYPE, MessageTypeStrings.TEAM_CHANGE),
                MakeKeyValuePair(MessageTypeStrings.ROOMID, roomContext.GetRoomId().ToString()),
                MakeKeyValuePair(MessageTypeStrings.PREV_POSITION, roomContext.GetMyPosition().ToString())
            };
            request = GetDataInstanceAfterSettingDataMap(keyValPairs);
        }
        packetManager.PackMessage(protoObj: request);
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
                    MakeKeyValuePair(MessageTypeStrings.ROOMID, roomContext.GetRoomId().ToString()),
                    MakeKeyValuePair(MessageTypeStrings.CHAT_STRING, message)
                };
                Data request = GetDataInstanceAfterSettingDataMap(keyValPairs);
                //packetManager.PackMessage(타입, data);
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
        painter.Init(roomContext.GetMaxUserCount(), roomContext.IsHost());
        painter.Draw(roomContext, DrawType.BOTH);
    }

    private void ProcessUserEnterEventReceived(Client newClient)
    {
        lock(Locks.lockForRoomContext)
        {
            roomContext.AddUserToTeam(newClient, newClient.Position);
        }
        TellPainterToDraw(newClient.Position);
    }

    private void ProcessTeamChangeEventReceived(MapField response) //test private
    {
        int prevPos = int.Parse(response[MessageTypeStrings.PREV_POSITION]);
        int nextPos = int.Parse(response[MessageTypeStrings.NEXT_POSITION]);

        lock (Locks.lockForRoomContext)
        {
            roomContext.ChangeTeam(prevPos, nextPos);
        }
        painter.Draw(roomContext, DrawType.BOTH);
    }

    private void ProcessReadyEventReceived(MapField response) //test private
    {
        int position = int.Parse(response[MessageTypeStrings.POSITION]);
        lock (Locks.lockForRoomContext)
        {
            roomContext.ReverseReadyState(position);
        }
        painter.ChangeReadyStateColor(GetCalculatedIndex(position), bool.Parse(response[MessageTypeStrings.TOREADY]));
    }

    private void ProcessLeaveEventReceived(MapField response)
    {
        int position = int.Parse(response[MessageTypeStrings.POSITION]);
        lock (Locks.lockForRoomContext)
        {
            roomContext.DeleteUserFromTeam(position);
        }
        TellPainterToDraw(position);
    }

    private void ProcessHostChangeEventReceived(MapField response)
    {
        int newHostPos = int.Parse(response[MessageTypeStrings.NEWHOST]);
        //방장을 저장하는게 없네 현재... 정보는 클라이언트에서 가지고있어서 간단한 문제라 이건 나중에 의논하기로..
        bool isReady;
        bool isHost;
        lock(Locks.lockForRoomContext)
        {
            isReady = roomContext.IsReady();
            isHost = (newHostPos == roomContext.GetMyPosition());
        }
        if (isHost)
        { //내가 방장이 된 경우
            painter.DisplayAppropriateButton(true);
            if(isReady)
            { //레디상태라면 레디 풀어야함
                OnReadyButtonClicked();
            }
        }
    }

    private void ProcessChatMessageReceived(string response)
    {
        painter.AddMessageToChatWindow(response);
    }

    //이게 좋은건가,,,
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
            if(messageType == MessageType.START_GAME)
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
                case MessageTypeStrings.TEAM_CHANGE:
                    ProcessTeamChangeEventReceived(response);
                    break;

                case MessageTypeStrings.LEAVE_GAMEROOM:
                    ProcessLeaveEventReceived(response);
                    break;

                case MessageTypeStrings.READY_EVENT:
                    ProcessReadyEventReceived(response);
                    break;

                case MessageTypeStrings.HOST_CHANGED:
                    ProcessHostChangeEventReceived(response);
                    break;

                default:
                    break;
            }
        }
        else if (type.Name == MessageTypeStrings.CLIENT)
        {
            ProcessUserEnterEventReceived((Client)obj);
        }
        else
        {
            Debug.Log("type unidentified. please check");
        }
    }

    private int GetCalculatedIndex(int position)
    {
        int ret = position;
        if (ret >= BLUEINDEXSTART)
            ret = (ret % BLUEINDEXSTART) + (roomContext.GetMaxUserCount() / 2);
        return ret;
    }

    private void TellPainterToDraw(int position = -1)
    {
        if(position == -1)
        {
            painter.Draw(roomContext, DrawType.BOTH);
            return;
        }

        if (position < BLUEINDEXSTART)
            painter.Draw(roomContext, DrawType.REDONLY);
        else
            painter.Draw(roomContext, DrawType.BLUEONLY);
    }
} 
