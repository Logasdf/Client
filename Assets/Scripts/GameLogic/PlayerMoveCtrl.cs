using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCtrl : MonoBehaviour {

    private float hori = 0f;
    private float verti = 0f;
    private float rot = 0f;

    public float moveSpeed = 10.0f;
    public float rotSpeed = 80.0f;

    private Transform tr;

	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        hori = Input.GetAxis("Horizontal");
        verti = Input.GetAxis("Vertical");
        rot = Input.GetAxis("Mouse X");

        Vector3 moveDir = (Vector3.forward * verti) + (Vector3.right * hori);

        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);
        //Debug.Log(rot);
        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * rot);
	}
}
