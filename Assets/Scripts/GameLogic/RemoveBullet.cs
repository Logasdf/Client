using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.GameLogic.Context;

public class RemoveBullet : MonoBehaviour {

    private PlayerContext myContext;

    private void Start()
    {
        myContext = GameManager.instance.MyContext;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "BULLET")
        {
            if (gameObject.tag != "ENEMY" && gameObject.tag != "MYTEAM")
            {
                Destroy(collision.gameObject);
            }
            else if(gameObject.tag == "MYTEAM")
            {
                if(gameObject.name == myContext.Client.Name)
                {
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    //내가 맞았을 경우, Hit했다는 정보를 서버에 전송해야함.
                    string from = collision.gameObject.GetComponent<BulletCtrl>().Shooter;
                    int damage = (int)collision.gameObject.GetComponent<BulletCtrl>().damage;
                    Debug.Log(string.Format("Be Shot by {0}, {1}", from, damage));
                    gameObject.GetComponent<PlayerCtrl>().HandleHitEvent(from, damage);
                }
                else
                {
                    Destroy(collision.gameObject);
                }
            }
            else if(gameObject.tag == "ENEMY")
            {
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "BULLET")
        {
            if (gameObject.tag == "MYTEAM" && gameObject.name == myContext.Client.Name)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                Destroy(collision.gameObject);
            }
        }
    }
}
