using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading;

public class AudioRecorder : MonoBehaviour
{
    AudioClip recording;
    //Keep this one as a global variable (outside the functions) too and use GetComponent during start to save resources
    AudioSource audioSource;
    private float startRecordingTime;

    //Get the audiosource here to save resources
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            Init();
        if (Input.GetKeyUp(KeyCode.A))
            Stop();
    }
    public void Init()
    {
        Events.Log("Start Recording");
        //Get the max frequency of a microphone, if it's less than 44100 record at the max frequency, else record at 44100
        int minFreq;
        int maxFreq;
        int freq = 22500;
        Microphone.GetDeviceCaps("", out minFreq, out maxFreq);
        if (maxFreq < 22500)
            freq = maxFreq;

        recording = Microphone.Start("", false, 10, freq);
        startRecordingTime = Time.time;     

    }
    public void Stop()
    {
        Events.Log("Stop Recoding");
        Microphone.End("");

        AudioClip recordingNew = AudioClip.Create(recording.name, (int)((Time.time - startRecordingTime) * recording.frequency), recording.channels, recording.frequency, false);
        float[] data = new float[(int)((Time.time - startRecordingTime) * recording.frequency)];
        recording.GetData(data, 0);
        recordingNew.SetData(data, 0);
        this.recording = recordingNew;

        if (recording != null)
        {
            string randomName = System.DateTime.Now.ToString().Replace("/", "-").Replace(":", "-").Replace(".", "-").Replace(" ", "-");
            string fileName = Application.persistentDataPath + "/" + randomName + ".wav";
            SaveWav.Save(fileName, recording);
            GetComponent<UploadWav>().UploadToWeb(recording, fileName);
        }


    }

    

}