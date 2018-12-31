using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitingRoomUIMgr : MonoBehaviour {

    private RoomContext roomContext;
    private PacketManager packetManager;
    private WaitingRoomUIDrawer drawer;

    public void ProcessReadyButtonEvent()
    {
        ProcessReadyButtonClickEvent();  
    }

    public void ProcessLeaveButtonEvent()
    {
        if (roomContext.IsReady()) // 있어도 그만 없어도 그만
            return;
        //여기서는 서버에 방나간다는 사실을 알려주고, 서버에서는 이 유저가 방장이냐 아니냐에 따라 적절한 처리를 해야한다.
        SceneManager.LoadScene("GameLobby");
    }

    public void RequestProcessForTeamChange()
    {
        ProcessTeamChangeEventRequest();
    }

    private void Start()
    {
        roomContext = RoomContext.GetInstance();
        packetManager = GameObject.Find("PacketManager").GetComponent<PacketManager>();
        drawer = (WaitingRoomUIDrawer)ScriptableObject.CreateInstance("WaitingRoomUIDrawer");
    }

    private void ProcessReadyButtonClickEvent()
    {
        //버튼 클릭
        //TODO : **서버에 준비완료 메시지 전송**
        Data request = new Data();
        request.DataMap.Add("content_type", "READY_EVENT");
        request.DataMap.Add("roomId", roomContext.GetRoomId().ToString());
        request.DataMap.Add("position", roomContext.GetMyPosition().ToString());
        request.DataMap.Add("ready", roomContext.IsReady() ? "true" : "false");

        //packetManager.SerializeAndSend(request);

        drawer.ChangeReadyStateColor(roomContext.GetMyPosition(), !roomContext.IsReady());
        roomContext.ReverseReadyState();
    }

    private void ProcessTeamChangeEventRequest()
    {

        //준비완료상태라면 해당 요청을 거부한다.
        if (roomContext.IsReady())
            return;
        //서버에 요청을 보내서 이동이 가능한지 응답을 받는다.
        Data data = new Data();
        data.DataMap.Add("content_type", "TEAM_CHANGE");
        data.DataMap.Add("roomId", roomContext.GetRoomId().ToString());
        data.DataMap.Add("position", roomContext.GetMyPosition().ToString());

        //packetManager.SerializeAndSend(data);
    }

    private void ProcessTeamChangeEventResponse()
    {
        /*
         * TODO :
         * 5. 현재 나의 팀의 유저정보 배열에서 나를 제외하고 배열을 정리한다.
         * 6. 현재 나의 팀의 idx를 감소시킨다.
         * 7. 이동할 팀의 유저정보 배열의 idx인덱스에 나의 정보를 삽입한다.
         * 8. 이동한 팀의 idx를 1 증가시킨다.
         * myPosition을 변경하고, 다시 그려낸다.
         */
    }

} 
