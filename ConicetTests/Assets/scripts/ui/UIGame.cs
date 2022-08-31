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

        [SerializeField] GameObject rewardPanel;
        [SerializeField] Image rewardThumb;
        [SerializeField] Sprite[] rewards;

        [SerializeField] Image image;
        [SerializeField] AudioRecorder audioRecorder;
        AudioSource audioSource;
        public states state;
        public enum states
        {
            STOP,
            RECORDING            
        }
        float timer;
        int levelID;
        int stageID;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            panel.SetActive(false);
            SetRewardOff();
        }
        public void Init()
        {
            Data.Instance.testData.Init(Data.Instance.GetFileName());
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
        void OnTextureLoaded(Texture2D tex)
        {
            image.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            image.SetNativeSize();
            image.GetComponent<Animation>().Play();
        }
        public void Cancel()
        {
            Reset();
            UIMain.Instance.Init();
            panel.SetActive(false);
        }
        public void Next()
        {
            DatabaseContent.ConfigProfiles active = Data.Instance.databaseContent.GetActive();
            if (active.content.session.levels.Length > levelID)
            {
                DatabaseContent.Levels level = active.content.session.levels[levelID];
                if (level.stages.Length > stageID)
                {
                    DatabaseContent.Stages stage = level.stages[stageID];
                    string clipName = stage.asset;

                    print("level: " + levelID + " stage: " + stageID + " clipName" + clipName);
                    StartCoroutine(Data.Instance.databaseContent.GetMp3(clipName, OnAudioLoaded));
                    StartCoroutine(Data.Instance.databaseContent.GetImage(clipName, OnTextureLoaded));

                    stageID++;
                }
                else
                {
                    SetRewardPanel();
                    levelID++;
                    stageID = 0;
                    Next();
                }
                Data.Instance.testData.Add(timer);
            }
            else
            {
                Reset();
                GetComponent<Conicet.UI.UIGameOver>().Init(); 
                audioRecorder.Stop();
                panel.SetActive(false);
            }
        }
        private void Update()
        {
            if (state == states.RECORDING)
                timer += Time.deltaTime;

            field.text = ((int)timer).ToString();

        }
        private void Reset()
        {
            stageID = 0;
            levelID = 0;
            timer = 0;
        }
        int rewardInt;
        public void SetRewardPanel()
        {
            rewardPanel.SetActive(true);
            Invoke("SetRewardOff", 3);
            rewardInt++;
            if (rewardInt >= rewards.Length) rewardInt = 0;
            rewardThumb.sprite = rewards[rewardInt];
        }
        public void SetRewardOff()
        {
            rewardPanel.SetActive(false);
        }
    }
}
