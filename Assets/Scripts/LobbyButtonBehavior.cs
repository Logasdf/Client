using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyButtonBehavior : MonoBehaviour {

    public void CreateBtnClicked()
    {
        uiMgr.RequestCreateModalWindow();
    }

    public void RefreshBtnClicked()
    {
        Debug.Log("refresh btn clicked");
    }

    public void ExitBtnClicked()
    {
        Application.Quit();
    }

    private LobbyUIMgr uiMgr;
    private void Start()
    {
        uiMgr = GameObject.Find("UIManager").GetComponent<LobbyUIMgr>();
    }
}
