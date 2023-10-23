using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Data.ActionData.Player;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Images;
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

    public class UIHealthAndStamina : UIStatic
    {
        private struct ImageChangeInfo
        {
            public readonly float Target;
            public readonly float Time;
            public readonly FillAmountType Type;

            public ImageChangeInfo(float target, float time, FillAmountType type)
            {
                Target = target;
                Time = time;
                Type = type;
            }
        }

        private const string UINameHealthImage = "HealthImage";
        private const string UINameStaminaImage = "StaminaImage";

        [SerializeField] private float time = 1.0f;
        [SerializeField] private int division = 2;
        [SerializeField] private PlayerHealthData healthData;
        [SerializeField] private StaminaData staminaData;

        private readonly Queue<ImageChangeInfo> staminaQueue = new Queue<ImageChangeInfo>();
        private readonly Queue<ImageChangeInfo> healthQueue = new Queue<ImageChangeInfo>();

        private readonly List<HealthImageInfo> healthImageInfos = new List<HealthImageInfo>();
        private UIPlayerStaminaImage playerStaminaImage;
        private GameObject healthPanel;
        private GameObject staminaPanel;
        private int prevHealth;
        private int prevStamina;

        private enum GameObjects
        {
            HealthPanel,
            StaminaPanel
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
            staminaPanel = GetGameObject((int)GameObjects.StaminaPanel);
        }

        private void OnEnable()
        {
            SubscribeAction();
        }

        private void SubscribeAction()
        {
            healthData.CurrentHealth.OnChange -= OnChangeHealth;
            healthData.CurrentHealth.OnChange += OnChangeHealth;

            staminaData.CurrentStamina.OnChange -= OnChangeStamina;
            staminaData.CurrentStamina.OnChange += OnChangeStamina;
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

            playerStaminaImage =
                UIManager.Instance.MakeSubItem<UIPlayerStaminaImage>(staminaPanel.transform, UINameStaminaImage);
            playerStaminaImage.transform.position = staminaPanel.transform.position;

            prevHealth = healthImageInfos.Count;
            prevStamina = staminaData.CurrentStamina.Value;
        }
        
        private void OnChangeHealth(int value)
        {
            if (prevHealth == value)
                return;
            float target = value / (float)healthData.MaxHealth;
            FillAmountType type = FillAmountType.Midground;
            int count = Math.Abs(prevHealth - value);
            if (prevHealth < value)
                type = FillAmountType.Foreground;

            healthQueue.Enqueue(new ImageChangeInfo(target, time / (division * count), type));
        }

        private void OnChangeStamina(int value)
        {
            if (prevStamina == value)
                return;

            float target = value / (float)staminaData.MaxStamina;
            float t = time / division;

            if (prevStamina > value)
            {
                playerStaminaImage.ChangeImageFillAmount(FillAmountType.Foreground, target);
                staminaQueue.Enqueue(new ImageChangeInfo(target, t, FillAmountType.Midground));
            }
            else if (prevStamina < value)
            {
                playerStaminaImage.ChangeImageFillAmount(FillAmountType.Midground, target);
                staminaQueue.Enqueue(new ImageChangeInfo(target, t, FillAmountType.Foreground));
            }

            prevStamina = value;
            StartCoroutine(ChangeStaminaImageLerp());
        }

        private IEnumerator ChangeHealthImageLerp()
        {
            if (healthQueue.Any())
            {
                var info = healthQueue.Peek();

                // int count = Math.Abs(prev - current);
                // if (prev > current)
                // {
                //     for (int i = prev; i > current; i--)
                //     {
                //         healthImageInfos[i - 1].ChangeImageFillAmount(FillAmountType.Foreground);
                //     }
                //
                //     yield return new WaitForSeconds(time / count);
                //
                //     for (int i = prev; i > current; i--)
                //     {
                //         yield return healthImageInfos[i - 1]
                //             .ChangeImageFillAmount(FillAmountType.Midground, time / (division * count));
                //     }
                // }
                // else if (prev < current)
                // {
                //     for (int i = prev; i < current; i++)
                //     {
                //         healthImageInfos[i]
                //             .ChangeImageFillAmount(FillAmountType.Midground, true);
                //     }
                //
                //     for (int i = prev; i < current; i++)
                //     {
                //         yield return healthImageInfos[i]
                //             .ChangeImageFillAmount(FillAmountType.Foreground, time / (division * count), true);
                //     }
                // }

                healthQueue.Dequeue();
            }

            yield return new WaitForEndOfFrame();
        }

        private IEnumerator ChangeStaminaImageLerp()
        {
            if (staminaQueue.Any())
            {
                var info = staminaQueue.Dequeue();
                yield return playerStaminaImage.ChangeImageFillAmount(info.Type, info.Target, info.Time);
            }
        }
    }
}