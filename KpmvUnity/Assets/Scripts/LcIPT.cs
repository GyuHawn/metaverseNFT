using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LcIPT : MonoBehaviour
{
    public static LcIPT Instance;
    private bool mbOnline = false;
    public float mJumpHeight = 2f;

    private MultiClient mCf;
    public GameObject splayer;
    public GameObject[] playerPF;
    public GameObject go;
    public GameObject Camera;
    public GameObject inputField;
    public TMP_FontAsset m_Font;


    public const int maxP = 3;

    Vector3[] positions = {
        new Vector3(2, 40, 15), new Vector3(-2, 40, 15), new Vector3(2, 40, 15), new Vector3(-2, 40, 13)
    };
    //  Color[] colors = { Color.blue, Color.red, C };
    Vector3[] positions2 =
    {
        new Vector3(-21, -1, 6), new Vector3(-27, -1, 6), new Vector3(-14, -1, 6), new Vector3(-31, -1, 6)
    };

    Vector3[] positions3 =
    {
        new Vector3(-21, -1, 6), new Vector3(-27, -1, 6), new Vector3(-14, -1, 6), new Vector3(-31, -1, 6)
    };

    public GameObject[] mPlayers;

    private int pIndex = -1;

    public bool isOnline() { return mbOnline; }
    public void SetIndex(int _index)
    {

        pIndex = _index;
        if (pIndex >= 0)
        {
            InstantiatePlayer(pIndex, MainClient.currentUser==null?"":MainClient.currentUser.mColor);
            go = mPlayers[pIndex];
            Camera.GetComponent<camera>().SetTarget(go);

            GameObject t = new GameObject("myname");
            t.transform.parent = go.transform;
            t.transform.localPosition = new Vector3(0f, 8f, 0f);

            var t1 = t.AddComponent<TextMeshPro>();
            t1.GetComponent<TMP_Text>().font = m_Font;
            if (MainClient.currentUser != null) 
            { 
                t1.text = MainClient.currentUser.mUserName;
                t1.GetComponent<TMP_Text>().color = Color.black;
            }
            t1.alignment = TextAlignmentOptions.Center;
            t1.fontSize = 5;

        }
        else
        {
            Debug.Log("인원수 초과2"); mCf.mCt.disconnect();
        }

    }

    public int GetIndex() { return pIndex; }
    public MultiClient GetMultiClient() { return mCf; }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        mCf = GetComponent<MultiClient>();
        mPlayers = new GameObject[maxP];

        go = splayer;

        GameObject t = new GameObject("myname");
        t.transform.parent = go.transform;
        t.transform.localPosition = new Vector3(0f, 8f, 0f);
        
        var t1 = t.AddComponent<TextMeshPro>();
        t1.GetComponent<TMP_Text>().font = m_Font;
        if (MainClient.currentUser!=null) {
            t1.text = MainClient.currentUser.mUserName;
            t1.GetComponent<TMP_Text>().color = Color.black;
        } 
        t1.alignment = TextAlignmentOptions.Center;
        t1.fontSize = 5;

        go.SetActive(true);
        Camera.GetComponent<camera>().SetTarget(go);
    }

    public void Connect()
    {
        if (!mCf.mCt.isConnected())
        {
            for (int i = 0; i < maxP; i++)
            {
                if (mPlayers[i])
                {
                    Destroy(mPlayers[i]);
                    Debug.Log("Destroy pidx: " + i);
                }
            }
            if (mCf.mCt.connect(MainClient.serverAddress, 7771))
            {
                go = null;
                splayer.SetActive(false);
                mbOnline = true;
                Debug.Log("MultiClient Start 1111");
                GameObject.Find("Text1").GetComponent<TextMeshProUGUI>().SetText(string.Join("\n", MultiClient.mLines));
            }
        }
    }

    public void DisConnect()
    {

        for (int i = 0; i < maxP; i++)
        {
            if (mPlayers[i])
            {
                Destroy(mPlayers[i]);
                Debug.Log("Destroy pidx: " + i);
            }
        }
        pIndex = -1;
        mCf.mCt.disconnect();
        mbOnline = false;
        go = splayer;
        go.SetActive(true);
        Camera.GetComponent<camera>().SetTarget(go);
        GameObject.Find("Text1").GetComponent<TextMeshProUGUI>().SetText(string.Join("\n", MainClient.mLines));
    }

    public void InstantiatePlayer(int i, string color)
    {
        Scene scene = SceneManager.GetActiveScene();
        GameObject myPF;
        int ipf = -1;

        switch (color)
        {
            case "blue":  ipf = 0;    break;
            case "white": ipf = 1;    break;
            case "red":  ipf = 2;    break;
        }
        if (ipf < 0) { return; }
        myPF = playerPF[ipf];
        GameObject player = new GameObject();
        if(scene.name == "Quiz1")
        {
            player = Instantiate(myPF, positions[i], Quaternion.identity);
        }else if(scene.name == "Quiz2")
        {
            player = Instantiate(myPF, positions2[i], Quaternion.identity);
        }else if(scene.name == "Quiz3")
        {
            player = Instantiate(myPF, positions3[i], Quaternion.identity);
        }
            mPlayers[i] = player;

    }
    public void moveSend(JcCtUnity1.JcCtUnity1 ct, GameObject obj, int code, float plusx, float plusy, float plusz)
    {
        using (JcCtUnity1.PkWriter1Nm pkw = new JcCtUnity1.PkWriter1Nm(3))
        {
            pkw.wInt32s(LcIPT.Instance.pIndex);
            pkw.wInt32s(code);
            pkw.wReal32(obj.transform.position.x + plusx);
            pkw.wReal32(obj.transform.position.y + plusy);
            pkw.wReal32(obj.transform.position.z + plusz);
            ct.send(pkw);
        }
    }

    public void currentSend(JcCtUnity1.JcCtUnity1 ct, int code = 0)
    {
        using (JcCtUnity1.PkWriter1Nm pkw = new JcCtUnity1.PkWriter1Nm(3))
        {
            pkw.wInt32s(LcIPT.Instance.pIndex);
            pkw.wInt32s(code);
            pkw.wReal32(go.transform.position.x);
            pkw.wReal32(go.transform.position.y);
            pkw.wReal32(go.transform.position.z);
            if (code ==0)
            {
                pkw.wStr1(MainClient.currentUser == null ? "" : MainClient.currentUser.mUserName);
                pkw.wStr1(MainClient.currentUser == null ? "" : MainClient.currentUser.mColor);
            }
            ct.send(pkw);
        }
    }

    public void jumpSend(JcCtUnity1.JcCtUnity1 ct, float force)
    {
        using (JcCtUnity1.PkWriter1Nm pkw = new JcCtUnity1.PkWriter1Nm(31))
        {           
            pkw.wInt32s(LcIPT.Instance.pIndex);
            pkw.wReal32(force);
            ct.send(pkw);
        }
    }

    void Update()
    {
        Move();
    }


    void Move()
    {
        if (inputField.GetComponent<UnityEngine.UI.InputField>().isFocused) { return; }
        float spd = 10.0f * Time.deltaTime;

        if (go)
        {

            if (Input.GetKey(KeyCode.W))
            {
                if (mbOnline)
                {
                    moveSend(mCf.mCt, go, (int)KeyCode.W, 0, 0, +spd);
                }
                else
                {
                    go.GetComponent<PlayerMotion>().zeroVec = new Vector3(0, 0, +spd);
                    go.transform.position += new Vector3(0, 0, +spd);
                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (mbOnline)
                {
                    moveSend(mCf.mCt, go, (int)KeyCode.S, 0, 0, -spd);
                }
                else
                {
                    go.GetComponent<PlayerMotion>().zeroVec = new Vector3(0, 0, -spd);
                    go.transform.position += new Vector3(0, 0, -spd);
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                if (mbOnline)
                {
                    moveSend(mCf.mCt, go, (int)KeyCode.A, -spd, 0, 0);
                }
                else
                {
                    go.GetComponent<PlayerMotion>().zeroVec = new Vector3(-spd, 0, 0);
                    go.transform.position += new Vector3(-spd, 0, 0);
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (mbOnline)
                {
                    moveSend(mCf.mCt, go, (int)KeyCode.D, +spd, 0, 0);
                }
                else
                {
                    go.GetComponent<PlayerMotion>().zeroVec = new Vector3(+spd, 0, 0);
                    go.transform.position += new Vector3(+spd, 0, 0);
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (go.GetComponent<PlayerMotion>().isGrounded)
                {
                    if (mbOnline)
                    {
                        float jumpForce = Mathf.Sqrt(mJumpHeight * -2 * Physics.gravity.y);
                        jumpSend(mCf.mCt,jumpForce);
                       // moveSend(mCf.mCt, go, (int)KeyCode.Space, 0, mJumpHeight, 0);
                    }
                    else
                    {
                        Rigidbody rb = go.GetComponent<Rigidbody>();
                        rb.AddForce(Vector3.up * Mathf.Sqrt(mJumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
                    }
                    go.GetComponent<PlayerMotion>().isGrounded = false;
                }
            }
        }
    }
}