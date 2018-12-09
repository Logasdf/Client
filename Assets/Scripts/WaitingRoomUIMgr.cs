using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomUIMgr : MonoBehaviour {

    public GameObject eachUserPrefab;

    private void Start()
    {
        //test
        GameObject redList = GameObject.Find("RedTeamList");
        GameObject blueList = GameObject.Find("BlueTeamList");

        for(int i = 1; i <= 16; i++)
        {
            GameObject eachUser = Instantiate(eachUserPrefab);
            eachUser.transform.Find("Username").GetComponent<Text>().text = "testuser " + i.ToString();
            if (i % 2 != 0)
                eachUser.transform.SetParent(redList.transform, false);
            else
                eachUser.transform.SetParent(blueList.transform, false);
        }

    }
}
