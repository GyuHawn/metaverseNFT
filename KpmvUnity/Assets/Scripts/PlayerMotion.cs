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
    public PlayerName pName;

    public Winner win;

    public bool save = false;

    public float collisonTime = 2f;
    private bool canCollide = true;
    
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
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Quiz") || collision.gameObject.CompareTag("Die")) //�ٴ� �±� �߰� (�±� �߰��� || ���)
        {
            isGrounded = true;
        }
    }
    void OnCollisionStay(Collision collision)
    {
        pdbList = MainClient.pdbList;
        //ox����
        if (collision.gameObject.CompareTag("Die"))
        {
            transform.position = new Vector3(8, 30, -12);
        }
        if (collision.gameObject.CompareTag("Respawn"))
        {
            transform.position = new Vector3(8, 30, -12);
        }
        //����
        if (collision.gameObject.CompareTag("Win"))
        {
            if (save == false)
            {
                win.isFollowing = true;
                Save();
            }
        }
        //4������ ����
        if (canCollide)
        {
            if (collision.gameObject.CompareTag("Red"))
            {
                transform.gameObject.tag = "Red";
                pName.playerNameText.color = Color.red;
            }
            else if (collision.gameObject.CompareTag("Blue"))
            {
                transform.gameObject.tag = "Blue";
                pName.playerNameText.color = Color.blue;
            }
            else if (collision.gameObject.CompareTag("Green"))
            {
                transform.gameObject.tag = "Green";
                pName.playerNameText.color = Color.green;
            }
            else if (collision.gameObject.CompareTag("Yellow"))
            {
                transform.gameObject.tag = "Yellow";
                pName.playerNameText.color = Color.yellow;
            }
            if (collision.gameObject.CompareTag("Fail"))
            {
                transform.gameObject.tag = "Fail";
                pName.playerNameText.color = Color.black;
            }
            canCollide = false;
            StartCoroutine(ChangeCollide());
        }
    } 

    private IEnumerator ChangeCollide()
    {
        yield return new WaitForSeconds(collisonTime);
        canCollide = true;
    }

    public void Save()
    {
        MainClient.currentUser.posiSend(mClient.mCt, true);
        save = true;
    }

    public void ClearPlayer()
    {
        transform.gameObject.tag = "Player";
        pName.playerNameText.color = Color.black;
    }
}
