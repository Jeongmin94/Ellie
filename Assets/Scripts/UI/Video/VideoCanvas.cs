using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Centers;
using Assets.Scripts.Data.UI.Video;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Assets.Scripts.UI.Video
{
    public class VideoCanvas : UIPopup
    {
        private enum GameObjects
        {
            VideoRawImage,
            VideoPlayer,
        }

        [SerializeField] private VideoData videoData;

        private readonly List<GameObject> gameObjects = new List<GameObject>();
        private readonly List<RectTransform> rectTransforms = new List<RectTransform>();

        private RawImage videoRawImage;
        private VideoPlayer videoPlayer;

        private TicketMachine ticketMachine;

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
            for (int i = 0; i < gos.Length; i++)
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
                return;

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
            WaitForEndOfFrame wfef = new WaitForEndOfFrame();
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
    }
}