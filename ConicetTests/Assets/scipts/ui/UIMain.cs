using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Conicet.UI
{
    public class UIMain : MonoBehaviour
    {
        [SerializeField] Text debugField;
        [SerializeField] Text field;

        void Start()
        {
            Events.Log += Log;

            string worldsFolder = Application.persistentDataPath;

            DirectoryInfo d = new DirectoryInfo(worldsFolder);
            foreach (var file in d.GetFiles("*.wav"))
            {
                field.text += "file:/" + file;
            }
        }
        void OnDestroy()
        {
            Events.Log += Log;
        }
        void Log(string s)
        {
            print("____DEBUG: " + s);
            CancelInvoke();
            debugField.text = s;
         //   Invoke("Reset", 5);
        }
        private void Reset()
        {
            debugField.text = "";
        }
    }
}
