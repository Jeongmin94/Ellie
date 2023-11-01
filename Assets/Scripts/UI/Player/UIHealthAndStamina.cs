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
        private const string UINameHealthImage = "HealthImage";
        private const string UINameBarImage = "BarImage";

        [SerializeField] private float time = 1.0f;
        [SerializeField] private int division = 2;
        [SerializeField] private PlayerHealthData healthData;
        [SerializeField] private StaminaData staminaData;

        private readonly Queue<ImageChangeInfo> staminaQueue = new Queue<ImageChangeInfo>();
        private readonly Queue<ImageChangeInfo> healthQueue = new Queue<ImageChangeInfo>();

        private readonly List<HealthImageInfo> healthImageInfos = new List<HealthImageInfo>();
        private UIBarImage barImage;

        public UIBarImage BarImage
        {
            get { return barImage; }
        }

        private GameObject healthPanel;
        private GameObject staminaPanel;
        private int prevHealth;
        private float prevStamina;

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
            barImage =
                UIManager.Instance.MakeSubItem<UIBarImage>(staminaPanel.transform, UINameBarImage);
            barImage.transform.position = staminaPanel.transform.position;
            
        }

        private void OnEnable()
        {
            SubscribeAction();
        }

        private void SubscribeAction()
        {
            healthData.CurrentHealth.Subscribe(OnChangeHealth);
            staminaData.CurrentStamina.Subscribe(OnChangeStamina);
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
            prevStamina = staminaData.CurrentStamina.Value;

            //barImage.midgroundColor.a = 0;
            Color color = Color.white;
            color.a = 0;
            barImage.MidgroundColor = color;
        }

        private void OnChangeHealth(int value)
        {
            if (prevHealth == value)
                return;

            int count = Math.Abs(prevHealth - value);
            float t = time / (division * count);

            if (prevHealth > value)
            {
                healthQueue.Enqueue(new ImageChangeInfo(prevHealth, value, 0.0f, t));
            }
            else if (prevHealth < value)
            {
                healthQueue.Enqueue(new ImageChangeInfo(prevHealth, value, 0.0f, t));
            }

            prevHealth = value;
            StartCoroutine(ChangeHealthImageLerp());
        }

        private void OnChangeStamina(float value)
        {
            if (MathF.Equals(prevStamina, value))
                return;

            float target = value / (float)staminaData.MaxStamina;
            float t = time / division;

            if (prevStamina > value)
            {
                barImage.ChangeImageFillAmount(FillAmountType.Foreground, target);
                staminaQueue.Enqueue(new ImageChangeInfo(prevStamina, value, target, t, FillAmountType.Midground));
            }
            else if (prevStamina < value)
            {
                barImage.ChangeImageFillAmount(FillAmountType.Midground, target);
                staminaQueue.Enqueue(new ImageChangeInfo(prevStamina, value, target, t, FillAmountType.Foreground));
            }

            prevStamina = value;
            StartCoroutine(ChangeStaminaImageLerp());
        }

        private IEnumerator ChangeHealthImageLerp()
        {
            if (healthQueue.Any())
            {
                var info = healthQueue.Dequeue();
                int prev = (int)info.Prev;
                int current = (int)info.Current;
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
                        yield return healthImageInfos[i - 1].ChangeImageFillAmount(FillAmountType.Midground, info.Time);
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
                            .ChangeImageFillAmount(FillAmountType.Foreground, info.Time, true);
                    }
                }
            }
        }

        private IEnumerator ChangeStaminaImageLerp()
        {
            if (staminaQueue.Any())
            {
                var info = staminaQueue.Dequeue();
                yield return barImage.ChangeImageFillAmount(info.Type, info.Target, info.Time);
            }
        }
    }
}