using System.Collections;
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
    private int currentUserCount; // 현재 유저 수 ( 필요한가? )
    private int maxUserCount; // 해당 방의 최대 수용가능 인원
    private int myPosition; // Client class field
    private int readyCount;

    private List<Client> redTeamPlayers; // 일단 정해진게없어서 string으로 함
    private List<Client> blueTeamPlayers;
    private bool isReady; // Client class field

    private const int MAXPLAYER_ON_EACHSIDE = 8; // 한팀에 최대 8명
    private static RoomContext instance;

    //GETTER SETTER
    public void ReverseReadyState() { isReady = !isReady; }
    public void SetRoomId(int val) { roomId = val; }
    //public void SetRoomName(string val) { roomName = val; }
    public List<Client> GetRedTeam() { return redTeamPlayers; }
    public List<Client> GetBlueTeam() { return blueTeamPlayers; }
    public int GetCurrentUserCount() { return currentUserCount; }
    public int GetMaxUserCount() { return maxUserCount; }
    public int GetMyPosition() { return myPosition; }
    public int GetRoomId() { return roomId; }
    public int GetRedTeamUserCount() { return redTeamPlayers.Count; }
    public int GetBlueTeamUserCount() { return blueTeamPlayers.Count; }
    public string GetRoomName() { return roomName; }
    public string GetRedTeamUserName(int index) { return redTeamPlayers[index].ToString(); }
    public string GetBlueTeamUserName(int index) { return blueTeamPlayers[index].ToString(); }
    public bool IsHost() { return EnterRoomType == EnterType.HOST; }
    public bool IsReady() { return isReady; }

    public void EnterRoomAsHost(string rName, int mCount) //test
    {   
        EnterRoomType = EnterType.HOST; 
        maxUserCount = mCount;
        currentUserCount = 1; 
        myPosition = 0; 
        isReady = false;
        roomName = rName;

        //test용
        AddUserToTeam("SELF", 0);
        AddUserToTeam("USER2", 1);
        AddUserToTeam("USER3", 2);
        AddUserToTeam("USER9", 8);
        AddUserToTeam("USER10", 9);
        AddUserToTeam("USER11", 10);
        AddUserToTeam("USER12", 11);
    }

    public void EnterRoomAsParticipant(string rName) 
    {
        //TODO : 서버에게서 입장이 가능하다는 응답이 오면 그 응답과 함께 온 정보를 이용해서 세팅해야함
        //아래 내용은 테스트
        EnterRoomType = EnterType.PARTICIPANT;
        myPosition = 6;
    }

    public void ChangeTeam(int prev, int next)
    {
        string userName = (prev < MAXPLAYER_ON_EACHSIDE) ? redTeamPlayers[prev].ToString() : blueTeamPlayers[prev % MAXPLAYER_ON_EACHSIDE].ToString();
        AddUserToTeam(userName, next);
        DeleteUserFromTeam(prev);

        if (prev == myPosition)
            myPosition = next;
    }

    public void AddUserToTeam(string name, int position)
    {
        //if (position < MAXPLAYER_ON_EACHSIDE)
        //    redTeamPlayers.Add(name);
        //else
        //    blueTeamPlayers.Add(name);
        //currentUserCount++;
    }

    public void DeleteUserFromTeam(int position)
    {
        if (position < MAXPLAYER_ON_EACHSIDE)
            redTeamPlayers.RemoveAt(position);
        else
            blueTeamPlayers.RemoveAt(position % MAXPLAYER_ON_EACHSIDE);
        currentUserCount--;
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
