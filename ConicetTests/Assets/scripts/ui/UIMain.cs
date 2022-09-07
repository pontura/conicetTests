using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Conicet.UI
{
    public class UIMain : MonoBehaviour
    {
        static UIMain mInstance = null;
        [SerializeField] Text debugField;
        [SerializeField] MainMenu mainMenu;

        public static UIMain Instance { get { return mInstance; } }
        void Awake()
        {
            mInstance = this;
        }
        void Start()
        {
            debugField.text = "";
            Events.Log += Log;
            //string worldsFolder = Application.persistentDataPath;
            //DirectoryInfo d = new DirectoryInfo(worldsFolder);
            //foreach (var file in d.GetFiles("*.wav"))
            //{
            //    field.text += "file:/" + file;
            //}
        }
        bool started;
        private void Update()
        {
            if (started) return;
            if(Data.Instance.databaseContent.allLoaded)
            {
                started = true;
                Init();
            }
        }
        public void Init()
        {
            debugField.text = "";
            Debug.Log("Init");
            if (Data.Instance.tabletID == 0)
                GetComponent<DatabaseTablet>().Init();
            else
                MainMenu();
        }
        public void MainMenu()
        {
            mainMenu.Init();
        }
        void OnDestroy()
        {
            Events.Log -= Log;
        }
        void Log(string s)
        {
            CancelInvoke();
            debugField.text = s;
            Debug.Log(s);
        }
        private void Reset()
        {
            debugField.text = "";
        }
        public void OpenSettings()
        {
            GetComponent<UISettings>().Init();
        }
    }
}
