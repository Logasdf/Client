﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf.Packet.Room;

public class RoomContext {
    //테스트용 클래스

    private enum EnterType
    {
        HOST, PARTICIPANT
    }

    private EnterType EnterRoomType; // 입장형식 { 방장 / 참가자 }
    private string roomName;
    private int roomId;
    private int currentUserCount; // 현재 유저 수
    private int maxUserCount; // 해당 방의 최대 수용가능 인원
    private int myPosition; // Client class field
    private int readyCount;

    private List<Client> redTeamPlayers; // 일단 정해진게없어서 string으로 함
    private List<Client> blueTeamPlayers;
    private int redTeamArrIdx; // 빨간팀 다음 추가 인덱스
    private int blueTeamArrIdx; // 파란팀 다음 추가 인덱스
    private bool isReady; // Client class field

    private const int MAX_PLAYER = 8; // 한팀에 최대 8명
    private static RoomContext instance;

    //GETTER SETTER
    public void ReverseReadyState() { isReady = !isReady; }
    public void SetRoomId(int val) { roomId = val; }
    //public void SetRoomName(string val) { roomName = val; }
    public List<Client> GetRedTeam() { return redTeamPlayers; }
    public List<Client> GetBlueTeam() { return blueTeamPlayers; }
    public int GetCurrentUserCount() { return currentUserCount; }
    public int GetRedTeamIndex() { return redTeamArrIdx; }
    public int GetBlueTeamIndex() { return blueTeamArrIdx; }
    public int GetMyPosition() { return myPosition; }
    public int GetRoomId() { return roomId; }
    public string GetRoomName() { return roomName; }
    public bool IsHost() { return EnterRoomType == EnterType.HOST; }
    public bool IsReady() { return isReady; }

    public void EnterRoomAsHost(string rName, int mCount)
    {   
        EnterRoomType = EnterType.HOST; //test
        maxUserCount = mCount;
        currentUserCount = 1; // test
        myPosition = 0; // test
        isReady = false;
        roomName = rName;
        //AddUserToRedTeam("SELF"); // 자기 아이피랑 포트 저장해놓을 필요가 생겼다.
    }

    public void EnterRoomAsParticipant(string rName) 
    {
        //TODO : 서버에게서 입장이 가능하다는 응답이 오면 그 응답과 함께 온 정보를 이용해서 세팅해야함
        //아래 내용은 테스트
        EnterRoomType = EnterType.PARTICIPANT;
        myPosition = 6;
    }

    public static RoomContext GetInstance() {
		// 싱글턴 제대로 필요 지금은 테스트중이라
        if(instance == null)
        {
            instance = new RoomContext();
        }

        return instance;
	}

    public void InitRoomContext(Room room)
    {
        roomId = room.RoomId;
        roomName = room.Name;
        readyCount = room.ReadyCount;
        currentUserCount = room.Current;
        maxUserCount = room.Limit;
        myPosition = room.MyPosition;

        redTeamPlayers = new List<Client>();
        foreach (var clnt in room.RedTeam)
        {
            redTeamPlayers.Add(clnt);
        }
        blueTeamPlayers = new List<Client>();
        foreach (var clnt in room.BlueTeam)
        {
            blueTeamPlayers.Add(clnt);
        }
    }

    private RoomContext()
    {   //initialize
        //redTeamPlayers = new List<string>();
        //blueTeamPlayers = new List<string>();
        //redTeamArrIdx = blueTeamArrIdx = 0;
    }
}
