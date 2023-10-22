using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Data.ActionData.Player;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Static;
using UnityEngine;

namespace Assets.Scripts.UI.Player
{
    public readonly struct HealthImageInfo
    {
        private readonly UIPlayerHealthImage image;
        private readonly int total;
        private readonly int idx;

        public HealthImageInfo(UIPlayerHealthImage image, int total, int idx)
        {
            this.image = image;
            this.total = total;
            this.idx = idx;
        }

        public IEnumerator ChangeImageFillAmount(FillAmountType type, float time, bool reverse = false)
        {
            return image.ChangeImageFillAmount(type, total, idx, time, reverse);
        }

        public void ChangeImageFillAmount(FillAmountType type, bool reverse = false)
        {
            image.ChangeImageFillAmount(type, total, idx, reverse);
        }
    }

    public class UIPlayerHealth : UIStatic
    {
        private const string UINameHealthImage = "HealthImage";

        [SerializeField] private float time = 1.0f;
        [SerializeField] private int division = 2;
        [SerializeField] private PlayerHealthData healthData;

        private readonly List<HealthImageInfo> healthImageInfos = new List<HealthImageInfo>();
        private GameObject healthPanel;
        private int prevHealth;

        private enum GameObjects
        {
            HealthPanel
        }

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            Bind<GameObject>(typeof(GameObjects));
            healthPanel = GetGameObject((int)GameObjects.HealthPanel);
        }

        private void OnEnable()
        {
            SubscribeAction();
        }

        private void SubscribeAction()
        {
            healthData.CurrentHealth.OnChange -= OnChangeHealth;
            healthData.CurrentHealth.OnChange += OnChangeHealth;
        }

        private void OnChangeHealth(int value)
        {
            StartCoroutine(ChangeHealthImage(value));
        }

        private IEnumerator ChangeHealthImage(int value)
        {
            int count = Math.Abs(prevHealth - value);
            if (prevHealth > value)
            {
                for (int i = prevHealth; i > value; i--)
                {
                    healthImageInfos[i - 1].ChangeImageFillAmount(FillAmountType.Foreground);
                }

                yield return new WaitForSeconds(time / count);
                
                for (int i = prevHealth; i > value; i--)
                {
                    yield return healthImageInfos[i - 1]
                        .ChangeImageFillAmount(FillAmountType.Midground, time / (division * count));
                }
            }
            else if (prevHealth < value)
            {
                for (int i = prevHealth; i < value; i++)
                {
                    yield return healthImageInfos[i]
                        .ChangeImageFillAmount(FillAmountType.Foreground, time / (division * count), true);
                }

                for (int i = prevHealth; i < value; i++)
                {
                    healthImageInfos[i]
                        .ChangeImageFillAmount(FillAmountType.Midground, true);
                }
            }

            prevHealth = value;
        }

        private void Start()
        {
            for (int i = 0; i < healthData.MaxHealth / division; i++)
            {
                var go = UIManager.Instance.MakeSubItem<UIPlayerHealthImage>(healthPanel.transform, UINameHealthImage);

                for (int j = 0; j < division; j++)
                {
                    HealthImageInfo info = new HealthImageInfo(go, division, j);
                    healthImageInfos.Add(info);
                }
            }

            prevHealth = healthImageInfos.Count;
        }
    }
}