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
    private GameObject readyBtn;
    private GameObject startBtn;
    private GameObject[] eachUserPrefabPool;

    private Text chatContents;
    private Text[] usernameTextArray;

    private readonly Color readyColor = Color.yellow;
    private readonly Color notReadyColor = Color.black;


    public void Init(int size, bool isHost)
    {
        readyBtn = GameObject.Find(ElementStrings.READY_BTN);
        startBtn = GameObject.Find(ElementStrings.START_BTN);
 
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
    {   //텍스트랑 색상그리는거를 나눠야 더 좋을 것 같은데.....
        lock(Locks.lockForRoomContext)
        {
            switch (drawType)
            {
                case DrawType.REDONLY:
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
    }

    public void AddMessageToChatWindow(string msg)
    {
        chatContents.text += msg + "\n";
    }

    private void OnEnable()
    {
        eachUserPrefab = (GameObject)Resources.Load(PathStrings.EACHUSER);
        redList = GameObject.Find(ElementStrings.REDTEAMLIST);
        blueList = GameObject.Find(ElementStrings.BLUETEAMLIST);
        chatContents = GameObject.Find(ElementStrings.CHAT_INPUTFIELD).GetComponent<Text>();
        ChangeGridCellSize();
    }

    private void CreateUserPrefabPool(int size)
    {   
        eachUserPrefabPool = new GameObject[size];
        usernameTextArray = new Text[size];

        for (int i = 0; i < size; i++)
        {
            eachUserPrefabPool[i] = Instantiate(eachUserPrefab);
            usernameTextArray[i] = eachUserPrefabPool[i].transform.Find(ElementStrings.USERNAME_PANEL).GetComponent<Text>();
        }
    }

    private void DrawRedTeam(RoomContext rContext)
    {
        int i = 0;
        int redTeamIdx = rContext.GetRedTeamUserCount();
        int maxCount = rContext.GetMaxUserCount() / 2;

        for (; i < redTeamIdx; i++)
        {
            usernameTextArray[i].text = rContext.GetRedTeamClient(i).Name;
            ChangeReadyStateColor(i, rContext.GetRedTeamClient(i).Ready);
        }
        for (; i < maxCount; i++)
        {
            usernameTextArray[i].text = "";
            ChangeReadyStateColor(i, false);
        }
    }

    private void DrawBlueTeam(RoomContext rContext)
    {
        int maxCount = rContext.GetMaxUserCount() / 2;
        int i = maxCount;
        int blueTeamIdx = maxCount + rContext.GetBlueTeamUserCount();

        for (; i < blueTeamIdx; i++)
        {
            usernameTextArray[i].text = rContext.GetBlueTeamClient(i % maxCount).Name;
            ChangeReadyStateColor(i, rContext.GetBlueTeamClient(i % maxCount).Ready);
        }
        maxCount += maxCount;
        for (; i < maxCount; i++)
        {
            usernameTextArray[i].text = "";
            ChangeReadyStateColor(i, false);
        }
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
