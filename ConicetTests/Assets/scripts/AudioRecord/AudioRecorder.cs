using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading;
using System.IO;

public class AudioRecorder : MonoBehaviour
{
    AudioClip recording;
    AudioSource audioSource;
    private float startRecordingTime;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
       // StartCoroutine(aaaaaaaaaaa());
    }

    //IEnumerator aaaaaaaaaaa()
    //{
    //    string urlDemo = "http://ciipme-voc.wnpower.host/pruebas_produccion/esp2022/audios/esp2022-anteojo.mp3";
    //    print("LoadFile " + urlDemo);
    //    using (var www = new WWW(urlDemo))
    //    {
    //        yield return www;
    //        print("loaded " + urlDemo);
    //        AudioClip audioClip = www.GetAudioClip();
    //        audioSource.clip = audioClip;
    //        audioSource.Play();
    //    }
    //}
    public void PPPPP(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void Init(float timer)
    {
        Invoke("Stop", timer);
        Events.Log("Start Recording");
        int minFreq;
        int maxFreq;
        //int freq = 22500;
        int freq = 44100;
        Microphone.GetDeviceCaps("", out minFreq, out maxFreq);
        if (maxFreq < freq)
            freq = maxFreq;

        recording = Microphone.Start("", false, 1000, freq);
        startRecordingTime = Time.time;     

    }
    public void Stop()
    {
        CancelInvoke();
        print("Stop Recoding");
        Microphone.End("");
        if (recording == null)
        {
            Events.Log("No mic available");
        }

        AudioClip recordingNew = AudioClip.Create(recording.name, (int)((Time.time - startRecordingTime) * recording.frequency), recording.channels, recording.frequency, false);
        if (recordingNew == null)
        {
            Events.Log("No recordingNew available");
        }

        float[] data = new float[(int)((Time.time - startRecordingTime) * recording.frequency)];       
        recording.GetData(data, 0);       
        recordingNew.SetData(data, 0);
        this.recording = recordingNew;



        if (recording != null)
        {
            string folder = "tests";

            string fullFileName = "";
#if UNITY_EDITOR
#elif UNITY_ANDROID
        fullFileName = "file://";
#endif
            string Fullfolder = fullFileName + Application.persistentDataPath + "/" + folder;
            Debug.Log("Folder: " + Fullfolder);

           // string path = "/storage/emulated/0/Android/data/com.conicet.ConicetTests/files/tests";
            string path = System.IO.Path.Combine(Application.persistentDataPath, folder);

            Debug.Log("Exis path: " + path + " ?");

            if (!Directory.Exists(path))
            {
                Debug.Log("Exists path: " + path + " ?");
                Directory.CreateDirectory(path);
            }

            path = System.IO.Path.Combine(Application.persistentDataPath, folder, Data.Instance.GetFileName());

            string jsonFile = path + ".json";

            Debug.Log("jsonFile: " + jsonFile);
            string json = JsonUtility.ToJson(Data.Instance.testData);

            Debug.Log("json: " + json);
            File.WriteAllText(jsonFile, json);

            string mp3File = path + ".mp3";
            Debug.Log("mp3File: " + jsonFile);

            Data.Instance.databaseContent.SaveMp3Locally(recording, mp3File);
        }
      
    }
    //public static DirectoryInfo SafeCreateDirectory(string path)
    //{
    //    //Generate if you don't check if the directory exists
    //    if (Directory.Exists(path))
    //    {
    //        return null;
    //    }
    //    return Directory.CreateDirectory(path);
    //}

    //public void Score_Save(string folder, string date)
    //{
    //    //Data storage
    //    SafeCreateDirectory(Application.persistentDataPath + "/" + folder);
       
    //}




}