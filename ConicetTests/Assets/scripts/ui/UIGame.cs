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
        [SerializeField] Text testField;
        [SerializeField] GameObject panel;
        [SerializeField] AudioRecorder audioRecorder;
        public states state;
        public enum states
        {
            STOP,
            RECORDING            
        }
        float timer;

        void Start()
        {
            panel.SetActive(false);
        }
        public void Init()
        {
            timer = 0;
            state = states.RECORDING;
            panel.SetActive(true);
            audioRecorder.Init(5);
            testField.text = "TEST " + Data.Instance.GetFileName();
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
        private void Update()
        {
            if (state == states.RECORDING)
                timer += Time.deltaTime;

            field.text = ((int)timer).ToString();

        }
    }
}
