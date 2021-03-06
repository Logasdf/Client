﻿using Google.Protobuf.Packet.Room;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomUIPainter : ScriptableObject {

    private GameObject eachUserPrefab;
    private GameObject redList;
    private GameObject blueList;
    private GameObject readyBtn;
    private GameObject startBtn;
    private GameObject errorWindow;
    private GameObject[] eachUserPrefabPool;

    private Text chatContents;
    private Text errorMessageTextField;
    private Text[] usernameTextArray;

    private ScrollRect chatScrollRect;

    private RoomContext rContext;

    private int blueteamStartIdx;
    private readonly Color readyColor = Color.yellow;
    private readonly Color notReadyColor = Color.black;


    public void Init(RoomContext roomContext)
    {
        readyBtn = GameObject.Find(ElementStrings.READY_BTN);
        startBtn = GameObject.Find(ElementStrings.START_BTN);
        rContext = roomContext;
        blueteamStartIdx = rContext.UserLimit / 2;

        CreateUserPrefabPool();
        AssignPrefabsToEachList();
        InitReadyButton();
        InitErrorWindow();
    }
    public void Draw()
    {   
        DrawRedTeam();
        DrawBlueTeam();
        if (rContext.Host)
            DisplayStartButton();
    }

    public void AddMessageToChatWindow(string msg)
    {
        chatContents.text += msg + "\n";
        chatScrollRect.verticalNormalizedPosition = 0;
    }

    public void DisplayErrorMessage(string msg)
    {
        errorMessageTextField.text = msg;
        errorWindow.SetActive(true);
    }

    private void OnEnable()
    {
        eachUserPrefab = (GameObject)Resources.Load(PathStrings.EACHUSER);
        redList = GameObject.Find(ElementStrings.REDTEAMLIST);
        blueList = GameObject.Find(ElementStrings.BLUETEAMLIST);
        chatContents = GameObject.Find(ElementStrings.CHAT_CONTENTHOLDER).GetComponent<Text>();
        chatScrollRect = GameObject.Find(ElementStrings.MESSAGE_PANEL).GetComponent<ScrollRect>();
        errorWindow = (GameObject)Instantiate(Resources.Load(PathStrings.ERROR_MESSAGE_PANEL));
        ChangeGridCellSize();
    }

    private void InitErrorWindow()
    {
        GameObject window = errorWindow.transform.Find(ElementStrings.ERRORMESSAGE_WINDOW).gameObject;
        errorMessageTextField = window.transform.Find(ElementStrings.ERRORMESSAGE_TEXTFIELD).GetComponent<Text>();

        Button closeButton = window.transform.Find(ElementStrings.CLOSE_BTN).GetComponent<Button>();
        closeButton.onClick.AddListener(delegate { errorWindow.SetActive(false); });

        GameObject mainCanvas = GameObject.Find(ElementStrings.CANVAS);
        errorWindow.transform.SetParent(mainCanvas.transform, false);
        errorWindow.transform.SetAsLastSibling();
        errorWindow.SetActive(false);
    }

    private void InitReadyButton()
    {
        readyBtn.SetActive(true);
        startBtn.SetActive(false);
    }

    private void DisplayStartButton()
    {
        readyBtn.SetActive(false);
        startBtn.SetActive(true);
    }

    private void CreateUserPrefabPool()
    {
        int size = rContext.UserLimit;
        eachUserPrefabPool = new GameObject[size];
        usernameTextArray = new Text[size];

        for (int i = 0; i < size; i++)
        {
            eachUserPrefabPool[i] = Instantiate(eachUserPrefab);
            usernameTextArray[i] = eachUserPrefabPool[i].transform.Find(ElementStrings.USERNAME_PANEL).GetComponent<Text>();
        }
    }

    private void AssignPrefabsToEachList()
    {
        int i = 0, size = rContext.UserLimit/2;
        for (; i < size; i++)
            eachUserPrefabPool[i].transform.SetParent(redList.transform, false);

        size += size;
        for (; i < size; i++)
            eachUserPrefabPool[i].transform.SetParent(blueList.transform, false);
    }

    private void ChangeGridCellSize()
    {
        Vector2 panelVector = redList.GetComponent<RectTransform>().sizeDelta;
        Vector2 newCellVector = new Vector2(panelVector.x, panelVector.y / 8);
        redList.GetComponent<GridLayoutGroup>().cellSize = newCellVector;
        blueList.GetComponent<GridLayoutGroup>().cellSize = newCellVector;
    }

    private void DrawRedTeam()
    {
        int i = 0, endIdx = rContext.RedteamCount;
        Client clnt;
        for(; i < endIdx; i++)
        {
            clnt = rContext.GetRedteamClient(i);
            usernameTextArray[i].text = clnt.Name;
            ChangeReadyStateColor(i, clnt.Ready);
        }

        for(; i < blueteamStartIdx; i++)
        {
            usernameTextArray[i].text = "";
            ChangeReadyStateColor(i, false);
        }
    }

    private void DrawBlueTeam()
    {
        int i = blueteamStartIdx, endIdx = i + rContext.BlueteamCount;
        Client clnt;
        for(; i < endIdx; i++)
        {
            clnt = rContext.GetBlueteamClient(i - blueteamStartIdx);
            usernameTextArray[i].text = clnt.Name;
            ChangeReadyStateColor(i, clnt.Ready);
        }

        endIdx = rContext.UserLimit;
        for(; i < endIdx; i++)
        {
            usernameTextArray[i].text = "";
            ChangeReadyStateColor(i, false);
        }
    }

    private void ChangeReadyStateColor(int index, bool isReady)
    {
        eachUserPrefabPool[index].GetComponent<Image>().color = isReady ? readyColor : notReadyColor;
    }

}
