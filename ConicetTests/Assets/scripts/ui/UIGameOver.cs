using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Conicet.UI
{
    public class UIGameOver : MonoBehaviour
    {
        [SerializeField] Text testField;
        [SerializeField] GameObject panel;

        void Start()
        {
            panel.SetActive(false);
        }
        public void Init()
        {
            testField.text = Data.Instance.GetFileName() + " ha terminado.";
            panel.SetActive(true);
        }
        public void Next()
        {
            GetComponent<UIMain>().Init();
            panel.SetActive(false);
        }
    }
}
