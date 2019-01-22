using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillLogMgr : MonoBehaviour {

    private static bool isChanged = false;
    private KillLogQueue list;

    private int displayCnt;
    private GameObject killLogs;
    private GameObject[] killLogPool;
    private Text[] killLog;

    [SerializeField]
    private GameObject pfKillLog;

	// Use this for initialization
	void Start () {
        list = KillLogQueue.Instance;
        displayCnt = 0;

        killLogs = GameObject.Find("KillLogs");
        ChangeGridCellSize();
        CreateKillLogPool();
	}

    private void LateUpdate()
    {
        if(isChanged)
        {
            //Debug.Log("Changed!");
            if (displayCnt >= 4)
                list.IncrementStart();
            //Debug.Log("Before Display");
            DisplayKillLogs();
            isChanged = false;
        }
    }

    public static void Notify()
    {
        //Debug.Log("Notify()!");
        isChanged = true;
    }

    public void PostKillLog(string from, string to)
    {
        list.AddLog(from, to);
    }

    private void DisplayKillLogs()
    {
        ClearKillLog();
        Tuple<string, string> log;
        int start = list.Start, end = list.End;
        Debug.Log(string.Format("Display from {0} to {1}", start, end));
        for (int i = start; i < end; ++i)
        {
            log = list.GetLog(i);
            string logMessage = string.Format("{0}->{1}", log.Item1, log.Item2);
            Debug.Log(logMessage);
            killLog[displayCnt].text = logMessage;
            if (!list.IsCalled(i))
            {
                list.Called(i);
                StartCoroutine(DeleteLog(2, i));
            }
        }
    }

    IEnumerator DeleteLog(float time, int index)
    {
        yield return new WaitForSeconds(time);
        if(list.Start <= index)
        {
            displayCnt--;
            list.IncrementStart();
        }
    }

    private void ClearKillLog()
    {
        for(int i = 0; i < 4; ++i)
        {
            killLog[i].text = "";
        }
        displayCnt = 0;
    }

    private void CreateKillLogPool(int size = 4)
    {
        killLogPool = new GameObject[size];
        killLog = new Text[size];
        for(int i = 0; i < size; ++i)
        {
            killLogPool[i] = Instantiate(pfKillLog);
            killLogPool[i].transform.SetParent(killLogs.transform, false);
            killLog[i] = killLogPool[i].GetComponent<Text>();
        }
    }

    private void ChangeGridCellSize()
    {
        Vector2 panelVector = killLogs.GetComponent<RectTransform>().sizeDelta;
        Vector2 newCellVector = new Vector2(panelVector.x, panelVector.y / 4);
        killLogs.GetComponent<GridLayoutGroup>().cellSize = newCellVector;
    }
}
