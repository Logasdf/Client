using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using Google.Protobuf.Packet.Room;
using Assets.Scripts;

// 이 클래스는 게임로비(대기실아님) UI 처리를 담당하는 클래스
public class LobbyUIMgr : MonoBehaviour {

    public GameObject modalWindowPrefab; 
    public GameObject roomItem; 
    public GameObject scrollContent; 

    private PacketManager packetManager;
    
    public void RequestCreateModalWindow() // 방만들기 버튼 이벤트 리스너가 이 함수를 호출함
    {
        CreateModalWindow();
    }

    private RoomContext roomContext;
    private Color selectedRoomColor = Color.gray; 
    private Color unselectedRoomColor = Color.black;
    private Color nameFieldBlankColor = Color.red;

    private void Awake()
    {
       // Debug.Log(this.ToString() + " Awake()");
        
    }

    private void Start()
    {
        Debug.Log("This is a LobbyUIMgr's Start()");
        packetManager = GameObject.Find("PacketManager").GetComponent<PacketManager>();
        packetManager.SetHandleMessage(PopMessage);
        roomContext = RoomContext.GetInstance();
    }

    public void PopMessage(object obj, Type type)
    {
        if(type.Name == "Data")
        {
            Data data = (Data)obj;
            string contentType = data.DataMap["contentType"];
            //Debug.Log(contentType);
            switch(contentType)
            {
                case "ASSIGN_USERNAME":
                    roomContext.SetUsername(data.DataMap["userName"]);
                    break;
            }
        }
        else if(type.Name == "RoomList")
        {
            ShowRoomList((RoomList)obj);
        }
        else if(type.Name == "RoomInfo")
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

    private void InitEventTriggerForRoomItem(EventTrigger trigger, GameObject obj)
    {
        EventTrigger.Entry entry_PointerEnter = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerEnter 
        };                                          
        entry_PointerEnter.callback.AddListener((data) => { ChangeObjectBgColor(obj, selectedRoomColor); });

        EventTrigger.Entry entry_PointerExit = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerExit 
        };                                        
        entry_PointerExit.callback.AddListener((data) => { ChangeObjectBgColor(obj, unselectedRoomColor); });

        trigger.triggers.Add(entry_PointerEnter);
        trigger.triggers.Add(entry_PointerExit); 
    }

    private void ShowRoomList(RoomList roomList) 
    {
        Debug.Log("RoomList Initialized!!");
        foreach (Transform child in scrollContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach(var pair in roomList.Rooms)
        {
            RoomInfo room = pair.Value;
            GameObject roomObj = Instantiate(roomItem);
            var textFields = roomObj.GetComponentsInChildren<Text>();
            textFields[0].text = room.Name;
            textFields[1].text = string.Format("( {0,3:D2} / {1,3:D2} )", room.Current, room.Limit);
            roomObj.GetComponent<Button>().onClick.AddListener( delegate {
                OnClickRoomItem(textFields[0].text, room.Current, room.Limit);
            });
            EventTrigger eTrigger = roomObj.AddComponent<EventTrigger>();
            InitEventTriggerForRoomItem(eTrigger, roomObj);
            roomObj.transform.SetParent(scrollContent.transform, false);
        }
    }

    private void CreateModalWindow() 
    {
        GameObject modalWindow = Instantiate(modalWindowPrefab); 
        GameObject dialogWindow = modalWindow.transform.GetChild(0).gameObject; 
       

        Button submitBtn = dialogWindow.transform.Find("SubmitBtn").gameObject.GetComponent<Button>();
        Button cancelBtn = dialogWindow.transform.Find("CancelBtn").gameObject.GetComponent<Button>();

        submitBtn.onClick.AddListener(delegate { SubmitCreateRoomRequest(dialogWindow); }); 
        cancelBtn.onClick.AddListener(delegate { Destroy(modalWindow); }); 

        GameObject canvas = GameObject.Find("Canvas"); 
        modalWindow.transform.SetParent(canvas.transform, false); 
        modalWindow.transform.SetAsLastSibling(); 
    }

    private void SubmitCreateRoomRequest(GameObject window) // 방 만들기 대화상자에서 확인버튼 누르면 실행
    {
        //TODO : Send CREATE ROOM REQUEST to the server and Get the response.
        InputField nameField = window.transform.Find("NameInputField").gameObject.GetComponent<InputField>(); 
        string roomName = nameField.text; 

        if(roomName.Trim() == "") 
        {
            nameField.GetComponent<Image>().color = nameFieldBlankColor; 
            return;
        }

        Dropdown limitDropdown = window.transform.Find("LimitDropdown").gameObject.GetComponent<Dropdown>(); 
        string selectedVal = limitDropdown.options[limitDropdown.value].text; 

        Debug.Log("Room name : " + roomName + ", and the selected value is " + selectedVal);

        Data data = new Data();
        data.DataMap["contentType"] = "CREATE_ROOM";
        data.DataMap["roomName"] = roomName;
        data.DataMap["limits"] = selectedVal;
        data.DataMap["userName"] = roomContext.GetMyUsername();
        packetManager.PackMessage(protoObj: data);
    }

    private void OnClickRoomItem(string roomName, int currentUserCount, int userCountMax) // 방 입장
    {
        /*
         * TODO : Send a message to the server and get the response.
         * With that response, the appropriate code block will be executed.
        */

        Data data = new Data();
        data.DataMap["contentType"] = "ENTER_ROOM";
        data.DataMap["roomName"] = roomName;
        data.DataMap["userName"] = roomContext.GetMyUsername();
        packetManager.PackMessage(protoObj: data);

        //Debug.Log("element clicked : " + roomName);
        //roomContext.EnterRoomAsParticipant(roomName); //test
        //SceneManager.LoadScene("WaitingRoom");
    }

    private void ChangeObjectBgColor(GameObject obj, Color color)
    {
        obj.GetComponent<Image>().color = color;
    }
}
