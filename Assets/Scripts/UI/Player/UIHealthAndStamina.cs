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
            public readonly int current;
            public readonly int prev;

            public ImageChangeInfo(int current, int prev)
            {
                this.current = current;
                this.prev = prev;
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

        private void OnChangeHealth(int value)
        {
            // if (prevHealth > value)
            // {
            //     for (int i = prevHealth; i > value; i--)
            //     {
            //         healthImageInfos[i - 1].ChangeImageFillAmount(FillAmountType.Foreground);
            //     }
            // }
            // else if (prevHealth < value)
            // {
            //     for (int i = prevHealth; i < value; i++)
            //     {
            //         healthImageInfos[i]
            //             .ChangeImageFillAmount(FillAmountType.Midground, true);
            //     }
            // }

            healthQueue.Enqueue(new ImageChangeInfo(value, prevHealth));
            prevHealth = value;
            if (healthQueue.Count == 1)
                StartCoroutine(ChangeHealthImage());
        }

        private IEnumerator ChangeHealthImage()
        {
            while (healthQueue.Any())
            {
                var info = healthQueue.Peek();
                int prev = info.prev;
                int current = info.current;

                int count = Math.Abs(prev - current);
                if (prev > current)
                {
                    for (int i = prev; i > current; i--)
                    {
                        healthImageInfos[i - 1].ChangeImageFillAmount(FillAmountType.Foreground);
                    }
                    yield return new WaitForSeconds(time / count);

                    for (int i = prev; i > current; i--)
                    {
                        yield return healthImageInfos[i - 1]
                            .ChangeImageFillAmount(FillAmountType.Midground, time / (division * count));
                    }
                }
                else if (prev < current)
                {
                    for (int i = prev; i < current; i++)
                    {
                        healthImageInfos[i]
                            .ChangeImageFillAmount(FillAmountType.Midground, true);
                    }
                    
                    for (int i = prev; i < current; i++)
                    {
                        yield return healthImageInfos[i]
                            .ChangeImageFillAmount(FillAmountType.Foreground, time / (division * count), true);
                    }
                }

                healthQueue.Dequeue();
            }
        }

        private void OnChangeStamina(int value)
        {
            staminaQueue.Enqueue(new ImageChangeInfo(value, prevStamina));
            prevStamina = value;

            if (staminaQueue.Count == 1)
                StartCoroutine(ChangeStaminaImage());
        }

        private IEnumerator ChangeStaminaImage()
        {
            while (staminaQueue.Any())
            {
                var info = staminaQueue.Dequeue();
                int prev = info.prev;
                int current = info.current;

                if (prev > current)
                {
                    playerStaminaImage.ChangeImageFillAmount(FillAmountType.Foreground, staminaData.GetPercentage());
                    yield return new WaitForSeconds(time);
                    yield return playerStaminaImage.ChangeImageFillAmount(FillAmountType.Midground,
                        staminaData.GetPercentage(), time / division);
                }
                else if (prev < current)
                {
                    playerStaminaImage.ChangeImageFillAmount(FillAmountType.Midground, staminaData.GetPercentage(),
                        true);
                    yield return playerStaminaImage.ChangeImageFillAmount(FillAmountType.Foreground,
                        staminaData.GetPercentage(), time, true);
                }
            }
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
    }
}