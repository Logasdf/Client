using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomUIPainter : ScriptableObject {
    private GameObject eachUserPrefab;
    private GameObject redList;
    private GameObject blueList;
    private GameObject[] eachUserPrefabPool;
    private Color readyColor = Color.yellow;
    private Color notReadyColor = Color.black;

    private void OnEnable()
    {
        eachUserPrefab = (GameObject)Resources.Load("Prefabs/EachUser");
        redList = GameObject.Find("RedTeamList");
        blueList = GameObject.Find("BlueTeamList");
        CreateUserPrefabPool();
        DrawUsers();
    }

    public void ChangeReadyStateColor(int index, bool toReady)
    {
        eachUserPrefabPool[index].GetComponent<Image>().color = toReady ? readyColor : notReadyColor;
    }

    private void CreateUserPrefabPool()
    {   //오브젝트풀
        const int USERPREFAB_POOL_MAX = 16;
        eachUserPrefabPool = new GameObject[USERPREFAB_POOL_MAX];
        for (int i = 0; i < USERPREFAB_POOL_MAX; i++)
        {
            eachUserPrefabPool[i] = Instantiate(eachUserPrefab);
        }
    }

    private void DrawUsers()
    {
        //TODO : ROOMCONTEXT 속에 들어있는 정보들을 이용해서(args) 그려내기
        int redTeamMaxIdx = 8; //test
        int blueTeamMaxIdx = 8 + 8; // test

        for (int i = 0; i < redTeamMaxIdx; i++) AddUserPrefabAsChildToList(i, "user" + (i + 1), redList.transform);
        for (int i = 8; i < blueTeamMaxIdx; i++) AddUserPrefabAsChildToList(i, "user" + (i + 1), blueList.transform);
    }

    private void AddUserPrefabAsChildToList(int index, string name, Transform parent)
    {
        Text userName = eachUserPrefabPool[index].transform.Find("UserName").GetComponent<Text>();
        userName.text = name;
        eachUserPrefabPool[index].transform.SetParent(parent, false);
    }
}
