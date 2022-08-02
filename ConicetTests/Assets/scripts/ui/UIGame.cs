using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Conicet.UI
{
    public class UIGame : MonoBehaviour
    {
        [SerializeField] Text field;
        [SerializeField] GameObject panel;
        [SerializeField] AudioRecorder audioRecorder;

        void Start()
        {
            panel.SetActive(false);
        }
        public void Init()
        {
            panel.SetActive(true);
            audioRecorder.Init(5);
        }
        public void Cancel()
        {
            UIMain.Instance.Init();
            panel.SetActive(false);
        }
        public void Next()
        {
            audioRecorder.Stop();
        }
    }
}
