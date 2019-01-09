using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomUIPainter : ScriptableObject {

    public enum DrawType
    {
        REDONLY, BLUEONLY, BOTH
    }
    private GameObject eachUserPrefab;
    private GameObject redList;
    private GameObject blueList;
    private GameObject[] eachUserPrefabPool;

    private Text chatContents;
    private Text[] usernameTextArray;

    private readonly Color readyColor = Color.yellow;
    private readonly Color notReadyColor = Color.black;


    public void Init(int size, bool isHost)
    {
        CreateUserPrefabPool(size);
        AssignPrefabsToEachList(size/2);
        DisplayAppropriateButton(isHost);
    }

    public void ChangeReadyStateColor(int index, bool toReady)
    {   
        eachUserPrefabPool[index].GetComponent<Image>().color = toReady ? readyColor : notReadyColor;
    }

    public void DisplayAppropriateButton(bool isHost)
    {
        GameObject readyBtn = GameObject.Find("ReadyBtn");
        GameObject startBtn = GameObject.Find("StartBtn");
        if(isHost)
        {
            readyBtn.SetActive(false);
            startBtn.SetActive(true);
        }
        else
        {
            readyBtn.SetActive(true);
            startBtn.SetActive(false);
        }
    }

    public void Draw(RoomContext rContext, DrawType drawType)
    {
        int maxCount = rContext.GetMaxUserCount()/2;
        int redTeamIdx = rContext.GetRedTeamUserCount();
        int blueTeamIdx = rContext.GetBlueTeamUserCount();
        switch(drawType)
        {
            case DrawType.REDONLY :
                DrawRedTeam(rContext);
                break;
            case DrawType.BLUEONLY:
                DrawBlueTeam(rContext);
                break;
            case DrawType.BOTH:
                DrawRedTeam(rContext);
                DrawBlueTeam(rContext);
                break;
        }
    }

    public void AddMessageToChatWindow(string msg)
    {
        chatContents.text += msg + "\n";
    }

    private void OnEnable()
    {
        eachUserPrefab = (GameObject)Resources.Load("Prefabs/EachUser");
        redList = GameObject.Find("RedTeamList");
        blueList = GameObject.Find("BlueTeamList");
        chatContents = GameObject.Find("ChatMessage").GetComponent<Text>();
        ChangeGridCellSize();
    }

    private void CreateUserPrefabPool(int size)
    {   
        eachUserPrefabPool = new GameObject[size];
        usernameTextArray = new Text[size];

        for (int i = 0; i < size; i++)
        {
            eachUserPrefabPool[i] = Instantiate(eachUserPrefab);
            usernameTextArray[i] = eachUserPrefabPool[i].transform.Find("UserName").GetComponent<Text>();
        }
    }

    private void DrawRedTeam(RoomContext rContext)
    {
        int i = 0;
        int redTeamIdx = rContext.GetRedTeamUserCount();
        int maxCount = rContext.GetMaxUserCount() / 2;

        for (; i < redTeamIdx; i++)
            usernameTextArray[i].text = rContext.GetRedTeamClient(i).Name;
        for (; i < maxCount; i++)
            usernameTextArray[i].text = "";
    }

    private void DrawBlueTeam(RoomContext rContext)
    {
        int maxCount = rContext.GetMaxUserCount() / 2;
        int i = maxCount;
        int blueTeamIdx = maxCount + rContext.GetBlueTeamUserCount();

        for (; i < blueTeamIdx; i++)
            usernameTextArray[i].text = rContext.GetBlueTeamClient(i % maxCount).Name;

        maxCount += maxCount;
        for (; i < maxCount; i++)
            usernameTextArray[i].text = "";
    }

    private void AssignPrefabsToEachList(int size)
    {
        int i = 0;
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
}
