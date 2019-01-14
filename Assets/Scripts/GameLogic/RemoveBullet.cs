using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "BULLET")
        {
            if (gameObject.tag == "ENEMY")
            {
                Debug.Log("It's an enemy");
                //GameManager.instance.SendBeShotEvent(gameObject.name, collision.gameObject.name);
            }
            else if(gameObject.tag == "MYTEAM")
            {
                Debug.Log("It's my team");
            }
            Destroy(collision.gameObject);
        }
    }
}
