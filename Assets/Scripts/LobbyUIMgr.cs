using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LobbyUIMgr : MonoBehaviour {

    public GameObject roomItem;
    public GameObject scrollContent;

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

    private void OnClickRoomItem()
    {
        Debug.Log("room clicked");
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
