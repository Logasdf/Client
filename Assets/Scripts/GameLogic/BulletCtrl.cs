using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour {

    private string shooter;
    public string Shooter
    {
        get { return shooter; }
        set { shooter = value; }
    }

    public float damage = 10.0f;
    public float bulletSpeed = 50.0f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * bulletSpeed);
    }
}
