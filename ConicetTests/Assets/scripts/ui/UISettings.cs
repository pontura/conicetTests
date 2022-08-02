using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Conicet.UI
{
    public class UISettings : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        [SerializeField] Dropdown dropDown;
        [SerializeField] Text sendField;
        [SerializeField] InputField tabletInputField;
        List<Dropdown.OptionData> arr;

        void Start()
        {
            panel.SetActive(false);
        }
        public void Init()
        {
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
        }
        public void Back()
        {
            panel.SetActive(false);
            Dropdown.OptionData data = arr[dropDown.value];
            int id = Data.Instance.databaseContent.GetConfig(arr[dropDown.value].text).id;
            Data.Instance.databaseContent.SetActive(id);
            UIMain.Instance.Init();
        }
        public void Refresh()
        {
            print("REFRWSG");
        }
        public void Send()
        {
            print("send");
        }

    }
}
