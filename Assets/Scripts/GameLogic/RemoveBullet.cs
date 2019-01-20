using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "BULLET")
        {
            if (gameObject.tag != "ENEMY" && gameObject.tag != "MYTEAM")
            {
                Destroy(collision.collider.gameObject);
            }
            else if(gameObject.tag == "MYTEAM")
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                Destroy(collision.collider.gameObject);
            }
            else if(gameObject.tag == "ENEMY")
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                Destroy(collision.collider.gameObject);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "BULLET")
        {
            if (gameObject.tag == "MYTEAM")
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            else if (gameObject.tag == "ENEMY")
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
}
