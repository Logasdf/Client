using Assets.Scripts.GameLogic.Context;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCtrl : MonoBehaviour {

    private float hori = 0f;
    private float verti = 0f;
    private float rot = 0f;

    public float moveSpeed = 10.0f;
    public float rotSpeed = 120.0f;

    private Transform tr;
    private Rigidbody rb;
    private PlayerContext myContext;
    private GameManager gm;
    private PacketManager pm;

    private int frameCount = 0;
    private int sendFrequency = 1;

    private float accumSendTime = 0f;
    //private float sendFrequency = 30.0f;

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
        hori = Input.GetAxis("Horizontal");
        verti = Input.GetAxis("Vertical");
        rot = Input.GetAxis("Mouse X");

        Vector3 moveDir = (Vector3.forward * verti) + (Vector3.right * hori);
        moveDir = transform.TransformDirection(moveDir.normalized * moveSpeed * Time.deltaTime);
        Vector3 newPosition = rb.position + moveDir;
        Vector3 newRotation = rb.rotation.eulerAngles + (Vector3.up * rotSpeed * Time.deltaTime * rot);

        rb.MovePosition(newPosition);
        rb.MoveRotation(Quaternion.Euler(newRotation));

        Debug.Log(string.Format("Frame #{0} Broadcast", Time.frameCount));
        BroadcastMyTransform();

        //if ((Time.frameCount - frameCount) >= sendFrequency)
        //{
        //    Debug.Log(string.Format("{0} / {1}", frameCount, Time.frameCount));
        //    frameCount = Time.frameCount;
        //    BroadcastMyTransform();
        //}

        //accumSendTime += Time.deltaTime;
        //if (accumSendTime >= 0.1f)
        //{
        //    accumSendTime = 0f;
        //    Debug.Log(string.Format("{0} / {1} Send Position!", accumSendTime, Time.deltaTime));
        //    BroadcastMyTransform();
        //}
    }

    // Update is called once per frame
    void Update ()
    {

    }

    private void BroadcastMyTransform()
    {
        myContext.State.AnimState = (int)PlayerContext.Animation.MOVE;
        myContext.CopyToTransformProto();
        gm.PacketManager.PackMessage(protoObj: myContext.State);
    }
}
