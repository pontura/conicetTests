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
        [SerializeField] GameObject end;

        [SerializeField] GameObject rewardPanel;
        [SerializeField] Image rewardThumb;
        [SerializeField] Sprite[] rewards;

        [SerializeField] Image image;
        [SerializeField] AudioRecorder audioRecorder;
        AudioSource audioSource;
        [SerializeField] AudioSource audioSourceAlert;
        [SerializeField] Animation anim;
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
            end.SetActive(false);
            Data.Instance.testData.Init(Data.Instance.GetFileName());
            timer = 0;
            state = states.RECORDING;
            panel.SetActive(true);
            audioRecorder.Init(1000);
            testField.text = Data.Instance.GetFileName();
            Next();
        }
        void OnAudioLoaded(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
        }
        void OnTextureLoaded(Texture2D tex)
        {
            image.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            image.SetNativeSize();
            CancelInvoke();
            Invoke("GameRestart", 0.5f);
        }
        void GameRestart()
        {
            CancelInvoke();
            anim.Play("game_init");
        }
        public void Cancel()
        {
            Reset();
            UIMain.Instance.Init();
            panel.SetActive(false);
        }
        DatabaseContent.Levels level;
        public void Next()
        {
            CancelInvoke();
            DatabaseContent.ConfigProfiles active = Data.Instance.databaseContent.GetActive();
            if (active.content.session.levels.Length > levelID)
            {
                if (active.content.session.assets.character == "Nilda")
                    Events.ChangeCharacter(2);
                else
                    Events.ChangeCharacter(1);

                level = active.content.session.levels[levelID];
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
                    if (active.content.session.levels.Length > levelID)
                        SetRewardPanel();
                }
                anim.Play("game_end");
            }
            else
            {
                rewardPanel.SetActive(false);
                end.SetActive(true);
                Events.Log("List! Procesando " + Data.Instance.GetFileName() + "...");
                StartCoroutine(End());
            }
        }

        IEnumerator End()
        {
            yield return new WaitForSeconds(0.15f);
            audioRecorder.Stop();
            rewardPanel.SetActive(false);
            panel.SetActive(false);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            GetComponent<Conicet.UI.UIGameOver>().Init();
            yield return new WaitForEndOfFrame();
            Reset();
            Events.Log("Test grabado!");
        }
        public void OnReadyClicked()
        {
            CancelInvoke();
            anim.Play("game_character_out");
            audioSource.Play();
            Data.Instance.testData.Add(timer);
            int delay = 10;
            if (level != null && level.recording_time > 0)
                delay = level.recording_time;
            Invoke("OnTimeout1", delay);
        }
        void OnTimeout1()
        {
            CancelInvoke();
            Alert();
            int delay = 10;
            if (level != null && level.recording_time > 0)
                delay = level.recording_time;
            Invoke("OnTimeout2", delay);
        }
        void OnTimeout2()
        {
            Alert();
            OnRewardClicked();
        }
        void Alert()
        {
            audioSourceAlert.Play();
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
        bool rewardClicked;
        public void SetRewardPanel()
        {
            CancelInvoke();
            rewardClicked = false;
            rewardPanel.SetActive(true);
            Invoke("OnRewardClicked", 10);
            rewardInt++;
            if (rewardInt >= rewards.Length) rewardInt = 0;
            rewardThumb.sprite = rewards[rewardInt];
        }
        
        public void OnRewardClicked()
        {
            if (rewardClicked) return;
            CancelInvoke();
            rewardClicked = true;
            CancelInvoke();
            levelID++;
            stageID = 0;
            Next();
            SetRewardOff();
        }
        public void SetRewardOff()
        {
            CancelInvoke();
            rewardPanel.SetActive(false);
        }
    }
}
