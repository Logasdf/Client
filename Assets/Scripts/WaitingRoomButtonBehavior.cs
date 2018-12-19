using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitingRoomButtonBehavior : MonoBehaviour {

    public void ReadyBtnClick()
    {
        Debug.Log("Ready btn clicked");
    }

    public void GoToGameLobby()
    {
        SceneManager.LoadScene("GameLobby");
    }
}
