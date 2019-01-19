using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Connection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectionUIMgr : MonoBehaviour {

    public GameObject ip;
    public GameObject port;
    public GameObject connect;
    public GameObject _default;

    private InputField ipInputField;
    private InputField portInputField;
    private Button connectBtn;
    private Button defaultBtn;

	// Use this for initialization
	void Start () {
        ipInputField = ip.transform.GetChild(1).GetComponent<InputField>();
        if (ipInputField == null) Debug.Log("ipInputField is null..");

        portInputField = port.transform.GetChild(1).GetComponent<InputField>();
        if (portInputField == null) Debug.Log("ipInputField is null..");

        connectBtn = connect.GetComponent<Button>();
        if(connectBtn == null) Debug.Log("connectBtn is null....");
        connectBtn.onClick.AddListener(SetServerInfo);

        defaultBtn = _default.GetComponent<Button>();
        if (defaultBtn == null) Debug.Log("defaultBtn is null....");
        defaultBtn.onClick.AddListener(FillDefalutInfo);
    }

    private void FillDefalutInfo()
    {
        ipInputField.text = "127.0.0.1";
        portInputField.text = "9910";
    }
    
    private void SetServerInfo()
    {
        if(ipInputField.text == null || ipInputField.text == "")
        {
            ServerInfo.IP = "127.0.0.1";
        }
        else
        {
            ServerInfo.IP = ipInputField.text;
        }
        if(portInputField.text == null || portInputField.text == "")
        {
            ServerInfo.Port = "9910";
        }
        else
        {
            ServerInfo.Port = portInputField.text;
        }
        SceneManager.LoadScene(PathStrings.SCENE_GAMELOBBY);
    }
}
