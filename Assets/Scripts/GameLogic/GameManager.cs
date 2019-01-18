using System.Collections;
using System.Collections.Generic;

using Google.Protobuf.State;
using Google.Protobuf.Packet.Room;

using Assets.Scripts.GameLogic.Context;
using UnityStandardAssets.Utility;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    private const int BLUEINDEXSTART = 8;

    public GameObject pfPlayer;
    public GameObject pfTheOther;
    public GameObject pfBullet;

    private bool isBoardActive;
    [SerializeField]
    private GameObject statusBoard;
    [SerializeField]
    private GameObject pfUserInfo;
    private GameObject redTeamInfo;
    private GameObject blueTeamInfo;
    private GameObject[] userInfoPool;
    private Text[,] userInfo;

    private Transform[] redTeamSpawns;
    private Transform[] blueTeamSpawns;

    private RoomContext roomContext;
    private PlayerContext myContext;
    public PlayerContext MyContext { get { return myContext;  } }
    private PlayerContext[] redPlayers;
    private PlayerContext[] bluePlayers;
    private Dictionary<string, PlayerContext> playerMap;

    private PacketManager _packetManager;
    public PacketManager PacketManager { get { return _packetManager; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
        _packetManager = GameObject.Find("PacketManager").GetComponent<PacketManager>();
        _packetManager.SetHandleMessage(PopMessage);
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {
        playerMap = new Dictionary<string, PlayerContext>();
        roomContext = RoomContext.GetInstance();
        redPlayers = new PlayerContext[roomContext.GetRedTeamUserCount()];
        for (int i = 0; i < redPlayers.Length; ++i)
            redPlayers[i] = ScriptableObject.CreateInstance<PlayerContext>();
        bluePlayers = new PlayerContext[roomContext.GetBlueTeamUserCount()];
        for (int i = 0; i < bluePlayers.Length; ++i)
            bluePlayers[i] = ScriptableObject.CreateInstance<PlayerContext>();

        InitStatusBoard();
        SpawnGamePlayers();
	}
	
	// Update is called once per frame
	void Update () {
        ToggleStatusBoard();
	}

    private void ToggleStatusBoard()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyUp(KeyCode.Tab))
        {
            isBoardActive = !isBoardActive;
            statusBoard.SetActive(isBoardActive);
        }
    }

    private void SpawnGamePlayers()
    {
        redTeamSpawns = transform.GetChild(0).GetChild(0).GetComponentsInChildren<Transform>();
        blueTeamSpawns = transform.GetChild(0).GetChild(1).GetComponentsInChildren<Transform>();

        Client clnt;
        GameObject player;
        int redCount = roomContext.GetRedTeamUserCount();
        int myPosition = roomContext.GetMyPosition();
        bool isRedTeam = (myPosition < BLUEINDEXSTART);
        for(int i = 0; i < redCount; ++i)
        {
            clnt = roomContext.GetRedTeamClient(i);
            player = Instantiate((myPosition == clnt.Position) ? pfPlayer : pfTheOther,
                redTeamSpawns[clnt.Position + 1].transform);
            player.GetComponent<MeshRenderer>().material.color = Color.red;
            player.name = clnt.Name;
            player.tag = (isRedTeam) ? "MYTEAM" : "ENEMY";
            player.transform.SetParent(transform.Find("/RedTeam"));

            redPlayers[clnt.Position].Init(roomContext.GetRoomId(), clnt, player);
            playerMap[clnt.Name] = redPlayers[clnt.Position];
        }

        int blueCount = roomContext.GetBlueTeamUserCount();
        for(int i = 0; i < blueCount; ++i)
        {
            clnt = roomContext.GetBlueTeamClient(i);
            player = Instantiate((myPosition == clnt.Position) ? pfPlayer : pfTheOther,
                blueTeamSpawns[clnt.Position % BLUEINDEXSTART + 1].transform);
            player.GetComponent<MeshRenderer>().material.color = Color.blue;
            player.name = clnt.Name;
            player.tag = (!isRedTeam) ? "MYTEAM" : "ENEMY";
            player.transform.SetParent(transform.Find("/BlueTeam"));

            bluePlayers[clnt.Position % BLUEINDEXSTART].Init(roomContext.GetRoomId(), clnt, player);
            playerMap[clnt.Name] = bluePlayers[clnt.Position % BLUEINDEXSTART];
        }

        myContext = (myPosition < BLUEINDEXSTART) ? redPlayers[myPosition] : bluePlayers[myPosition % BLUEINDEXSTART];
        SmoothFollow sf = GameObject.Find("Main Camera").GetComponent<SmoothFollow>();
        sf.setTarget(myContext.Player.transform);

        SetUserInfoPool();
    }

    private void SetUserInfoPool()
    {
        //// red team
        //for(int i = 0; i < redPlayers.Length; ++i)
        //{
        //    userInfo[i, 0].text = redPlayers[i].Client.Name;
        //    userInfo[i, 1].text = string.Format("{0, 3:D3}/{1, 3:D3}",
        //        redPlayers[i].State.KillCount, redPlayers[i].State.DeathCount);
        //}

        //// blue team
        //for(int i = 0; i < bluePlayers.Length; ++i)
        //{
        //    userInfo[i + BLUEINDEXSTART, 0].text = bluePlayers[i].Client.Name;
        //    userInfo[i + BLUEINDEXSTART, 1].text = string.Format("{0, 3:D3}/{1, 3:D3}",
        //        bluePlayers[i].State.KillCount, bluePlayers[i].State.DeathCount);
        //}
    }

    private void CreateUserInfoPool(int size=16)
    {
        userInfoPool = new GameObject[size];
        userInfo = new Text[size, 2];
        for(int i = 0; i < size; ++i)
        {
            userInfoPool[i] = Instantiate(pfUserInfo);
            userInfo[i, 0] = userInfoPool[i].transform.Find("UserName").GetComponent<Text>();
            userInfo[i, 1] = userInfoPool[i].transform.Find("KDA").GetComponent<Text>();

            if (i < 8)
            {
                userInfoPool[i].transform.SetParent(redTeamInfo.transform, false);
                
            }
            else
            {
                userInfoPool[i].transform.SetParent(blueTeamInfo.transform, false);
            }
        }
    }

    private void InitStatusBoard()
    {
        isBoardActive = false;
        redTeamInfo = statusBoard.transform.GetChild(1).GetChild(2).gameObject;
        blueTeamInfo = statusBoard.transform.GetChild(1).GetChild(3).gameObject;
        ChangeGridCellSize();
        CreateUserInfoPool();
    }

    private void ChangeGridCellSize()
    {
        Vector2 panelVector = redTeamInfo.GetComponent<RectTransform>().sizeDelta;
        Vector2 newCellVector = new Vector2(panelVector.x, panelVector.y / 8);
        redTeamInfo.GetComponent<GridLayoutGroup>().cellSize = newCellVector;
        blueTeamInfo.GetComponent<GridLayoutGroup>().cellSize = newCellVector;
    }

    private void UpdateWorldState(WorldState worldState)
    {
        if(worldState.ClntName != myContext.Client.Name)
        {
            playerMap[worldState.ClntName].UpdateTransform(worldState.Transform);
            if(worldState.Fired)
            {
                GameObject firer = playerMap[worldState.ClntName].Player;
                Transform firePos = firer.transform.GetChild(0);
                Instantiate(pfBullet, firePos.position, firePos.rotation);
            }
        }
    }

    private void UpdatePlayerState(PlayState playState)
    {
        if (playState.ClntName != myContext.Client.Name)
        {
            playerMap[playState.ClntName].UpdateTransform(playState.Transform);
        }
    }

    public void SendFireBulletEvent(string fromClnt)
    {
        Debug.Log(string.Format("{0} in Room {1} is fired!", fromClnt, roomContext.GetRoomId()));

        Data request = new Data();
        request.DataMap["contentType"] = "FIRE_BULLET";
        request.DataMap["roomId"] = Convert.ToString(roomContext.GetRoomId());
        request.DataMap["fromClnt"] = fromClnt;

        _packetManager.PackMessage(protoObj: request);
    }

    private void HandleFireBulletEvent(string fromClnt)
    {
        //Debug.Log(string.Format("{0} was fired!", fromClnt));
        if (fromClnt == myContext.Player.name) return;

        GameObject firer = playerMap[fromClnt].Player;

        Transform firePos = firer.transform.GetChild(0);
        //Debug.Log(string.Format("FirePos Name is {0}", firePos.gameObject.name));
        Instantiate(pfBullet, firePos.position, firePos.rotation);
    }

    public void SendBeShotEvent(string fromClnt, string toClnt)
    {
        Data request = new Data();
        request.DataMap["contentType"] = "BE_SHOT";
        request.DataMap["roomId"] = Convert.ToString(roomContext.GetRoomId());
        request.DataMap["fromClnt"] = fromClnt;
        request.DataMap["toClnt"] = toClnt;
        //request.DataMap["hitType"] = "";

        _packetManager.PackMessage(protoObj: request);
    }

    private void HandleBeShotEvent()
    {

    }

    private void PopMessage(object obj, Type type)
    {
        if (type.Name == "Data")
        {
            //Data response = (Data)obj;
            //string contentType = response.DataMap["contentType"];
            //switch (contentType)
            //{
            //    case "FIRE_BULLET":
            //        string fromClnt = response.DataMap["fromClnt"];
            //        HandleFireBulletEvent(fromClnt);
            //        break;
            //    case "BE_SHOT":
            //        fromClnt = response.DataMap["fromClnt"];
            //        string toClnt = response.DataMap["toClnt"];
            //        Debug.Log(string.Format("{0} was shot by {1}", toClnt, fromClnt));
            //        //HandleBeShotEvent();
            //        break;
            //}
        }
        else if (type.Name == "PlayState")
        {
            Debug.Log("Update PlayState");
            UpdatePlayerState((PlayState)obj);
        }
        else if (type.Name == "WorldState")
        {
            Debug.Log("Update WorldState");
            UpdateWorldState((WorldState)obj);
        }
    }
}