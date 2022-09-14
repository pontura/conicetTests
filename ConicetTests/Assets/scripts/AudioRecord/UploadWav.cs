using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.Networking;

public class UploadWav : MonoBehaviour
{
    // string url = "http://pontura.com/conicet/uploadAudio.php";
    string url = "http://ciipme-voc.wnpower.host/produccion/sync/upload/";

    System.Action<bool> OnDone;
    int id = 0;
    public List<string> paths;
    [SerializeField] Text field;
    public int lastIDSended;
    private void Start()
    {
        lastIDSended = PlayerPrefs.GetInt("lastIDSended", 0);
    }
    public void Init()
    {
        if (lastIDSended > 0)
            field.text = "Último enviado: " + lastIDSended;
    }
    public void UploadAll(System.Action<bool> OnDone)
    {
        id = 0;
        this.OnDone = OnDone;
        paths = new List<string>();

        string worldsFolder = System.IO.Path.Combine(Application.persistentDataPath, "tests", Data.Instance.GetFileName());

        print("upload all : " + worldsFolder);
        DirectoryInfo d = new DirectoryInfo(worldsFolder);
        foreach (var file in d.GetFiles("*.mp3"))
        {
            string fileName = file.Name.Split("."[0])[0];

            if (int.Parse(fileName) > lastIDSended)
            {
                print("fileName: " + fileName + "  lastIDSended " + lastIDSended);
                paths.Add(fileName);
            }
            // StartCoroutine(LoadFile(file.Name));
        }
        if(paths.Count == 0)
            field.text = "Nada nuevo para enviar!";
        else
        {
            field.text = "enviar: " + paths.Count;
            SendNext();
        }
    }
    void SendNext()
    {
        StartCoroutine(Upload(paths[id], OnReady));        
    }
    void OnReady(bool isOk)
    {
        field.text = "Enviando: " + paths[id];

        id++;
        if (id <= paths.Count-1)
            SendNext();
        else
        {
            lastIDSended = int.Parse(paths[id - 1]);
            field.text = "Enviados " + id;
            PlayerPrefs.SetInt("lastIDSended", lastIDSended);
        }
    }



    //        UnityWebRequest[] files = new UnityWebRequest[path.Count];

    //        WWWForm form = new WWWForm();

    //        for (int i = 0; i < files.Length; i++)
    //        {

    //#if UNITY_EDITOR
    //            string p = Application.persistentDataPath + "/test/" + path[i];
    //            files[i] = UnityWebRequest.Get(p);
    //            print(p);
    //#else

    //            string p = "file://" + Application.persistentDataPath + "/test/" + path[i];
    //            files[i] = UnityWebRequest.Get(p);
    //#endif

    //            yield return files[i].SendWebRequest();
    //            form.AddBinaryData("files[]", files[i].downloadHandler.data, Path.GetFileName(path[i]));

    //        }

    //        UnityWebRequest req = UnityWebRequest.Post(url, form);
    //        yield return req.SendWebRequest();

    //        if (req.isHttpError || req.isNetworkError)
    //            Events.Log(req.error);
    //        else
    //            Debug.Log("Uploaded ");
    // }

    IEnumerator Upload(string filename, System.Action<bool> OnReady)
    {
        UnityWebRequest request = new UnityWebRequest();
        WWWForm form = new WWWForm();

        string fullPath = System.IO.Path.Combine(Application.persistentDataPath, "tests", filename);
        string path = fullPath + ".mp3";
#if UNITY_EDITOR
        request = UnityWebRequest.Get(path);
#else
        request = UnityWebRequest.Get("file:/" + path);
#endif
        yield return request.SendWebRequest();

        form.AddBinaryData("files", request.downloadHandler.data, filename + ".mp3");
        
        path = fullPath + ".json";
        using (UnityWebRequest www = UnityWebRequest.Get(path))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string jsonData = (www.downloadHandler.text);
                form.AddField("data", jsonData);
            }
        }


        form.AddField("uuid", filename);
        Debug.Log("url: " + url + form);

        UnityWebRequest req = UnityWebRequest.Post(url, form);
        yield return req.SendWebRequest();

        if (req.isHttpError || req.isNetworkError)
        {
            Events.Log(req.error);
            OnReady(false);
        }
        else
        {
            Events.Log("Uploaded " + filename);
            OnReady(true);
        }
    }
   



}
