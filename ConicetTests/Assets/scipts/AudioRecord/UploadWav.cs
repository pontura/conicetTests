using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Networking;

public class UploadWav : MonoBehaviour
{
    string url = "http://pontura.com/conicet/uploadAudio.php";
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
      //  StartCoroutine(UploadAll());
    }
    public void UploadToWeb(AudioClip clip, string filename)
    {
        var samples = new float[clip.samples];

        clip.GetData(samples, 0);

        Int16[] intData = new Int16[samples.Length];
        Byte[] bytesData = new Byte[samples.Length * 2];

        int rescaleFactor = 32767;

        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            Byte[] byteArr = new Byte[2];
            byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }
       // StartCoroutine(UploadFile(bytesData));
        StartCoroutine(Send(filename));
    }

    IEnumerator Send(string filename)
    {
        yield return new WaitForSeconds(5);
        string[] path = new string[1];
        path[0] = filename;

        UnityWebRequest[] files = new UnityWebRequest[path.Length];

        WWWForm form = new WWWForm();

        for (int i = 0; i < files.Length; i++)
        {
#if UNITY_EDITOR
            files[i] = UnityWebRequest.Get(path[i]);
#else
            files[i] = UnityWebRequest.Get("file:/" + path[i]);
#endif
            Events.Log(path[i]);

            yield return files[i].SendWebRequest();
            form.AddBinaryData("files[]", files[i].downloadHandler.data, Path.GetFileName(path[i]));

        }

        UnityWebRequest req = UnityWebRequest.Post(url, form);
        yield return req.SendWebRequest();

        if (req.isHttpError || req.isNetworkError)
            Events.Log(req.error);
        else
            Events.Log("Uploaded " + filename);
    }

    IEnumerator UploadAll()
    {
        List<string> path = new List<string>();

        string worldsFolder = Application.persistentDataPath;
        DirectoryInfo d = new DirectoryInfo(worldsFolder);
        foreach (var file in d.GetFiles("*.wav"))
        {
            path.Add(file.Name);
            StartCoroutine(LoadFile(file.Name));
        }

        

        UnityWebRequest[] files = new UnityWebRequest[path.Count];

        WWWForm form = new WWWForm();

        for (int i = 0; i < files.Length; i++)
        {

#if UNITY_EDITOR
            string p = Application.persistentDataPath + "/" + path[i];
            files[i] = UnityWebRequest.Get(p);
            print(p);
#else
            
            string p = "file://" + Application.persistentDataPath + "/" + path[i];
            files[i] = UnityWebRequest.Get(p);
#endif

            yield return files[i].SendWebRequest();
            form.AddBinaryData("files[]", files[i].downloadHandler.data, Path.GetFileName(path[i]));
            
        }

        UnityWebRequest req = UnityWebRequest.Post(url, form);
        yield return req.SendWebRequest();

        if (req.isHttpError || req.isNetworkError)
            Events.Log(req.error);
        else
            Debug.Log("Uploaded ");
    }

    IEnumerator LoadFile(string fullpath)
    {
        AudioClip temp = null;

#if UNITY_EDITOR
        string p = Application.persistentDataPath + "/" + fullpath;
#else
            
            string p = "file://" + Application.persistentDataPath + "/" + fullpath;
#endif

        print("LoadFile " + p);
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(p, AudioType.WAV))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                temp = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = temp;
                audioSource.Play();
            
            }
        }

    }


}
