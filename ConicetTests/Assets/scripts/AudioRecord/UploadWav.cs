using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UploadWav : MonoBehaviour
{
    // string url = "http://pontura.com/conicet/uploadAudio.php";
    string url = "https://conicet.casacam.net/produccion/sync/upload/";
  //  string url = "https://ciipme-voc.wnpower.host/produccion/sync/upload/";
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
            field.text = "Último enviado: " + lastIDSended.ToString("0000");
    }
    public void UploadAll(System.Action<bool> OnDone)
    {
        id = 0;
        this.OnDone = OnDone;
        paths = new List<string>();


        string worldsFolder = System.IO.Path.Combine(Application.persistentDataPath, "tests");

        print("upload all : " + worldsFolder);
        DirectoryInfo d = new DirectoryInfo(worldsFolder);
        foreach (var file in d.GetFiles("*.mp3"))
        {
            string fileName = file.Name.Split("."[0])[0];
            int idSended = int.Parse(fileName.Split("-"[0])[1]);

            if (idSended > lastIDSended)
            {
                print("fileName:" + fileName + ". Last ID Sended " + lastIDSended);
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

            lastIDSended = int.Parse(paths[id - 1].Split("-"[0])[1]);
            field.text = "Enviados " + id;
            PlayerPrefs.SetInt("lastIDSended", lastIDSended);
        }
    }

    IEnumerator Upload(string filename, System.Action<bool> OnReady)
    {
        UnityWebRequest request = new UnityWebRequest();
        WWWForm form = new WWWForm();


        string fullPath = "";
#if UNITY_EDITOR
#elif UNITY_ANDROID
        fullPath = "file://";
#endif
        fullPath = fullPath + Application.persistentDataPath + "/tests/" + filename;
        //string fullPath = System.IO.Path.Combine(Application.persistentDataPath, "tests", filename);

        string path = fullPath + ".mp3";
        Debug.Log("Upload: " + path);
        request = UnityWebRequest.Get(path);
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
        print("MANDA: uuid: " + filename);

        form.AddField("uuid", filename);
        Debug.Log("url: " + url);

        UnityWebRequest req = UnityWebRequest.Post(url, form);
        yield return req.SendWebRequest();

        if (req.isHttpError || req.isNetworkError)
        {
            Events.Log(req.error);
            OnReady(false);
        }
        else
        {
            Events.Log("Request response: " + req.result + " text: " + req.downloadHandler.text);
            Events.Log("Uploaded " + filename);
            OnReady(true);
        }
    }
   



}
