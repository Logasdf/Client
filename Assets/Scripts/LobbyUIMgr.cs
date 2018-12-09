using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LobbyUIMgr : MonoBehaviour {

    public GameObject modalWindowPrefab;
    public GameObject roomItem;
    public GameObject scrollContent;
    
    public void RequestCreateModalWindow()
    {
        CreateModalWindow();
    }

    private Color selectedRoomColor = Color.gray;
    private Color unselectedRoomColor = Color.black;
    private Color nameFieldBlankColor = Color.red;
    
	private void Awake () {
        AttachToServerAsDelegate();
	}

    private void AttachToServerAsDelegate()
    {
        ServerConnection connection = GameObject.Find("Connection").GetComponent<ServerConnection>();
        connection.SetLobbyUIMgrCallBack(CallBackMethod);
    }

    private void InitEventTriggerForRoomItem(EventTrigger trigger, GameObject obj)
    {
        EventTrigger.Entry entry_PointerEnter = new EventTrigger.Entry();
        entry_PointerEnter.eventID = EventTriggerType.PointerEnter;
        entry_PointerEnter.callback.AddListener((data) => { ChangeObjectBgColor(obj, selectedRoomColor); });

        EventTrigger.Entry entry_PointerExit = new EventTrigger.Entry();
        entry_PointerExit.eventID = EventTriggerType.PointerExit;
        entry_PointerExit.callback.AddListener((data) => { ChangeObjectBgColor(obj, unselectedRoomColor); });

        trigger.triggers.Add(entry_PointerEnter);
        trigger.triggers.Add(entry_PointerExit);
    }

    private void CallBackMethod()
    {
        Debug.Log("callback method called");
        for (int i = 0; i < 10; i++)
        {
            GameObject room = Instantiate(roomItem);
            var textFields = room.GetComponentsInChildren<Text>();
            textFields[0].text = "Room #" + (i + 1);
            room.GetComponent<Button>().onClick.AddListener(delegate {
                OnClickRoomItem(textFields[0].text);
            });
            EventTrigger eTrigger = room.AddComponent<EventTrigger>();
            InitEventTriggerForRoomItem(eTrigger, room);
            room.transform.SetParent(scrollContent.transform, false);
        }
    }

    private void CreateModalWindow() // it is possible to use enum as the argument of this func
    {
        Debug.Log("Successfully Executed");
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

    private void SubmitCreateRoomRequest(GameObject window)
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
        SceneManager.LoadScene("WaitingRoom");
    }

    private void OnClickRoomItem(string roomName)
    {
        /*
         * TODO : Send a message to the server and get the response.
         * With that response, the appropriate code block will be executed.
        */
        Debug.Log("element clicked : " + roomName);
        //SceneManager.LoadScene("WaitingRoom");
    }

    private void ChangeObjectBgColor(GameObject obj, Color color)
    {
        obj.GetComponent<Image>().color = color;
    }
}
