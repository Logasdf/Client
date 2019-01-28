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

    // 상황판
    private bool isBoardActive;
    private GameObject redTeamInfo;
    private GameObject blueTeamInfo;
    private GameObject[] userInfoPool;
    private Text[,] userInfo;

    [SerializeField]
    private GameObject statusBoard;
    [SerializeField]
    private GameObject pfUserInfo;

    // 스폰지역
    private Transform[] redTeamSpawns;
    private Transform[] blueTeamSpawns;

    private RoomContext roomContext;

    private PlayerContext myContext;
    public PlayerContext MyContext { get { return myContext;  } }

    private PlayerContext[] redPlayers;
    private PlayerContext[] bluePlayers;
    private Dictionary<string, PlayerContext> playerMap; // <ClntName, PlayerConetext>

    private PacketManager _packetManager;
    public PacketManager PacketManager { get { return _packetManager; } }

    private KillLogMgr killLogMgr;
    public KillLogMgr KillLogMgr { get { return killLogMgr; } }

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
        _packetManager = GameObject.Find(ElementStrings.PACKETMANAGER).GetComponent<PacketManager>();
        _packetManager.SetHandleMessage(PopMessage);
        killLogMgr = GetComponent<KillLogMgr>();
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {
        playerMap = new Dictionary<string, PlayerContext>();
        roomContext = RoomContext.GetInstance();

        redPlayers = new PlayerContext[roomContext.RedteamCount];
        for (int i = 0; i < redPlayers.Length; ++i)
            redPlayers[i] = ScriptableObject.CreateInstance<PlayerContext>();
        bluePlayers = new PlayerContext[roomContext.BlueteamCount];
        for (int i = 0; i < bluePlayers.Length; ++i)
            bluePlayers[i] = ScriptableObject.CreateInstance<PlayerContext>();

        InitStatusBoard();
        SpawnGamePlayers();
	}
	
	// Update is called once per frame
	void Update () {
        ToggleStatusBoard();
	}

    private void LateUpdate()
    {
        UpdateUserInfoPool();
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
        Transform respawnPos;
        int redCount = roomContext.RedteamCount;
        int myPosition = roomContext.GetMyPosition();
        bool isRedTeam = (myPosition < BLUEINDEXSTART);
        for(int i = 0; i < redCount; ++i)
        {
            clnt = roomContext.GetRedteamClient(i);
            respawnPos = redTeamSpawns[clnt.Position + 1].transform;
            player = Instantiate((myPosition == clnt.Position) ? pfPlayer : pfTheOther, respawnPos);
            player.GetComponent<MeshRenderer>().material.color = Color.red;
            player.name = clnt.Name;
            player.tag = (isRedTeam) ? ElementStrings.MYTEAMTAG : ElementStrings.ENEMYTAG;
            player.transform.SetParent(transform.Find("/RedTeam"));

            redPlayers[clnt.Position].Init(roomContext.RoomId, clnt, player, respawnPos);
            playerMap[clnt.Name] = redPlayers[clnt.Position];
        }

        int blueCount = roomContext.BlueteamCount;
        for(int i = 0; i < blueCount; ++i)
        {
            clnt = roomContext.GetBlueteamClient(i);
            respawnPos = blueTeamSpawns[clnt.Position % BLUEINDEXSTART + 1].transform;
            player = Instantiate((myPosition == clnt.Position) ? pfPlayer : pfTheOther, respawnPos);
            player.GetComponent<MeshRenderer>().material.color = Color.blue;
            player.name = clnt.Name;
            player.tag = (!isRedTeam) ? ElementStrings.MYTEAMTAG : ElementStrings.ENEMYTAG;
            player.transform.SetParent(transform.Find("/BlueTeam"));

            bluePlayers[clnt.Position % BLUEINDEXSTART].Init(roomContext.RoomId, clnt, player, respawnPos);
            playerMap[clnt.Name] = bluePlayers[clnt.Position % BLUEINDEXSTART];
        }

        myContext = (myPosition < BLUEINDEXSTART) ? redPlayers[myPosition] : bluePlayers[myPosition % BLUEINDEXSTART];
        SmoothFollow sf = GameObject.Find(ElementStrings.MAINCAMERA).GetComponent<SmoothFollow>();
        sf.setTarget(myContext.Player.transform);

        SetUserInfoPool();
    }

    private void UpdateWorldState(WorldState worldState)
    {
        if(worldState.ClntName != myContext.Client.Name)
        {
            GameObject player = playerMap[worldState.ClntName].Player;
            Text healthText = player.transform.GetChild(1).GetChild(0).GetComponent<Text>();
            playerMap[worldState.ClntName].UpdateTransform(worldState.Transform);
            playerMap[worldState.ClntName].WorldState.Health = worldState.Health;
            playerMap[worldState.ClntName].WorldState.KillPoint = worldState.KillPoint;
            playerMap[worldState.ClntName].WorldState.DeathPoint = worldState.DeathPoint;
            playerMap[worldState.ClntName].WorldState.AnimState = worldState.AnimState;

            if (worldState.AnimState == PlayerContext.DEATH)
                healthText.text = "Death";
            else
                healthText.text = worldState.Health.ToString();

            if (worldState.Fired)
            {
                Transform firePos = player.transform.GetChild(0);
                GameObject _bullet = Instantiate(pfBullet, firePos.position, firePos.rotation);
                _bullet.GetComponent<BulletCtrl>().Shooter = worldState.ClntName;
                Debug.Log(_bullet.GetComponent<BulletCtrl>().Shooter);
            }

            if(worldState.Hit && worldState.AnimState == PlayerContext.DEATH)
            {
                HitState hitState = worldState.HitState;
                killLogMgr.PostKillLog(hitState.From, hitState.To);
                if (hitState.From == myContext.Client.Name)
                {
                    //만약 내가 죽였다면, kill point 올리기
                    myContext.WorldState.KillPoint++;
                }
            }
        }
    }

    private void PopMessage(object obj, Type type)
    {
        if(obj == null)
        {
            Debug.Log("Object is null...");
            return;
        }

        if (type.Name == MessageTypeStrings.WORLDSTATE)
        {
            UpdateWorldState((WorldState)obj);
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

    private void CreateUserInfoPool(int size = 16)
    {
        userInfoPool = new GameObject[size];
        userInfo = new Text[size, 2];
        for (int i = 0; i < size; ++i)
        {
            userInfoPool[i] = Instantiate(pfUserInfo);
            userInfo[i, 0] = userInfoPool[i].transform.Find(ElementStrings.USERNAME_PANEL).GetComponent<Text>();
            userInfo[i, 1] = userInfoPool[i].transform.Find(ElementStrings.KDASTATE).GetComponent<Text>();
            
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

    private void SetUserInfoPool()
    {
        // red team
        for (int i = 0; i < redPlayers.Length; ++i)
        {
            userInfo[i, 0].text = redPlayers[i].Client.Name;
            userInfo[i, 1].text = string.Format("{0, 3:D3}/{1, 3:D3}",
                redPlayers[i].WorldState.KillPoint, redPlayers[i].WorldState.DeathPoint);
        }

        // blue team
        for (int i = 0; i < bluePlayers.Length; ++i)
        {
            userInfo[i + BLUEINDEXSTART, 0].text = bluePlayers[i].Client.Name;
            userInfo[i + BLUEINDEXSTART, 1].text = string.Format("{0, 3:D3}/{1, 3:D3}",
                bluePlayers[i].WorldState.KillPoint, bluePlayers[i].WorldState.DeathPoint);
        }
    }

    private void UpdateUserInfoPool()
    {
        // red team
        for (int i = 0; i < redPlayers.Length; ++i)
        {
            userInfo[i, 1].text = string.Format("{0}/{1}",
                redPlayers[i].WorldState.KillPoint, redPlayers[i].WorldState.DeathPoint);
        }

        // blue team
        for (int i = 0; i < bluePlayers.Length; ++i)
        {
            userInfo[i + BLUEINDEXSTART, 1].text = string.Format("{0}/{1}",
                bluePlayers[i].WorldState.KillPoint, bluePlayers[i].WorldState.DeathPoint);
        }
    }

    private void ChangeGridCellSize()
    {
        Vector2 panelVector = redTeamInfo.GetComponent<RectTransform>().sizeDelta;
        Vector2 newCellVector = new Vector2(panelVector.x, panelVector.y / 8);
        redTeamInfo.GetComponent<GridLayoutGroup>().cellSize = newCellVector;
        blueTeamInfo.GetComponent<GridLayoutGroup>().cellSize = newCellVector;
    }
}