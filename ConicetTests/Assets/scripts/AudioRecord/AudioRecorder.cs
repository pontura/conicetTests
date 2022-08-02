using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading;

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
        int freq = 22500;
        Microphone.GetDeviceCaps("", out minFreq, out maxFreq);
        if (maxFreq < 22500)
            freq = maxFreq;

        recording = Microphone.Start("", false, 1000, freq);
        startRecordingTime = Time.time;     

    }
    public void Stop()
    {
        CancelInvoke();
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