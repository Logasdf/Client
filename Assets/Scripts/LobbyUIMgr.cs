using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Google.Protobuf.Packet.Room;
using Assets.Scripts;

public class LobbyUIMgr : MonoBehaviour {

    [SerializeField]
    private GameObject scrollContent; 
    private PacketManager packetManager;
    private LobbyUIPainter painter;
    private RoomContext roomContext;

    public void OnCreateButtonClicked()
    {
        painter.DisplayCreateRoomWindow();
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
        Debug.Log("메시지 타입이름 : " + type.Name);
        if (type.Name == MessageTypeStrings.DATA)
        {
            Data data = (Data)obj;
            string contentType = data.DataMap[MessageTypeStrings.CONTENT_TYPE];

            switch(contentType)
            {
                case MessageTypeStrings.ASSIGN_USERNAME:
                    roomContext.SetUsername(data.DataMap[MessageTypeStrings.USERNAME]);
                    break;

                case MessageTypeStrings.REJECT_CREATEROOM:
                case MessageTypeStrings.REJECT_ENTERROOM:
                    painter.DisplayErrorWindow(data.DataMap[MessageTypeStrings.ERRORMESSAGE]);
                    break;
            }
        }
        else if (type.Name == MessageTypeStrings.ROOMLIST)
        {
            painter.DisplayRoomlist((RoomList)obj, OnRoomItemClicked); 
        }
        else if (type.Name == MessageTypeStrings.ROOMINFO)
        {
            RoomInfo room = (RoomInfo)obj;
            roomContext.InitRoomContext(room);
            SceneManager.LoadScene(PathStrings.SCENE_WAITINGROOM);
        }
        else
        {
            if(MessageType.EMPTY_ROOMLIST == (int)obj)
            {
                painter.DestroyRoomObjects();
            }
            else
            {
                Debug.Log("Type is not defined...., Check it!");
            }
        }
    }

    private void Start()
    {
        packetManager = GameObject.Find(ElementStrings.PACKETMANAGER).GetComponent<PacketManager>();
        packetManager.SetHandleMessage(PopMessage);
        roomContext = RoomContext.GetInstance();
        painter = (LobbyUIPainter)ScriptableObject.CreateInstance(ElementStrings.LOBBYUIPAINTER);
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
        data.DataMap[MessageTypeStrings.CONTENT_TYPE] = MessageTypeStrings.CREATE_ROOM;
        data.DataMap[MessageTypeStrings.ROOMNAME] = roomName;
        data.DataMap[MessageTypeStrings.LIMIT] = selectedVal;
        data.DataMap[MessageTypeStrings.USERNAME] = roomContext.GetMyUsername();
        packetManager.PackMessage(protoObj: data);
    }

    private void OnRoomItemClicked(string roomName, int currentUserCount, int userCountMax) 
    {
        Data data = new Data();
        data.DataMap[MessageTypeStrings.CONTENT_TYPE] = MessageTypeStrings.ENTER_ROOM;
        data.DataMap[MessageTypeStrings.ROOMNAME] = roomName;
        data.DataMap[MessageTypeStrings.USERNAME] = roomContext.GetMyUsername();
        packetManager.PackMessage(protoObj: data);
    }
}
