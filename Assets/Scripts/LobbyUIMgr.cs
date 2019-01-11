using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Google.Protobuf.Packet.Room;
using Assets.Scripts;

public class LobbyUIMgr : MonoBehaviour {

    public GameObject scrollContent; 

    private PacketManager packetManager;
    private LobbyUIPainter painter;
    private RoomContext roomContext;

    public void OnCreateButtonClicked()
    {
        painter.ShowCreateRoomWindow();
    }

    public void OnRefreshButtonClicked()
    {
        packetManager.PackMessage(MessageType.REFRESH);
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }

    public void PopMessage(object obj, Type type)
    {
        Debug.Log("메시지 타입이름 : " + type.Name); //test
        if (type.Name == "Data")
        {
            Data data = (Data)obj;
            string contentType = data.DataMap["contentType"];
            Debug.Log(contentType);
            switch (contentType)
            {
                case "ASSIGN_USERNAME":
                    roomContext.SetUsername(data.DataMap["userName"]);
                    break;
            }
        }
        else if (type.Name == "RoomList")
        {
            painter.DisplayRoomlist((RoomList)obj, OnRoomItemClicked); 
        }
        else if (type.Name == "RoomInfo")
        {
            Debug.Log("Create/Enter Room Success!!");
            RoomInfo room = (RoomInfo)obj;
            roomContext.InitRoomContext(room);
            SceneManager.LoadScene("WaitingRoom");
        }
        else
        {
            Debug.Log("Type is not defined...., Check it!");
        }
    }

    private void Start()
    {
        packetManager = GameObject.Find("PacketManager").GetComponent<PacketManager>();
        packetManager.SetHandleMessage(PopMessage);
        roomContext = RoomContext.GetInstance();
        painter = (LobbyUIPainter)ScriptableObject.CreateInstance("LobbyUIPainter");
        painter.Init(scrollContent, SubmitCreateRoomRequest);
    }

    private void SubmitCreateRoomRequest(InputField nameField, Dropdown limitDropdown)
    {
        string roomName = nameField.text;
        if (roomName.Trim() == "")
        {
            painter.OnRoomCreatedWithEmptyTitle();
            return;
        }

        string selectedVal = limitDropdown.options[limitDropdown.value].text;

        Data data = new Data();
        data.DataMap["contentType"] = "CREATE_ROOM";
        data.DataMap["roomName"] = roomName;
        data.DataMap["limits"] = selectedVal;
        data.DataMap["userName"] = roomContext.GetMyUsername();
        packetManager.PackMessage(protoObj: data);
        
    }

    private void OnRoomItemClicked(string roomName, int currentUserCount, int userCountMax) // 방 입장
    {
        Data data = new Data();
        data.DataMap["contentType"] = "ENTER_ROOM";
        data.DataMap["roomName"] = roomName;
        data.DataMap["userName"] = roomContext.GetMyUsername();
        packetManager.PackMessage(protoObj: data);
    }
}
