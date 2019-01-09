using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour {

    public GameObject bullet; // bullet prefab
    public Transform firePos; // the position that a bullet is fired

    public float clickInterval = 0.1f;
    private float accumTime;

	// Use this for initialization
	void Start () {
        accumTime = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        accumTime += Time.deltaTime;
        if (accumTime >= clickInterval && Input.GetMouseButton(0))
        {
            accumTime = 0f;
            FireBullet();
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    FireBullet();
        //}
    }

    void FireBullet()
    {
        Instantiate(bullet, firePos.position, firePos.rotation);
    }
}
