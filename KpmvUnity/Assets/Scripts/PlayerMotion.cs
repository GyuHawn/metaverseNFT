using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    public bool isGrounded = true;

    public Vector3 zeroVec;

    public GameObject terrain; // Generator;

    private Rigidbody rigid;
    private Animator anim;

    public static List<MainClient.PlayerObj> pdbList;

    public MainClient mClient;

    public Winner win;

    public bool save = false;

    private KeyCode currentKeycode;

    private bool GetKey(KeyCode code)
    {
        if (LcIPT.Instance.isOnline())
        {
            return currentKeycode == code;
        }
        else
        {
            return Input.GetKey(code);
        }
    }

    public void SetKey(KeyCode code) { currentKeycode = code; }
    private void Start()
    {
        zeroVec = new Vector3(0, 0, 1);
    }

    private void Awake()
    {
        mClient = GameObject.FindObjectOfType<MainClient>();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        terrain = GameObject.Find("Terrain");

    }

    public void Update()
    {
        if (!LcIPT.Instance.isOnline())
        {
            ThisUpdate();
        }
    }


    public void ThisUpdate()
    {
        Move();
        Turn();
    }

    private void Move()
    {


        if (GetKey(KeyCode.W) || GetKey(KeyCode.S) || GetKey(KeyCode.A) || GetKey(KeyCode.D))
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }
    }

    private void Turn()
    {
        Vector3 movement = Vector3.zero;

        if (GetKey(KeyCode.A))
        {
            movement.x = -1;
        }
        else if (GetKey(KeyCode.D))
        {
            movement.x = 1;
        }

        if (GetKey(KeyCode.W))
        {
            movement.z = 1;
        }
        else if (GetKey(KeyCode.S))
        {
            movement.z = -1;
        }

        if (movement != Vector3.zero)
        {
            zeroVec = movement.normalized;
        }

        transform.LookAt(transform.position + zeroVec);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Quiz") || collision.gameObject.CompareTag("Die")) //바닥 태그 추가 (태그 추가시 || 사용)
        {
            isGrounded = true;
        }
    }
    void OnCollisionStay(Collision collision)
    {
        pdbList = MainClient.pdbList;

        if (collision.gameObject.CompareTag("Die"))
        {
            transform.position = new Vector3(8, 30, -12);
        }
        if (collision.gameObject.CompareTag("Respawn"))
        {
            transform.position = new Vector3(8, 30, -12);
        }
        if (collision.gameObject.CompareTag("Win"))
        {
            if (save == false)
            {
                win.isFollowing = true;
                Save();
            }
        }
    } 

    public void Save()
    {
        MainClient.currentUser.posiSend(mClient.mCt, true);
        save = true;
    }
}
