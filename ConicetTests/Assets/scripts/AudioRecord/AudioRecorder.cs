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

        AudioClip recordingNew = AudioClip.Create(recording.name, (int)((Time.time - startRecordingTime) * recording.frequency), recording.channels, recording.frequency, false);
       

        float[] data = new float[(int)((Time.time - startRecordingTime) * recording.frequency)];       
        recording.GetData(data, 0);       
        recordingNew.SetData(data, 0);
        this.recording = recordingNew;



        if (recording != null)
        {
            string folder = "tests";
            Directory.CreateDirectory(Application.persistentDataPath + "/" + folder);

            string fullFileName = Application.persistentDataPath + "/" + folder + "/" + Data.Instance.GetFileName();

            string json = JsonUtility.ToJson(Data.Instance.testData);
            File.WriteAllText(fullFileName + ".json", json);

            Data.Instance.databaseContent.SaveMp3Locally(recording, fullFileName + ".mp3");
        }

    }

    

}