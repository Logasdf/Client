using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour {

    public float damage = 10.0f;
    public float bulletSpeed = 5000.0f;

    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
    }
}
