using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LobbyUIMgr : MonoBehaviour {

    public GameObject roomItem;
    public GameObject scrollContent;

	private void Awake () {
        ServerConnection connection = GameObject.Find("Connection").GetComponent<ServerConnection>();
        connection.SetLobbyUIMgrCallBack(CallBackMethod);
        for(int i = 0; i<10; i++)
        {
            GameObject room = (GameObject)Instantiate(roomItem);
            room.transform.SetParent(scrollContent.transform, false);
        }
        
	}

    private void CallBackMethod()
    {
        Debug.Log("callback method called");
    }
}
