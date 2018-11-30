using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour {

    public GameObject roomItem;
    public GameObject scrollContent;

	// Use this for initialization
	void Start () {
	    
        for(int i = 0; i<10; i++)
        {
            GameObject room = (GameObject)Instantiate(roomItem);
            room.transform.SetParent(scrollContent.transform, false);
        }
        
	}
	
}
