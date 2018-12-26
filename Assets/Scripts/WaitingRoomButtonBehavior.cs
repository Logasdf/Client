using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitingRoomButtonBehavior : MonoBehaviour {

    public void ReadyBtnClick() // 준비버튼 눌렀을 때
    {
        Debug.Log("Ready btn clicked"); 
    }

    public void GoToGameLobby() // 방에서 나가기 눌렀을 때
    {
        SceneManager.LoadScene("GameLobby"); // 게임 로비로 돌아감
    }
}
