using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class Main : MonoBehaviour
{
    public GameObject splayer;
    public GameObject[] mPlayerPF;
    public GameObject[] mPlayer;
    public GameObject go;
    public TMP_FontAsset m_Font;
    public GameObject Camera;
    public GameObject inputField;
    void Awake()
    {
        LcIPT.GetThis().go = splayer;
        LcIPT.GetThis().splayer = splayer;
        go = LcIPT.GetThis().splayer;
        LcIPT.GetThis().playerPF = mPlayerPF;
        LcIPT.GetThis().mPlayers = mPlayer;
        LcIPT.GetThis().m_Font = m_Font;
        LcIPT.GetThis().Camera = Camera;
        LcIPT.GetThis().inputField = inputField;
    }

    void Start()
    {
       LcIPT.GetThis().Start();
    }

    void Update()
    {
        LcIPT.GetThis().Update();
    }

    public void Connect()
    {
        LcIPT.GetThis().Connect();
    }

    public void DisConnect()
    {
        LcIPT.GetThis().DisConnect();
    }

    public void NextScenes()
    {
    Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Quiz1")
        {
            SceneManager.LoadScene("Scenes/Quiz2");
        }
        else if (scene.name == "Quiz2")
        {
            SceneManager.LoadScene("Scenes/Quiz3");
        }
        else if (scene.name == "Quiz3")
        {
            SceneManager.LoadScene("Scenes/Quiz1");
        }
    }
}
