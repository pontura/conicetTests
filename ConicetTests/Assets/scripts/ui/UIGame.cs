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
        AudioSource audioSource;
        public states state;
        public enum states
        {
            STOP,
            RECORDING            
        }
        float timer;
        int id;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            panel.SetActive(false);
        }
        public void Init()
        {
            timer = 0;
            state = states.RECORDING;
            panel.SetActive(true);
            audioRecorder.Init(1000);
            testField.text = "TEST " + Data.Instance.GetFileName();
            Next();
        }
        void OnAudioLoaded(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        public void Cancel()
        {
            UIMain.Instance.Init();
            panel.SetActive(false);
        }
        public void Next()
        {
            DatabaseContent.ConfigProfiles active = Data.Instance.databaseContent.GetActive();
            string clipName = active.content.session.levels[0].stages[id].asset;
            print("clipName" + clipName);
            StartCoroutine(Data.Instance.databaseContent.GetMp3(clipName, OnAudioLoaded));
            id++;
            // audioRecorder.Stop();
        }
        private void Update()
        {
            if (state == states.RECORDING)
                timer += Time.deltaTime;

            field.text = ((int)timer).ToString();

        }
    }
}
