using Assets.Scripts.GameLogic.Context;
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
    private Rigidbody rb;
    private PlayerContext myContext;
    private GameManager gm;
    private PacketManager pm;

    // Use this for initialization
    void Start () {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        gm = GameManager.instance;
        if(gm != null)
            myContext = gm.MyContext;
    }

    private void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        hori = Input.GetAxis("Horizontal");
        verti = Input.GetAxis("Vertical");
        rot = Input.GetAxis("Mouse X");

        Vector3 moveDir = (Vector3.forward * verti) + (Vector3.right * hori);
        moveDir = transform.TransformDirection(moveDir.normalized * moveSpeed * Time.deltaTime);
        Vector3 newPosition = rb.position + moveDir;
        Vector3 newRotation = rb.rotation.eulerAngles + (Vector3.up * rotSpeed * Time.deltaTime * rot);

        rb.MovePosition(newPosition);
        rb.MoveRotation(Quaternion.Euler(newRotation));

        BroadcastMyTransform();
    }

    //float accum = 0f;
    //float interval = 0.5f;
    private void LateUpdate()
    {
        //BroadcastMyTransform();
        //accum += Time.deltaTime;
        //if(accum >= interval)
        //{
        //    accum = 0f;
        //    BroadcastMyTransform();
        //}
    }

    private void BroadcastMyTransform()
    {
        myContext.State.AnimState = (int)PlayerContext.Animation.MOVE;
        myContext.CopyToTransFormProto();
        gm.PacketManager.PackMessage(protoObj: myContext.State);
    }
}
