using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using Data.UI.Video;
using Managers.Input;
using Managers.Save;
using Managers.SceneLoad;
using Managers.Ticket;
using UI.Framework.Popup;
using UI.Framework.Presets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Utils;

namespace UI.Video
{
    public class VideoCanvas : UIPopup
    {
        [SerializeField] private VideoData videoData;

        private readonly List<GameObject> gameObjects = new();
        private readonly List<RectTransform> rectTransforms = new();

        private TicketMachine ticketMachine;
        private VideoPlayer videoPlayer;

        private RawImage videoRawImage;

        public bool IsEnd { get; set; } = true;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            Bind();
            InitObjects();
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));

            var gos = Enum.GetValues(typeof(GameObjects));
            for (var i = 0; i < gos.Length; i++)
            {
                var go = GetGameObject(i);
                rectTransforms.Add(go.GetComponent<RectTransform>());
                gameObjects.Add(go);
            }

            videoRawImage = gameObjects[(int)GameObjects.VideoRawImage].GetComponent<RawImage>();
            videoPlayer = gameObjects[(int)GameObjects.VideoPlayer].GetComponent<VideoPlayer>();
        }

        private void InitObjects()
        {
            AnchorPresets.SetAnchorPreset(rectTransforms[(int)GameObjects.VideoRawImage], AnchorPresets.StretchAll);
            rectTransforms[(int)GameObjects.VideoRawImage].sizeDelta = Vector2.zero;
            rectTransforms[(int)GameObjects.VideoRawImage].localPosition = Vector3.zero;

            videoPlayer.playOnAwake = false;
            videoPlayer.clip = videoData.videoClip;
            videoPlayer.loopPointReached -= CheckOver;
            videoPlayer.loopPointReached += CheckOver;

            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTicket(ChannelType.UI);
            ticketMachine.RegisterObserver(ChannelType.UI, OnNotify);

            TicketManager.Instance.Ticket(ticketMachine);

            gameObject.SetActive(false);
        }

        private void OnNotify(IBaseEventPayload payload)
        {
            if (payload is not UIPayload uiPayload)
            {
                return;
            }

            if (uiPayload.actionType == ActionType.PlayVideo)
            {
                if (IsEnd)
                {
                    gameObject.SetActive(true);
                    StartCoroutine(PlayVideo());
                }
            }
        }

        private IEnumerator PlayVideo()
        {
            InputManager.Instance.CanInput = false;
            IsEnd = false;
            Cursor.visible = false;

            videoPlayer.Play();
            var wfef = new WaitForEndOfFrame();
            while (!IsEnd)
            {
                yield return wfef;
            }

            EndVideo();
        }

        public void EndVideo()
        {
            InputManager.Instance.CanInput = true;
            gameObject.SetActive(false);
            Cursor.visible = true;
            SaveLoadManager.Instance.IsLoadData = videoData.isLoadData;
            SceneLoadManager.Instance.LoadScene(videoData.playAfterScene);
        }

        private void CheckOver(VideoPlayer vp)
        {
            IsEnd = true;
        }

        private enum GameObjects
        {
            VideoRawImage,
            VideoPlayer
        }
    }
}