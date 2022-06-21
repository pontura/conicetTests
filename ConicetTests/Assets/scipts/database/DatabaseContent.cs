using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class DatabaseContent : MonoBehaviour
{
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
        public Stages stages;
    }
    [Serializable]
    public class Stages
    {
        public string object_;
    }
    public IEnumerator Load(System.Action OnLoaded)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string json = www.downloadHandler.text;
                main = JsonUtility.FromJson<Main>(json);

                // Or retrieve results as binary data
                byte[] results = www.downloadHandler.data;

                OnLoaded();
            }
        }
    }
}
