using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour {

    public void CreateBtnClicked()
    {
        Debug.Log("Create Button Clicked");
    }

    public void ExitBtnClicked()
    {
        Debug.Log("Exit Button clicked");
        Application.Quit();
    }
  
}
