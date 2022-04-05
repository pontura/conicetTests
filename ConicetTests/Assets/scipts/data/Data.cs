using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class Data : MonoBehaviour
{
    public string url = "http://pontura.com/conicet/";
    const string PREFAB_PATH = "Data";
    static Data mInstance = null;
    public bool DEBUG;
   
    int dataLoaded;
    public bool allLoaded;

    public static Data Instance
    {
        get
        {
            if (mInstance == null)
            {
                print("ADS");
                mInstance = FindObjectOfType<Data>();

                if (mInstance == null)
                {
                    GameObject go = Instantiate(Resources.Load<GameObject>(PREFAB_PATH)) as GameObject;
                    mInstance = go.GetComponent<Data>();
                    go.transform.localPosition = new Vector3(0, 0, 0);
                }
            }
            return mInstance;
        }
    }
    string newScene;
    public void LoadScene(string aLevelName)
    {
        this.newScene = aLevelName;
        SceneManager.LoadScene(newScene);
    }
    void Awake()
    {

        if (!mInstance)
            mInstance = this;

        else
        {
            Destroy(this.gameObject);
            return;
        }

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        DontDestroyOnLoad(this);
     
    }
}
