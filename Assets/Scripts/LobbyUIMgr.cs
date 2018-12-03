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
        entry_PointerEnter.callback.AddListener((data) => { PointerEntered(data, obj); });

        EventTrigger.Entry entry_PointerExit = new EventTrigger.Entry();
        entry_PointerExit.eventID = EventTriggerType.PointerExit;
        entry_PointerExit.callback.AddListener((data) => { PointerExited(data, obj); });

        trigger.triggers.Add(entry_PointerEnter);
        trigger.triggers.Add(entry_PointerExit);
    }

    private void CallBackMethod()
    {
        Debug.Log("callback method called");
        for (int i = 0; i < 10; i++)
        {
            GameObject room = (GameObject)Instantiate(roomItem);
            var textFields = room.GetComponentsInChildren<Text>();
            textFields[0].text = "Room #" + (i + 1);
            room.GetComponent<Button>().onClick.AddListener(delegate { OnClickRoomItem(); });
            EventTrigger eTrigger = room.AddComponent<EventTrigger>();
            InitEventTriggerForRoomItem(eTrigger, room);
            room.transform.SetParent(scrollContent.transform, false);
        }
    }

    private void CreateModalWindow() // it is possible to use enum as the argument of this func
    {
        Debug.Log("Successfully Executed");
        GameObject modalWindow = (GameObject)Instantiate(modalWindowPrefab);
        GameObject dialogWindow = modalWindow.transform.GetChild(0).gameObject;

        GameObject submitBtn = dialogWindow.transform.GetChild(1).gameObject;
        GameObject cancelBtn = dialogWindow.transform.GetChild(2).gameObject;

        submitBtn.GetComponent<Button>().onClick.AddListener(delegate { SubmitCreateRoomRequest(); });
        cancelBtn.GetComponent<Button>().onClick.AddListener(delegate { CloseModalWindow(modalWindow); });

        GameObject canvas = GameObject.Find("Canvas");
        modalWindow.transform.SetParent(canvas.transform, false);
        modalWindow.transform.SetAsLastSibling();
    }

    public void SubmitCreateRoomRequest()
    {
        Debug.Log("CREATE ROOM CLICKED");
        //TODO : Send CREATE ROOM REQUEST to the server and Get the response.

        SceneManager.LoadScene("WaitingRoom");
    }

    public void CloseModalWindow(GameObject window)
    {
        Destroy(window);
    }

    private void OnClickRoomItem()
    {
        Debug.Log("room clicked");
        /*
         * TODO : Send a message to the server and get the response.
         * With that response, the appropriate code block will be executed.
        */
        
    }

    //test
    private void PointerEntered(BaseEventData data, GameObject obj) 
    {
        obj.GetComponent<Image>().color = selectedRoomColor;
    }

    //test
    private void PointerExited(BaseEventData data, GameObject obj)
    {
        obj.GetComponent<Image>().color = unselectedRoomColor;
    }
}
