using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Google.Protobuf.Packet.Room;

public class LobbyUIPainter : ScriptableObject {
    //Instantiated Gameobjects
    private GameObject mainCanvas;
    private GameObject createRoomWindow;
    private GameObject errorWindow;
    private GameObject scrollContent;
    private InputField roomNameInputField;
    private Dropdown roomLimitDropdown;

    //Loaded Prefabs
    private GameObject roomItem;

    private Color selectedRoomColor = Color.gray;
    private Color unselectedRoomColor = Color.black;
    private Color nameFieldBlankColor = Color.red;
    private Color nameFieldNormalColor = Color.white;

    public void Init(GameObject scrollContent, Action<InputField, Dropdown> CreateRoomHandler)
    {
        this.scrollContent = scrollContent;
        InitCreateRoomWindow(CreateRoomHandler);
    }

    public void ShowCreateRoomWindow()
    {
        roomNameInputField.text = "";
        roomLimitDropdown.value = 0;
        ChangeObjectBgColor(roomNameInputField.gameObject, nameFieldNormalColor);
        createRoomWindow.SetActive(true);
    }

    public void DisplayRoomlist(RoomList roomList, Action<string, int, int> RoomItemClickHandler)
    {
        foreach (Transform child in scrollContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var pair in roomList.Rooms)
        {
            RoomInfo room = pair.Value;
            GameObject roomObj = Instantiate(roomItem);
            var textFields = roomObj.GetComponentsInChildren<Text>();
            textFields[0].text = room.Name;
            textFields[1].text = string.Format("( {0,3:D2} / {1,3:D2} )", room.Current, room.Limit);
            roomObj.GetComponent<Button>().onClick.AddListener(delegate {
                RoomItemClickHandler(textFields[0].text, room.Current, room.Limit);
            });
            EventTrigger eTrigger = roomObj.AddComponent<EventTrigger>();
            InitEventTriggerForRoomItem(eTrigger, roomObj);
            roomObj.transform.SetParent(scrollContent.transform, false);
        }
    }

    public void OnRoomCreatedWithEmptyTitle()
    {
        ChangeObjectBgColor(roomNameInputField.gameObject, nameFieldBlankColor);
    }

    private void OnEnable()
    {
        mainCanvas = GameObject.Find("Canvas");
        createRoomWindow = Instantiate((GameObject)Resources.Load("Prefabs/CreateRoomPanel"));
        errorWindow = Instantiate((GameObject)Resources.Load("Prefabs/ErrorMessagePanel"));
        roomItem = (GameObject)Resources.Load("Prefabs/RoomItem");
    }

    private void InitCreateRoomWindow(Action<InputField, Dropdown> CreateRoomHandler)
    {
        GameObject dialogWindow = createRoomWindow.transform.GetChild(0).gameObject;

        roomNameInputField = dialogWindow.transform.Find("NameInputField").GetComponent<InputField>();
        roomLimitDropdown = dialogWindow.transform.Find("LimitDropdown").GetComponent<Dropdown>();

        Button submitBtn = dialogWindow.transform.Find("SubmitBtn").gameObject.GetComponent<Button>();
        Button cancelBtn = dialogWindow.transform.Find("CancelBtn").gameObject.GetComponent<Button>();
        submitBtn.onClick.AddListener(delegate { CreateRoomHandler(roomNameInputField, roomLimitDropdown); });
        cancelBtn.onClick.AddListener(delegate { createRoomWindow.SetActive(false); });

        createRoomWindow.transform.SetParent(mainCanvas.transform, false);
        createRoomWindow.transform.SetAsLastSibling();
        createRoomWindow.SetActive(false);
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

    private void ChangeObjectBgColor(GameObject obj, Color color)
    {
        obj.GetComponent<Image>().color = color;
    }
}
