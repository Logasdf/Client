﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Google.Protobuf.Packet.Room;

public class LobbyUIPainter : ScriptableObject {
    //Instantiated Gameobjects
    private GameObject mainCanvas;
    private GameObject scrollContent;

    private GameObject createRoomWindow;
    private InputField roomNameInputField;
    private Dropdown roomLimitDropdown;

    private GameObject errorWindow;
    private Text errorMessageTextField;

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
        InitErrorWindow();
    }

    public void DisplayCreateRoomWindow()
    {
        roomNameInputField.text = "";
        roomLimitDropdown.value = 0;
        ChangeObjectBgColor(roomNameInputField.gameObject, nameFieldNormalColor);
        createRoomWindow.SetActive(true);
    }

    public void DisplayErrorWindow(string errorMessage)
    {
        errorMessageTextField.text = errorMessage;
        errorWindow.SetActive(true);
    }

    public void DestroyRoomObjects()
    {
        foreach (Transform child in scrollContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void DisplayRoomlist(RoomList roomList, Action<string, int, int> RoomItemClickHandler)
    {
        DestroyRoomObjects();

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
        mainCanvas = GameObject.Find(ElementStrings.CANVAS);
        createRoomWindow = Instantiate((GameObject)Resources.Load(PathStrings.CREATE_ROOM_PANEL));
        errorWindow = Instantiate((GameObject)Resources.Load(PathStrings.ERROR_MESSAGE_PANEL));
        roomItem = (GameObject)Resources.Load(PathStrings.ROOMITEM);
    }

    private void InitCreateRoomWindow(Action<InputField, Dropdown> CreateRoomHandler)
    {
        GameObject dialogWindow = createRoomWindow.transform.GetChild(0).gameObject;

        roomNameInputField = dialogWindow.transform.Find(ElementStrings.ROOMNAME_INPUTFIELD).GetComponent<InputField>();
        roomLimitDropdown = dialogWindow.transform.Find(ElementStrings.ROOMLIMIT_DROPDOWN).GetComponent<Dropdown>();

        Button submitBtn = dialogWindow.transform.Find(ElementStrings.SUBMIT_BTN).gameObject.GetComponent<Button>();
        Button cancelBtn = dialogWindow.transform.Find(ElementStrings.CANCEL_BTN).gameObject.GetComponent<Button>();
        submitBtn.onClick.AddListener(delegate { CreateRoomHandler(roomNameInputField, roomLimitDropdown); });
        cancelBtn.onClick.AddListener(delegate { createRoomWindow.SetActive(false); });

        createRoomWindow.transform.SetParent(mainCanvas.transform, false);
        createRoomWindow.transform.SetAsLastSibling();
        createRoomWindow.SetActive(false);
    }

    private void InitErrorWindow()
    {
        GameObject window = errorWindow.transform.Find(ElementStrings.ERRORMESSAGE_WINDOW).gameObject;
        errorMessageTextField = window.transform.Find(ElementStrings.ERRORMESSAGE_TEXTFIELD).GetComponent<Text>();

        Button closeButton = window.transform.Find(ElementStrings.CLOSE_BTN).GetComponent<Button>();
        closeButton.onClick.AddListener(delegate { errorWindow.SetActive(false); });

        errorWindow.transform.SetParent(mainCanvas.transform, false);
        errorWindow.transform.SetAsLastSibling();
        errorWindow.SetActive(false);
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
