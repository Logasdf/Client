using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomUIMgr : MonoBehaviour {

    public GameObject eachUserPrefab; // 각 유저를 나타낼 때 사용할 prefab 외부연결

    private void Start()
    {
        //Debug.Log("This is a WaitingRoomUIMgr's Start()");

        //test
        GameObject redList = GameObject.Find("RedTeamList"); // 빨간팀 유저정보를 그려낼 패널
        GameObject blueList = GameObject.Find("BlueTeamList"); // 파란팀 유저정보를 그려낼 패널

        for(int i = 1; i <= 16; i++)
        {
            GameObject eachUser = Instantiate(eachUserPrefab); // 동적 생성
            eachUser.transform.Find("Username").GetComponent<Text>().text = "testuser " + i.ToString(); // 유저의 이름을 표시
            if (i % 2 != 0) // 테스트용으로 한명씩 돌아가면서 빨-파-빨-파 ........ 배정
                eachUser.transform.SetParent(redList.transform, false);
            else
                eachUser.transform.SetParent(blueList.transform, false);
        }

    }
}
