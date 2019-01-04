using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DrawType = WaitingRoomUIPainter.DrawType;
using MapField = Google.Protobuf.Collections.MapField<string, string>;

public class WaitingRoomUIMgr : MonoBehaviour {

    private RoomContext roomContext;
    private PacketManager packetManager;
    private WaitingRoomUIPainter painter;
    private const int MAXEACH = 8;

    public void OnStartButtonClicked()
    {
        Debug.Log("시작");
    }

    public void OnReadyButtonClicked()
    {
        Tuple<string, string>[] keyValPairs =
        {
            MakeKeyValuePair("content_type", "READY_EVENT"),
            MakeKeyValuePair("roomId", roomContext.GetRoomId().ToString()),
            MakeKeyValuePair("position", roomContext.GetMyPosition().ToString()),
            MakeKeyValuePair("toReady", !roomContext.IsReady() ? "true" : "false")
        };
        Data request = GetDataInstanceAfterSettingDataMap(keyValPairs);

        //packetManager.SerializeAndSend(request);
        painter.ChangeReadyStateColor(GetCalculatedIndex(roomContext.GetMyPosition()), !roomContext.IsReady());
        roomContext.ReverseReadyState();
    }

    public void OnLeaveButtonClicked()
    {
        if (roomContext.IsReady())
            return;
       
        Tuple<string, string>[] keyValPairs =
        {
           MakeKeyValuePair("content_type", "LEAVE_GAMEROOM"),
           MakeKeyValuePair("roomId", roomContext.GetRoomId().ToString()),
           MakeKeyValuePair("position", roomContext.GetMyPosition().ToString())
        };
        Data request = GetDataInstanceAfterSettingDataMap(keyValPairs);
        //packetManager.SerializeAndSend(request);
        SceneManager.LoadScene("GameLobby");
    }

    public void OnTeamChangeButtonClicked()
    {
        if (roomContext.IsReady())
            return;

        Tuple<string, string>[] keyValPairs =
        {
            MakeKeyValuePair("content_type", "TEAM_CHANGE"),
            MakeKeyValuePair("roomId", roomContext.GetRoomId().ToString()),
            MakeKeyValuePair("position", roomContext.GetMyPosition().ToString())
        };

        Data request = GetDataInstanceAfterSettingDataMap(keyValPairs);
        //packetManager.SerializeAndSend(data);
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
                    MakeKeyValuePair("content_type", "CHAT_MESSAGE"),
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
        painter = (WaitingRoomUIPainter)ScriptableObject.CreateInstance("WaitingRoomUIPainter");
        painter.Init(roomContext.GetMaxUserCount(), roomContext.IsHost());
        painter.Draw(roomContext, DrawType.BOTH);
    }

    private void ProcessTeamChangeEventReceived(MapField response) //test private
    {
        //서버에서 오는 정보는 과거 위치와 이동할 위치라고 가정하자.
        int prevPos = int.Parse(response["prev_position"]);
        int nextPos = int.Parse(response["next_position"]);

        roomContext.ChangeTeam(prevPos, nextPos);
        painter.Draw(roomContext, DrawType.BOTH);
    }

    private void ProcessReadyEventReceived(MapField response) //test private
    {
        painter.ChangeReadyStateColor(GetCalculatedIndex(int.Parse(response["position"])), bool.Parse(response["toReady"]));
    }

    private void ProcessLeaveEventReceived(MapField response)
    {
        int position = int.Parse(response["position"]);
        roomContext.DeleteUserFromTeam(position);

        if (position < MAXEACH)
            painter.Draw(roomContext, DrawType.REDONLY);
        else
            painter.Draw(roomContext, DrawType.BLUEONLY);
    }

    private void ProcessChatMessageReceived(string response)
    {
        painter.AddMessageToChatWindow(response);
    }

    private int GetCalculatedIndex(int val)
    {
        int ret = val;
        if (ret >= MAXEACH)
            ret = (ret % MAXEACH) + (roomContext.GetMaxUserCount() / 2);
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
} 
