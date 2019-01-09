using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf.Packet.Room;
using UnityStandardAssets.Utility;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public GameObject pfPlayer;
    public GameObject pfTheOther;

    private bool isBoardActive;
    [SerializeField]
    private GameObject statusBoard;
    [SerializeField]
    private GameObject pfUserInfo;
    private GameObject redTeamInfo;
    private GameObject blueTeamInfo;
    private GameObject[] userInfoPool;

    private Transform[] redTeamSpawns;
    private Transform[] blueTeamSpawns;

    private RoomContext roomContext;
    private int numOfRed;
    private int numOfBlue;
    private List<Client> redTeamPlayers;
    private List<Client> blueTeamPlayers;

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
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {
        //roomContext = RoomContext.GetInstance();
        //redTeamPlayers = roomContext.GetRedTeam();
        //numOfRed = redTeamPlayers.Count;
        //blueTeamPlayers = roomContext.GetBlueTeam();
        //numOfBlue = blueTeamPlayers.Count;

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

        // 현재는 테스트용, 나중에 접속한 Client Red Team에 맞게 조건부 수정.
        // Red 1이 Client로 가정.
        for (int i = 1; i < redTeamSpawns.Length; ++i)
        {
            GameObject redPlayer;
            if (i == 1)
            {
                redPlayer = Instantiate(pfPlayer, redTeamSpawns[i].transform);
                SmoothFollow sf = GameObject.Find("Main Camera").GetComponent<SmoothFollow>();
                sf.setTarget(redPlayer.transform);
            }
            else
            {
                redPlayer = Instantiate(pfTheOther, redTeamSpawns[i].transform);
            }
            redPlayer.transform.SetParent(GameObject.Find("/RedTeam").transform);
            redPlayer.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        for (int i = 1; i < blueTeamSpawns.Length; ++i)
        {
            GameObject bluePlayer = Instantiate(pfTheOther, blueTeamSpawns[i].transform);
            bluePlayer.transform.SetParent(GameObject.Find("/BlueTeam").transform);
            bluePlayer.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
    }

    private void CreateUserInfoPool(int size=16)
    {
        userInfoPool = new GameObject[size];
        for(int i = 0; i < size; ++i)
        {
            userInfoPool[i] = Instantiate(pfUserInfo);
            if (i < 8) userInfoPool[i].transform.SetParent(redTeamInfo.transform, false);
            else userInfoPool[i].transform.SetParent(blueTeamInfo.transform, false);
        }
    }

    private void SetUserInfoPool()
    {

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
}

