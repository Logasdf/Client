using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitingRoomUIMgr : MonoBehaviour {

    public GameObject eachUserPrefab;

    private GameObject redList;
    private GameObject blueList;
    private RoomContext roomContext;
    private PacketManager packetManager;
    private Color readyColor = Color.yellow;
    private Color notReadyColor = Color.black;
    private GameObject[] eachUserPrefabPool;
    

    public void RequestProcessForReadyButton()
    {
        ProcessReadyButtonClickEvent();  
    }

    public void RequestProcessForLeaveButton()
    {
        if (roomContext.IsReady()) // 있어도 그만 없어도 그만
            return;
        //여기서는 서버에 방나간다는 사실을 알려주고, 서버에서는 이 유저가 방장이냐 아니냐에 따라 적절한 처리를 해야한다.
        SceneManager.LoadScene("GameLobby");
    }

    public void RequestProcessForTeamChange()
    {
        ProcessTeamChangeEvent();
    }

    private void Start()
    {
        redList = GameObject.Find("RedTeamList"); 
        blueList = GameObject.Find("BlueTeamList");
        roomContext = GameObject.Find("RoomContextHolder").GetComponent<RoomContext>();
        packetManager = GameObject.Find("PacketManager").GetComponent<PacketManager>();
        CreateUserPrefabPool();
        DrawUsers();
    }
    
    private void CreateUserPrefabPool()
    { 
        const int USERPREFAB_POOL_MAX = 16;
        eachUserPrefabPool = new GameObject[USERPREFAB_POOL_MAX];
        for(int i = 0; i<USERPREFAB_POOL_MAX; i++)
        {
           eachUserPrefabPool[i] = Instantiate(eachUserPrefab);
        }
    }

    private void DrawUsers()
    {
        //TODO : ROOMCONTEXT 속에 들어있는 정보들을 이용해서 그려내기
        //int redTeamMaxIdx = roomContext.GetRedTeamIndex();
        //int blueTeamMaxIdx = roomContext.GetBlueTeamIndex();
        int redTeamMaxIdx = 8; //test
        int blueTeamMaxIdx = 8 + 8; // test

        for(int i = 0; i < redTeamMaxIdx; i++) AddUserPrefabAsChildToList(i, "user"+ ( i + 1 ), redList.transform);
        for(int i = 8; i < blueTeamMaxIdx; i++) AddUserPrefabAsChildToList(i, "user" + (i + 1), blueList.transform);
    }

    private void AddUserPrefabAsChildToList(int index, string name, Transform parent)
    {
        Text userName = eachUserPrefabPool[index].transform.Find("UserName").GetComponent<Text>();
        userName.text = name;
        eachUserPrefabPool[index].transform.SetParent(parent, false);
    }

    private void ProcessReadyButtonClickEvent()
    {
        //버튼 클릭
        //TODO : **서버에 준비완료 메시지 전송**

        //pool[myPosition] => 해당 위치 게임오브젝트 찾을 수 있음
        ChangeReadyStateColor(roomContext.GetMyPosition(), !roomContext.IsReady());
        roomContext.ReverseReadyState();
    }

    private void ProcessTeamChangeEvent()
    {

        //준비완료상태라면 해당 요청을 거부한다.
        if (roomContext.IsReady())
            return;
        //서버에 요청을 보내서 이동이 가능한지 응답을 받는다.
        
        /* 이건 다른함수에서..
         * TODO :
         * 5. 현재 나의 팀의 유저정보 배열에서 나를 제외하고 배열을 정리한다.
         * 6. 현재 나의 팀의 idx를 감소시킨다.
         * 7. 이동할 팀의 유저정보 배열의 idx인덱스에 나의 정보를 삽입한다.
         * 8. 이동한 팀의 idx를 1 증가시킨다.
         * myPosition을 변경하고, 다시 그려낸다.
         */

    }

    private void ChangeReadyStateColor(int index, bool toReady)
    {
        eachUserPrefabPool[index].GetComponent<Image>().color = toReady ? readyColor : notReadyColor;
    }
} 
