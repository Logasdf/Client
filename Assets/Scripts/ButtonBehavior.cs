using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour {

    public void CreateBtnClicked()
    {
        //TODO : display a dialog window to communicate with a user
        LobbyUIMgr uiMgr = GameObject.Find("UIManager").GetComponent<LobbyUIMgr>();
        uiMgr.RequestCreateModalWindow();
    }

    public void ExitBtnClicked()
    {
        Application.Quit();
    }

    public void GoToGameLobby()
    {
        SceneManager.LoadScene("GameLobby");
    }
  
}
