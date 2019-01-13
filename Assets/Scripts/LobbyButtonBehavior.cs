using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class LobbyButtonBehavior : MonoBehaviour {

    public void CreateBtnClicked()
    {
        uiMgr.RequestCreateModalWindow();
    }

    public void RefreshBtnClicked()
    {
        Debug.Log("refresh btn clicked");
        packetManager.PackMessage(MessageType.REFRESH);
    }

    public void ExitBtnClicked()
    {
        Application.Quit();
    }

    private LobbyUIMgr uiMgr;
    private PacketManager packetManager; 

    private void Start()
    {
        //Debug.Log("This is a LobbyButtonBehavior's Start()");
        uiMgr = GameObject.Find("UIManager").GetComponent<LobbyUIMgr>();
        packetManager = GameObject.Find("PacketManager").GetComponent<PacketManager>();
    }
}
