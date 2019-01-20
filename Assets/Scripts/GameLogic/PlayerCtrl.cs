using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.GameLogic.Context;

public class PlayerCtrl : MonoBehaviour {

    private PlayerContext myContext;
    private GameManager gm;
    private PacketManager pm;

    // For Movement
    private Rigidbody rb;

    private float hori = 0f;
    private float verti = 0f;
    private float rot = 0f;

    public float moveSpeed = 10.0f;
    public float rotSpeed = 120.0f;

    // For Fire
    public GameObject bullet;
    public Transform firePos;

    public float clickInterval = 0.1f;
    private float accTimeForFire = 0f;

    // For Broadcast
    //private float accTimeForSend = 0f;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        gm = GameManager.instance;
        if (gm != null)
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
    }

    private void Update()
    {
        accTimeForFire += Time.deltaTime;
        if(accTimeForFire >= clickInterval && Input.GetMouseButton(0))
        {
            accTimeForFire = 0f;
            FireBullet();
        }

        //Debug.Log(string.Format("Frame #{0} Broadcast!", Time.frameCount));
        //BroadcastMyState();
    }

    private void FireBullet()
    {
        //myContext.SetFireFlag(true);
        GameObject _bullet = Instantiate(bullet, firePos.position, firePos.rotation);
        Destroy(_bullet, 5f);
    }

    private void BroadcastMyState()
    {
        myContext.CopyToTransformProto();
        gm.PacketManager.PackMessage(protoObj: myContext.WorldState);

        ResetState();
    }

    private void ResetState()
    {
        myContext.SetFireFlag(false);
    }

}
