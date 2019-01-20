using UnityEngine;
using Google.Protobuf.Packet.Room;

public class RoomContext {

    public int UserLimit { get { return roomInfo.Limit; } }
    public int RoomId { get { return roomInfo.RoomId; } }
    public int RedteamCount { get { return roomInfo.RedTeam.Count; } }
    public int BlueteamCount { get { return roomInfo.BlueTeam.Count; } }
    public bool Host { get { return myPosition == roomInfo.Host ? true : false; } }

    private static RoomContext instance;
    private RoomInfo roomInfo;
    private int myPosition;
    private string myUserName;

    public void SetUsername(string val) { myUserName = val; }
    public void SetRoomInfo(RoomInfo val) { roomInfo = val; }
    public void SetMyPosition(int val) { myPosition = val; }
    
    public int GetMyPosition() { return myPosition; }
    public string GetMyUsername() { return myUserName; }
    public Client GetRedteamClient(int idx) { return roomInfo.RedTeam[idx]; }
    public Client GetBlueteamClient(int idx) { return roomInfo.BlueTeam[idx]; }

    public static RoomContext GetInstance()
    {
        // 싱글턴 제대로 필요 지금은 테스트중이라
        if (instance == null)
        {
            instance = new RoomContext();
        }

        return instance;
    }

    private RoomContext() { }
}
