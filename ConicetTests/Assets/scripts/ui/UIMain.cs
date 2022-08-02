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
        [SerializeField] Text field;
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
            string worldsFolder = Application.persistentDataPath;
            DirectoryInfo d = new DirectoryInfo(worldsFolder);
            foreach (var file in d.GetFiles("*.wav"))
            {
                field.text += "file:/" + file;
            }
            Init();
        }
        public void Init()
        {
            Data.Instance.tabletID = PlayerPrefs.GetInt("tabletID", 0);
            Data.Instance.userAutoIncrementID = PlayerPrefs.GetInt("userAutoIncrementID", 0);

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
            Events.Log += Log;
        }
        void Log(string s)
        {
            CancelInvoke();
            debugField.text = s;
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
