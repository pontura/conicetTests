using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static DatabaseContent;

namespace Conicet.UI
{
    public class UISettings : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        [SerializeField] Dropdown dropDown;
        [SerializeField] Text sendField;
        [SerializeField] InputField tabletInputField;

        [SerializeField] InputField sampleRate;

        List<Dropdown.OptionData> arr;

        void Start()
        {
            panel.SetActive(false);
        }
        public void Init()
        {
            Data.Instance.uploadWav.Init();
            panel.SetActive(true);
            tabletInputField.text = Data.Instance.tabletID.ToString();
            AddTests();
        }
        void AddTests()
        {
            dropDown.ClearOptions();
            arr = new List<Dropdown.OptionData>();
            foreach (DatabaseContent.ConfigProfiles cf in Data.Instance.databaseContent.main.config_profiles)
            { 
                arr.Add(new Dropdown.OptionData(cf.name)); 
            }
            dropDown.AddOptions(arr);
            if(Data.Instance.databaseContent.GetActive().id>0)
            {
                int id = 0;
                foreach(ConfigProfiles c in Data.Instance.databaseContent.main.config_profiles)
                {
                    if(c.id == Data.Instance.databaseContent.GetActive().id)
                    {
                        dropDown.value = id;
                    }
                    id++;
                }
            }
               
        }
        public void Back()
        {
            panel.SetActive(false);
            Data.Instance.tabletID = int.Parse(tabletInputField.text);
            Data.Instance.SetSampleRate(int.Parse(sampleRate.text));
            Dropdown.OptionData data = arr[dropDown.value];
            int id = Data.Instance.databaseContent.GetConfig(arr[dropDown.value].text).id;
            Data.Instance.databaseContent.SetActive(id);
            UIMain.Instance.Init();
        }
        public void Refresh()
        {
            print("Refresh");
        }
        public void Send()
        {
            print("Send");
            Data.Instance.uploadWav.UploadAll(OnDone);            
        }
        void OnDone(bool isOk)
        {
            Debug.Log("is ok: " + isOk);
        }
    }
}
