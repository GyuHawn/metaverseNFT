using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public GameObject mWin;

    public bool save = false;

    public float collisonTime = 2f;
    private bool canCollide = true;
    
    private KeyCode currentKeycode;

    float time;

    private bool GetKey(KeyCode code)
    {
        if (LcIPT.GetThis().isOnline())
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
        time = Time.time;
        mClient = LcIPT.GetThis().mMc;
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        terrain = GameObject.Find("Terrain");
        mWin = GameObject.Find("Trophy");
        win = GameObject.FindObjectOfType<Winner>();

    }

    public void Update()
    {
        if (LcIPT.GetThis().inputField)
        {
            if (LcIPT.GetThis().inputField.GetComponent<UnityEngine.UI.InputField>().isFocused) { return; }
        }
        if (!LcIPT.GetThis().isOnline())
        {
            ThisUpdate();
        } else
        {
            if (! (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && currentKeycode != (KeyCode)1 )
            {
                LcIPT.GetThis().currentSend(LcIPT.GetThis().GetMultiClient().mCt,1);
            }
 
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
            LcIPT.GetThis().go.GetComponent<Rigidbody>().velocity += new Vector3(0, -0.02f, 0);
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

        if (transform.Find("myname"))
        {
            transform.Find("myname").rotation = Quaternion.Euler(0, 0, 0);
        }

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
            if (!LcIPT.GetThis().isOnline())
            {
                transform.position = new Vector3(8, 30, -12);
            } else
            {
                LcIPT.GetThis().moveSend(LcIPT.GetThis().GetMultiClient().mCt, gameObject, 1, 8 - transform.position.x, 30 - transform.position.y, -12 - transform.position.z);
            }
        }
        if (collision.gameObject.CompareTag("Respawn"))
        {
            if (!LcIPT.GetThis().isOnline())
            {
                transform.position = new Vector3(8, 30, -12);
            }
            else
            {
                LcIPT.GetThis().moveSend(LcIPT.GetThis().GetMultiClient().mCt, gameObject, 1, 8 - transform.position.x, 30 - transform.position.y, -12 - transform.position.z);
            }
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
        //4������ ����, it
        if (canCollide)
        {
            if (collision.gameObject.CompareTag("Red"))
            {
                transform.gameObject.tag = "Red";
                transform.Find("myname").GetComponentInChildren<TMP_Text>().color = Color.red;
                //pName.playerNameText.color = Color.red;
            }
            else if (collision.gameObject.CompareTag("Blue"))
            {
                transform.gameObject.tag = "Blue";
                transform.Find("myname").GetComponentInChildren<TMP_Text>().color = Color.blue;
                //pName.playerNameText.color = Color.blue;
            }
            else if (collision.gameObject.CompareTag("Green"))
            {
                transform.gameObject.tag = "Green";
                transform.Find("myname").GetComponentInChildren<TMP_Text>().color = Color.green;
                //pName.playerNameText.color = Color.green;
            }
            else if (collision.gameObject.CompareTag("Yellow"))
            {
                transform.gameObject.tag = "Yellow";
                transform.Find("myname").GetComponentInChildren<TMP_Text>().color = Color.yellow;
                //pName.playerNameText.color = Color.yellow;
            }
            if (collision.gameObject.CompareTag("Fail"))
            {
                transform.gameObject.tag = "Fail";
                transform.Find("myname").GetComponentInChildren<TMP_Text>().color = Color.black;
                // pName.playerNameText.color = Color.black;
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
