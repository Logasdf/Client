using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
        Debug.Log("시작");
    }

    public void OnReadyButtonClicked()
    {
        Tuple<string, string>[] keyValPairs =
        {
            MakeKeyValuePair("contentType", "READY_EVENT"),
            MakeKeyValuePair("roomId", roomContext.GetRoomId().ToString()),
            MakeKeyValuePair("position", roomContext.GetMyPosition().ToString()),
            MakeKeyValuePair("toReady", !roomContext.GetClient(roomContext.GetMyPosition()).Ready ? "true" : "false")
        };
        Data request = GetDataInstanceAfterSettingDataMap(keyValPairs);

        packetManager.PackMessage(protoObj: request);
    }

    public void OnLeaveButtonClicked()
    {
        if (roomContext.IsReady())
            return;
       
        Tuple<string, string>[] keyValPairs =
        {
           MakeKeyValuePair("contentType", "LEAVE_GAMEROOM"),
           MakeKeyValuePair("roomId", roomContext.GetRoomId().ToString()),
           MakeKeyValuePair("position", roomContext.GetMyPosition().ToString())
        };
        Data request = GetDataInstanceAfterSettingDataMap(keyValPairs);
        packetManager.PackMessage(protoObj : request);
        //SceneManager.LoadScene("GameLobby");
    }

    public void OnTeamChangeButtonClicked()
    {
        if (roomContext.IsReady())
            return;

        Tuple<string, string>[] keyValPairs =
        {
            MakeKeyValuePair("contentType", "TEAM_CHANGE"),
            MakeKeyValuePair("roomId", roomContext.GetRoomId().ToString()),
            MakeKeyValuePair("prev_position", roomContext.GetMyPosition().ToString())
        };

        Data request = GetDataInstanceAfterSettingDataMap(keyValPairs);
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
                    MakeKeyValuePair("contentType", "CHAT_MESSAGE"),
                    MakeKeyValuePair("roomId", roomContext.GetRoomId().ToString()),
                    MakeKeyValuePair("message", message)
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
        packetManager = GameObject.Find("PacketManager").GetComponent<PacketManager>();
        packetManager.SetHandleMessage(PopMessage);
        painter = (WaitingRoomUIPainter)ScriptableObject.CreateInstance("WaitingRoomUIPainter");
        painter.Init(roomContext.GetMaxUserCount(), roomContext.IsHost());
        painter.Draw(roomContext, DrawType.BOTH);
    }

    private void ProcessUserEnterEventReceived(Client newClient)
    {
        roomContext.AddUserToTeam(newClient, newClient.Position);
        TellPainterToDraw(newClient.Position);
    }

    private void ProcessTeamChangeEventReceived(MapField response) //test private
    {
        int prevPos = int.Parse(response["prev_position"]);
        int nextPos = int.Parse(response["next_position"]);

        roomContext.ChangeTeam(prevPos, nextPos);
        painter.Draw(roomContext, DrawType.BOTH);
    }

    private void ProcessReadyEventReceived(MapField response) //test private
    {
        int position = int.Parse(response["position"]);
        painter.ChangeReadyStateColor(GetCalculatedIndex(position), bool.Parse(response["toReady"]));
        roomContext.ReverseReadyState(position);
    }

    private void ProcessLeaveEventReceived(MapField response)
    {
        int position = int.Parse(response["position"]);
        roomContext.DeleteUserFromTeam(position);

        TellPainterToDraw(position);
    }

    private void ProcessChatMessageReceived(string response)
    {
        painter.AddMessageToChatWindow(response);
    }

    private int GetCalculatedIndex(int val)
    {
        int ret = val;
        if (ret >= BLUEINDEXSTART)
            ret = (ret % BLUEINDEXSTART) + (roomContext.GetMaxUserCount() / 2);
        return ret;
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
        if(type.Name == "Data")
        {
            MapField response = ((Data)obj).DataMap;
            string contentType = response["contentType"];
            switch(contentType)
            {
                case "TEAM_CHANGE":
                    ProcessTeamChangeEventReceived(response);
                    break;

                case "LEAVE_GAME":
                    break;

                case "READY_EVENT":
                    ProcessReadyEventReceived(response);
                    break;

                default:
                    break;
            }
        }
        else if (type.Name == "Client")
        {
            ProcessUserEnterEventReceived((Client)obj);
        }
        else
        {
            Debug.Log("type unidentified. please check");
        }
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
