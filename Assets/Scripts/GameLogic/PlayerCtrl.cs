using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.GameLogic.Context;
using Google.Protobuf.State;

public class PlayerCtrl : MonoBehaviour {

    private PlayerContext myContext;
    private GameManager gm;
    private KillLogMgr killLogMgr;
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

    // Health UI
    Text healthText;

    // For Respawn
    public int respawnTime = 3;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody>();
        gm = GameManager.instance;
        if (gm != null)
        {
            myContext = gm.MyContext;
            killLogMgr = gm.KillLogMgr;
        }
        healthText = transform.GetChild(1).GetChild(0).GetComponent<Text>();
    }

    private void FixedUpdate()
    {
        if (myContext.WorldState.AnimState != PlayerContext.DEATH)
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
    }

    private void Update()
    {
        if(myContext.WorldState.AnimState != PlayerContext.DEATH)
        {
            accTimeForFire += Time.deltaTime;
            if (accTimeForFire >= clickInterval && Input.GetMouseButton(0))
            {
                accTimeForFire = 0f;
                FireBullet();
            }
        }
        
        BroadcastMyState();
    }

    public void HandleHitEvent(string from, int damage)
    {
        // 이미 죽어있다면 무시
        if (myContext.WorldState.AnimState == PlayerContext.DEATH)
        {
            return;
        }

        HitState hitState = new HitState
        {
            From = from,
            To = myContext.Client.Name,
            Damage = damage
        };
        myContext.WorldState.Hit = true;
        myContext.UpdateHitState(hitState);

        myContext.WorldState.Health -= damage;
        healthText.text = myContext.WorldState.Health.ToString();
        if (myContext.WorldState.Health <= 0)
        {
            Debug.Log("Death!!");
            myContext.WorldState.DeathPoint++;
            myContext.WorldState.AnimState = PlayerContext.DEATH;
            healthText.text = "Death";
            killLogMgr.PostKillLog(hitState.From, hitState.To);
            StartCoroutine(RespawnPlayer(respawnTime));
        }
    }

    IEnumerator RespawnPlayer(float time)
    {
        yield return new WaitForSeconds(time);

        Debug.Log("Respawn!!");
        myContext.ResetTransform();
        myContext.WorldState.AnimState = PlayerContext.IDLE;
        myContext.WorldState.Health = 100;
        healthText.text = myContext.WorldState.Health.ToString();
    }

    private void FireBullet()
    {
        myContext.WorldState.Fired = true;
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
        myContext.WorldState.Hit = false;
        myContext.WorldState.Fired = false;
    }

}
