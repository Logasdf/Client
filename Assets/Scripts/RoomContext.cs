﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf.Packet.Room;
using System.Net;
using System.Net.Sockets;

public class RoomContext {

    private enum EnterType
    {
        HOST, PARTICIPANT
    }

    private EnterType EnterRoomType; // 입장형식 { 방장 / 참가자 }
    private string roomName;
    private string myUsername;
    private int roomId;
    private int currentUserCount; // 현재 유저 수 ( 필요한가? )
    private int maxUserCount; // 해당 방의 최대 수용가능 인원
    private int myPosition; // Client class field
    private int readyCount;

    private List<Client> redTeamPlayers; // 일단 정해진게없어서 string으로 함
    private List<Client> blueTeamPlayers;
    private bool isReady; // Client class field

    private const int BLUEINDEXSTART = 8; // 한팀에 최대 8명
    private static RoomContext instance;

    //GETTER SETTER
    public void SetRoomId(int val) { roomId = val; }
    public void SetUsername(string val) { myUsername = val; }
    //public void SetRoomName(string val) { roomName = val; }
    public int GetCurrentUserCount() { return currentUserCount; }
    public int GetMaxUserCount() { return maxUserCount; }
    public int GetMyPosition() { return myPosition; }
    public int GetRoomId() { return roomId; }
    public int GetRedTeamUserCount() { return redTeamPlayers.Count; }
    public int GetBlueTeamUserCount() { return blueTeamPlayers.Count; }
    public string GetRoomName() { return roomName; }
    public string GetMyUsername() { return myUsername; }
    public Client GetClient(int index) { return index < BLUEINDEXSTART ? 
            GetRedTeamClient(index) : GetBlueTeamClient(index % BLUEINDEXSTART); }
    public Client GetRedTeamClient(int index) { return redTeamPlayers[index]; }
    public Client GetBlueTeamClient(int index) { return blueTeamPlayers[index]; }
    public bool IsHost() { return EnterRoomType == EnterType.HOST; }
    public bool IsReady() { return isReady; }

    public void ReverseReadyState(int index)
    {
        GetClient(index).Ready = !GetClient(index).Ready;
        if (index == myPosition)
            isReady = !isReady;
    }

    public void ChangeTeam(int prev, int next)
    {
        Client user = GetClient(prev);
        AddUserToTeam(user, next);
        DeleteUserFromTeam(prev);
    }

    public void AddUserToTeam(Client user, int position)
    {
        if (position < BLUEINDEXSTART)
            redTeamPlayers.Add(user);
        else
            blueTeamPlayers.Add(user);
        currentUserCount++;
    }

    public void DeleteUserFromTeam(int position)
    {
        if (position < BLUEINDEXSTART)
            redTeamPlayers.RemoveAt(position);
        else
            blueTeamPlayers.RemoveAt(position % BLUEINDEXSTART);
        currentUserCount--;

        if((myPosition < BLUEINDEXSTART && position < BLUEINDEXSTART) || (myPosition >= BLUEINDEXSTART && position >= BLUEINDEXSTART))
        {
            if(myPosition > position)
            {
                myPosition--;
            }
            else if(myPosition == position)
            {
                myPosition = SeekMyPosition();
            }
        }
    }

    public static RoomContext GetInstance() {
		// 싱글턴 제대로 필요 지금은 테스트중이라
        if(instance == null)
        {
            instance = new RoomContext();
        }

        return instance;
	}

    public void InitRoomContext(RoomInfo room)
    {
        roomId = room.RoomId;
        roomName = room.Name;
        readyCount = room.ReadyCount;
        currentUserCount = room.Current;
        maxUserCount = room.Limit;
       
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

        myPosition = SeekMyPosition();

        if (myPosition != room.Host)
            EnterRoomType = EnterType.PARTICIPANT;
        else
            EnterRoomType = EnterType.HOST;
    }

    private int SeekMyPosition()
    {
        for (int i = 0; i < GetRedTeamUserCount(); i++)
            if (GetRedTeamClient(i).Name == myUsername) 
                return i;
        for (int i = 0; i < GetBlueTeamUserCount(); i++)
            if (GetBlueTeamClient(i).Name == myUsername) 
                return i + 8;
        return -1;
    }

    private RoomContext() { }
}
