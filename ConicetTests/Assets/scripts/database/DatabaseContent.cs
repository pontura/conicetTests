using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.IO;

public class DatabaseContent : MonoBehaviour
{
    [SerializeField] AudioRecorder audioRecForDEBUG;
    public bool allLoaded;
    public int version;
    [SerializeField] ConfigProfiles activeTest;

    public string url;
    public Main main;

    [Serializable]
    public class Main
    {
        public int version;
        public GameObjects[] game_objects;
        public ConfigProfiles[] config_profiles;
    }
    [Serializable]
    public class GameObjects
    {
        public string name;
        public string image;
        public string sound_sprite;
    }
    [Serializable]
    public class ConfigProfiles
    {
        public int id;
        public string name;
        public Content content;
    }
    [Serializable]
    public class Content
    {
        public string name;
        public Session session;
    }
    [Serializable]
    public class Session
    {
        public Assets assets;
        public Levels[] levels;
        public bool time_limit;
    }
    [Serializable]
    public class Assets
    {
        public string[] backgrounds;
        public string[] objects;
    }
    [Serializable]
    public class Levels
    {
        public string background;
        public string name;
        public int recording_time;
        public Stages[] stages;
    }
    [Serializable]
    public class Stages
    {
        public string asset;
    }



    public IEnumerator Load(System.Action OnLoaded, bool loadFromServer)
    {
        Events.Log("Load from " + url);
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Init(PlayerPrefs.GetString("json"), OnLoaded); // Init: data guardada local
            }
            else
            {
                string json = www.downloadHandler.text;
                PlayerPrefs.SetString("json", json); // Init: data del server
                Init(json, OnLoaded);
            }
        }
    }
    private void Init(string json, System.Action OnLoaded)
    {
        main = JsonUtility.FromJson<Main>(json);
        //if (main.version > PlayerPrefs.GetInt("version", 0))
        //{
            LoadDataContent();
        //}

        version = main.version;
        PlayerPrefs.SetInt("version", version);

        int active = PlayerPrefs.GetInt("activeText");
        if (active != 0)
            SetActive(active);

        OnLoaded();
    }
    void AllReady()
    {
        Events.Log("All assets ready");
        allLoaded = true;
    }



    public void SetActive(int id)
    {
        print("id: " + id);
        PlayerPrefs.SetInt("activeText", id);
        foreach (ConfigProfiles cf in main.config_profiles)
            if (cf.id == id)
                activeTest = cf;
    }
    public ConfigProfiles GetConfig(string name)
    {
        foreach (ConfigProfiles cf in main.config_profiles)
            if (cf.name == name)
                return cf;
        return null;
    }
    public ConfigProfiles GetActive()
    {
        return activeTest;
    }
    public GameObjects GetDataFor(string name)
    {
        foreach (GameObjects go in main.game_objects)
            if (go.name == name)
                return go;
        return null;
    }


    string fileName;
    string fullpathWav;
    string fullpathSprite;
    GameObjects go;
    int dataContentID = 0;

    void LoadDataContent()
    {
        if (dataContentID >= main.game_objects.Length)
            AllReady();
        else if (!FileExists(main.game_objects[dataContentID].name))
        {
            go = main.game_objects[dataContentID];
            fullpathSprite = go.image;
            fullpathWav = go.sound_sprite;
            fileName = go.name;
            Events.Log("Load: " + go.name + " id:" + dataContentID + " de: " + main.game_objects.Length);
            LoadWav();
            dataContentID++;
        }
        else //Loop
        {
            dataContentID++;
            Events.Log("File exists eon disk: " + dataContentID );
            LoadDataContent();
        }
    }
    bool FileExists(string fileName)
    {
        string filePath = Application.persistentDataPath + "/" + fileName + ".mp3";
        if (System.IO.File.Exists(filePath))
            return true;
        return false;
    }
    void LoadWav()
    {
        StartCoroutine(LoadWav(LoadSprite));
    }
    void LoadSprite()
    {
        StartCoroutine(LoadSprite(LoadDataContent));
    }


    IEnumerator LoadSprite(System.Action OnDone)
    {
        WWW www = new WWW(fullpathSprite);
        yield return www;
        SaveImage(www.texture);
        OnDone();
    }


    IEnumerator LoadWav(System.Action OnDone)
    {
        print("LoadFile " + fullpathWav);
        using (var www = new WWW(fullpathWav))
        {
            yield return www;

            AudioClip audioClip = www.GetAudioClip();
            yield return new WaitForEndOfFrame();
            SaveMp3Locally(audioClip);


            OnDone();
        }
    }
    public void SaveMp3Locally(AudioClip audioClip)
    {
        string fullFileName = Application.persistentDataPath + "/" + fileName + ".mp3";
        Events.Log("save: " + fullFileName);
        EncodeMP3.convert(audioClip, fullFileName, 128);
    }
    public void SaveMp3Locally(AudioClip audioClip, string fullFileName)
    {
        Events.Log("SaveMp3Locally: " + fullFileName);
        EncodeMP3.convert(audioClip, fullFileName, 64);
    }
    void SaveImage(Texture2D texture2d)
    {
        string fullFileName = Application.persistentDataPath + "/" + fileName + ".png";
        byte[] fileData = texture2d.EncodeToPNG();
        using (var fs = new FileStream(fullFileName, FileMode.Create, FileAccess.Write))
        {
            fs.Write(fileData, 0, fileData.Length);
        }
    }

    public Texture2D GetTexture(string fileName, System.Action<AudioClip> OnDone)
    {
        string fullFileName = Application.persistentDataPath + "/" + fileName + ".png";
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(fullFileName))
        {
            fileData = File.ReadAllBytes(fullFileName);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }
    public IEnumerator GetMp3(string fileName, System.Action<AudioClip> OnDone)
    {
        string fullFileName = "";
#if UNITY_EDITOR
#elif UNITY_ANDROID
        fullFileName = "file://";
#endif
        fullFileName += Application.persistentDataPath + "/" + fileName + ".mp3";
        print("GetMp3: " + fullFileName);
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fullFileName, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                OnDone(myClip);
            }
        }
    }
    public IEnumerator GetImage(string fileName, System.Action<Texture2D> OnDone)
    {
        string fullFileName = "";
#if UNITY_EDITOR
#elif UNITY_ANDROID
        fullFileName = "file://";
#endif
        fullFileName += Application.persistentDataPath + "/" + fileName + ".png";
        Debug.Log("Get image: " + fullFileName);
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(fullFileName))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Texture2D sprite = DownloadHandlerTexture.GetContent(www);
                OnDone(sprite);
            }
        }
    }
}
