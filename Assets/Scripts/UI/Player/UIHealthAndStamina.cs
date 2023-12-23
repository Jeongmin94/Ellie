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

        private readonly List<HealthImageInfo> healthImageInfos = new();
        private readonly Queue<ImageChangeInfo> healthQueue = new();

        private readonly Queue<Rect> healthRectQueue = new();
        private readonly Queue<Vector2> healthScaleQueue = new();

        private readonly Queue<ImageChangeInfo> staminaQueue = new();
        private readonly Queue<Rect> staminaRectQueue = new();
        private readonly Queue<Vector2> staminaScaleQueue = new();

        private GameObject healthPanel;

        private RectTransform healthPanelRect;

        private int prevHealth;
        private float prevStamina;
        private GameObject staminaPanel;
        private RectTransform staminaPanelRect;

        public UIBarImage BarImage { get; private set; }

        private void Awake()
        {
            Init();
            InitTransformData();
        }

        private void Start()
        {
            for (var i = 0; i < healthData.MaxHealth / division; i++)
            {
                var go = UIManager.Instance.MakeSubItem<UIPlayerHealthImage>(healthPanel.transform, UINameHealthImage);

                for (var j = 0; j < division; j++)
                {
                    var info = new HealthImageInfo(go, division, j);
                    healthImageInfos.Add(info);
                }
            }


            prevHealth = healthImageInfos.Count;
            prevStamina = staminaData.CurrentStamina.Value;

            //barImage.midgroundColor.a = 0;
            var color = Color.white;
            color.a = 0;
            BarImage.MidgroundColor = color;
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            if (healthRectQueue.Any())
            {
                SetRect(healthPanelRect, healthRectQueue.Dequeue());
            }

            if (healthScaleQueue.Any())
            {
                SetScale(healthPanelRect, healthScaleQueue.Dequeue());
            }

            if (staminaRectQueue.Any())
            {
                SetRect(staminaPanelRect, staminaRectQueue.Dequeue());
            }

            if (staminaScaleQueue.Any())
            {
                SetScale(staminaPanelRect, staminaScaleQueue.Dequeue());
            }
#endif
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

            BarImage =
                UIManager.Instance.MakeSubItem<UIBarImage>(staminaPanel.transform, UINameBarImage);
            BarImage.transform.position = staminaPanel.transform.position;

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

        private void SubscribeAction()
        {
            healthData.CurrentHealth.Subscribe(OnChangeHealth);
            staminaData.CurrentStamina.Subscribe(OnChangeStamina);
        }

        private void OnChangeHealth(int value)
        {
            if (prevHealth == value)
            {
                return;
            }

            var count = Math.Abs(prevHealth - value);
            var t = time / (division * count);

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
            if (Equals(prevStamina, value))
            {
                return;
            }

            var target = value / staminaData.MaxStamina;
            var t = time / division;

            if (prevStamina > value)
            {
                BarImage.ChangeImageFillAmount(FillAmountType.Foreground, target);
                staminaQueue.Enqueue(new ImageChangeInfo(prevStamina, value, target, t, FillAmountType.Midground));
            }
            else if (prevStamina < value)
            {
                BarImage.ChangeImageFillAmount(FillAmountType.Midground, target);
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
                var prev = (int)info.Prev;
                var current = (int)info.Current;
                var count = Math.Abs(prev - current);

                if (prev > current)
                {
                    for (var i = prev; i > current; i--)
                    {
                        healthImageInfos[i - 1].ChangeImageFillAmount(FillAmountType.Foreground);
                    }

                    yield return new WaitForSeconds(time / count);

                    for (var i = prev; i > current; i--)
                    {
                        yield return healthImageInfos[i - 1].ChangeImageFillAmount(FillAmountType.Midground, info.Time);
                    }
                }
                else if (prev < current)
                {
                    for (var i = prev; i < current; i++)
                    {
                        healthImageInfos[i]
                            .ChangeImageFillAmount(FillAmountType.Midground, true);
                    }

                    for (var i = prev; i < current; i++)
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
                yield return BarImage.ChangeImageFillAmount(info.Type, info.Target, info.Time);
            }
        }

        private enum GameObjects
        {
            HealthPanel,
            StaminaPanel
        }
    }
}