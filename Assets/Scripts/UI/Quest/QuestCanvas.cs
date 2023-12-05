using System;
using System.Collections.Generic;
using Assets.Scripts.Data.UI.Transform;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Framework.Static;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using Data.UI.Opening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Quest
{
    public class QuestCanvas : UIStatic
    {
        private enum GameObjects
        {
            QuestIconPanel,
            QuestNamePanel,
            QuestDescPanel,
        }

        private enum Images
        {
            QuestIconImage
        }

        private enum Texts
        {
            QuestNameText,
            QuestDescText,
        }

        [SerializeField] private UITransformData[] questPanelTransformData;
        [SerializeField] private TextTypographyData[] questTextTypography;

        private readonly List<RectTransform> rectTransforms = new List<RectTransform>();
        private readonly List<TextMeshProUGUI> questTexts = new List<TextMeshProUGUI>();

        private Image questIconImage;

        private TicketMachine ticketMachine;

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

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));
            Bind<Image>(typeof(Images));
            Bind<TextMeshProUGUI>(typeof(Texts));

            var gos = Enum.GetValues(typeof(GameObjects));
            for (int i = 0; i < gos.Length; i++)
            {
                var go = GetGameObject(i);
                rectTransforms.Add(go.GetComponent<RectTransform>());
            }

            var texts = Enum.GetValues(typeof(Texts));
            for (int i = 0; i < texts.Length; i++)
            {
                questTexts.Add(GetText(i));
            }

            questIconImage = GetImage((int)Images.QuestIconImage);
        }

        private void InitObjects()
        {
            for (int i = 0; i < questPanelTransformData.Length; i++)
            {
                AnchorPresets.SetAnchorPreset(rectTransforms[i], AnchorPresets.MiddleCenter);
                rectTransforms[i].sizeDelta = questPanelTransformData[i].actionRect.Value.GetSize();
                rectTransforms[i].localPosition = questPanelTransformData[i].actionRect.Value.ToCanvasPos();
                rectTransforms[i].localScale = questPanelTransformData[i].actionScale.Value;
            }

            for (int i = 0; i < questTextTypography.Length; i++)
            {
                questTexts[i].font = questTextTypography[i].fontAsset;
                questTexts[i].fontSize = questTextTypography[i].fontSize;
                questTexts[i].color = questTextTypography[i].color;
                questTexts[i].lineSpacing = questTextTypography[i].lineSpacing;
            }

            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.UI);
            ticketMachine.RegisterObserver(ChannelType.UI, OnNotifyAction);

            TicketManager.Instance.Ticket(ticketMachine);
        }

        private void OnNotifyAction(IBaseEventPayload payload)
        {
            if (payload is not UIPayload uiPayload)
                return;

            switch (uiPayload.actionType)
            {
                case ActionType.ClearQuest:
                {
                    questIconImage.gameObject.SetActive(false);
                    var texts = Enum.GetValues(typeof(Texts));
                    for (int i = 0; i < texts.Length; i++)
                    {
                        questTexts[i].text = string.Empty;
                    }
                    
                    gameObject.SetActive(false);
                }
                    break;

                case ActionType.SetQuestIcon:
                {
                    gameObject.SetActive(true);

                    questIconImage.gameObject.SetActive(true);
                    questIconImage.sprite = uiPayload.questInfo.questIcon;
                }
                    break;

                case ActionType.SetQuestName:
                {
                    gameObject.SetActive(true);

                    questTexts[(int)Texts.QuestNameText].text = uiPayload.questInfo.questName;
                }
                    break;

                case ActionType.SetQuestDesc:
                {
                    gameObject.SetActive(true);

                    questTexts[(int)Texts.QuestDescText].text = uiPayload.questInfo.questDesc;
                }
                    break;
            }
        }
    }
}