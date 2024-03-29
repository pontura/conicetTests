using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class Data : MonoBehaviour
{
    public states state;
    public enum states
    {
        online,
        offline
    }

    const string PREFAB_PATH = "Data";
    static Data mInstance = null;
    public int sampleRate = 44100;

    int dataLoaded;
    public bool allLoaded;
    public DatabaseContent databaseContent;
    public UploadWav uploadWav;

    public int tabletID;
    public int userAutoIncrementID;

    public static Data Instance
    {
        get
        {
            if (mInstance == null)
            {
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
        databaseContent = GetComponent<DatabaseContent>();
    }
    private void Start()
    {
        string[] arr = Microphone.devices;
        StartCoroutine(databaseContent.Load(OnLoaded, state == states.online));
        tabletID = PlayerPrefs.GetInt("tabletID", 0);
        userAutoIncrementID = PlayerPrefs.GetInt("userAutoIncrementID", 0);
        sampleRate = PlayerPrefs.GetInt("sampleRate", 44100);
        uploadWav = GetComponent<UploadWav>();
    }
    void OnLoaded()
    {
        print("done");
    }
    public string GetFileName(bool next = false)
    {

        if (next)
            return tabletID.ToString("00") + "-" + (userAutoIncrementID + 1).ToString("0000");
        else
            return tabletID.ToString("00") + "-" + (userAutoIncrementID).ToString("0000");
    }
    public TestData testData;

    public void SetNewUser()
    {
        testData = new TestData();
        userAutoIncrementID++;
        PlayerPrefs.SetInt("userAutoIncrementID", userAutoIncrementID);
    }
    public void SetSampleRate(int newSampleRate)
    {
        PlayerPrefs.SetInt("sampleRate", newSampleRate);
        sampleRate = newSampleRate;
    }
}
