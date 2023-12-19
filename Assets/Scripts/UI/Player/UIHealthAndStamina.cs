using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Data.ActionData.Player;
using Assets.Scripts.Data.UI.Transform;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Images;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Framework.Static;
using Assets.Scripts.UI.Inventory;
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
        private const string UINameBarImage = "StaminaImage";

        [SerializeField] private UITransformData playerHealthTransformData;
        [SerializeField] private UITransformData playerStaminaTransformData;
        [SerializeField] private float time = 1.0f;
        [SerializeField] private int division = 2;
        [SerializeField] private PlayerHealthData healthData;
        [SerializeField] private StaminaData staminaData;

        private readonly Queue<ImageChangeInfo> staminaQueue = new Queue<ImageChangeInfo>();
        private readonly Queue<ImageChangeInfo> healthQueue = new Queue<ImageChangeInfo>();

        private readonly List<HealthImageInfo> healthImageInfos = new List<HealthImageInfo>();
        private UIBarImage barImage;

        private readonly Queue<Rect> healthRectQueue = new Queue<Rect>();
        private readonly Queue<Vector2> healthScaleQueue = new Queue<Vector2>();
        private readonly Queue<Rect> staminaRectQueue = new Queue<Rect>();
        private readonly Queue<Vector2> staminaScaleQueue = new Queue<Vector2>();

        public UIBarImage BarImage
        {
            get { return barImage; }
        }

        private GameObject healthPanel;
        private GameObject staminaPanel;

        private RectTransform healthPanelRect;
        private RectTransform staminaPanelRect;

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
            InitTransformData();
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            if (healthRectQueue.Any())
                SetRect(healthPanelRect, healthRectQueue.Dequeue());
            if (healthScaleQueue.Any())
                SetScale(healthPanelRect, healthScaleQueue.Dequeue());

            if (staminaRectQueue.Any())
                SetRect(staminaPanelRect, staminaRectQueue.Dequeue());
            if (staminaScaleQueue.Any())
                SetScale(staminaPanelRect, staminaScaleQueue.Dequeue());
#endif
        }

        private void SetRect(RectTransform rectTransform, Rect rect)
        {
            AnchorPresets.SetAnchorPreset(rectTransform, AnchorPresets.MiddleCenter);
            rectTransform.sizeDelta = rect.GetSize();
            rectTransform.localPosition = rect.ToCanvasPos();
        }

        private void SetScale(RectTransform rectTransform, Vector2 scale)
        {
            rectTransform.localScale = scale;
        }

        protected override void Init()
        {
            base.Init();

            Bind<GameObject>(typeof(GameObjects));
            healthPanel = GetGameObject((int)GameObjects.HealthPanel);
            staminaPanel = GetGameObject((int)GameObjects.StaminaPanel);

            healthPanelRect = healthPanel.GetComponent<RectTransform>();
            staminaPanelRect = staminaPanel.GetComponent<RectTransform>();

            barImage =
                UIManager.Instance.MakeSubItem<UIBarImage>(staminaPanel.transform, UINameBarImage);
            barImage.transform.position = staminaPanel.transform.position;

            AnchorPresets.SetAnchorPreset(healthPanelRect, AnchorPresets.MiddleCenter);
            healthPanelRect.sizeDelta = playerHealthTransformData.actionRect.Value.GetSize();
            healthPanelRect.localPosition = playerHealthTransformData.actionRect.Value.ToCanvasPos();
            healthPanelRect.localScale = playerHealthTransformData.actionScale.Value;

            AnchorPresets.SetAnchorPreset(staminaPanelRect, AnchorPresets.MiddleCenter);
            staminaPanelRect.sizeDelta = playerStaminaTransformData.actionRect.Value.GetSize();
            staminaPanelRect.localPosition = playerStaminaTransformData.actionRect.Value.ToCanvasPos();
            staminaPanelRect.localScale = playerStaminaTransformData.actionScale.Value;
        }

        private void InitTransformData()
        {
#if UNITY_EDITOR
            playerHealthTransformData.actionRect.Subscribe(OnHealthRectChange);
            playerHealthTransformData.actionScale.Subscribe(OnHealthScaleChange);

            playerStaminaTransformData.actionRect.Subscribe(OnStaminaRectChange);
            playerStaminaTransformData.actionScale.Subscribe(OnStaminaScaleChange);
#endif
        }

        private void OnHealthRectChange(Rect rect)
        {
            healthRectQueue.Enqueue(rect);
        }

        private void OnHealthScaleChange(Vector2 scale)
        {
            healthScaleQueue.Enqueue(scale);
        }

        private void OnStaminaRectChange(Rect rect)
        {
            staminaRectQueue.Enqueue(rect);
        }

        private void OnStaminaScaleChange(Vector2 scale)
        {
            staminaScaleQueue.Enqueue(scale);
        }


        private void OnEnable()
        {
            SubscribeAction();
        }

        private void OnDisable()
        {
            healthData.CurrentHealth.Unsubscribe(OnChangeHealth);
            staminaData.CurrentStamina.Unsubscribe(OnChangeStamina);
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