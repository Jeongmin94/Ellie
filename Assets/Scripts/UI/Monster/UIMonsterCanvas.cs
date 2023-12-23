using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.UI.Framework.Images;
using Assets.Scripts.UI.Framework.Static;
using Assets.Scripts.UI.Player;
using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Monster
{
    public class UIMonsterCanvas : UIStatic
    {
        private const string NameBarImage = "BarImage";
        private const string NameMonsterText = "MonsterText";

        [SerializeField] protected float time = 1.0f;

        private readonly Queue<ImageChangeInfo> healthQueue = new();
        private UIBarImage barImage;
        private int maxHealth;

        protected GameObject monsterPanel;
        protected RectTransform monsterPanelRect;
        private TextMeshProUGUI monsterText;
        private int prevHealth;

        private void Awake()
        {
            Init();
        }

        protected virtual void Start()
        {
            var go = GetGameObject((int)GameObjects.MonsterPanel);

            var image = go.FindChild(NameBarImage, true);
            barImage = image.GetOrAddComponent<UIBarImage>();
        }

        protected override void Init()
        {
            base.Init();

            Bind<GameObject>(typeof(GameObjects));
            monsterPanel = GetGameObject((int)GameObjects.MonsterPanel);
            monsterText = monsterPanel.FindChild<TextMeshProUGUI>();
            monsterPanelRect = monsterPanel.GetComponent<RectTransform>();
        }

        public void InitData(MonsterDataContainer container)
        {
            prevHealth = container.PrevHp;
            maxHealth = container.MaxHp;
            container.CurrentHp.Subscribe(OnChangeHealth);
            SetName(container.Name);
        }

        private void SetName(string monsterName)
        {
            monsterText.text = monsterName;
        }

        private void OnChangeHealth(int value)
        {
            if (prevHealth == value)
            {
                return;
            }

            var target = value / (float)maxHealth;
            var t = time;

            if (prevHealth > value)
            {
                barImage.ChangeImageFillAmount(FillAmountType.Foreground, target);
                healthQueue.Enqueue(new ImageChangeInfo(prevHealth, value, target, t, FillAmountType.Midground));
            }
            else if (prevHealth < value)
            {
                barImage.ChangeImageFillAmount(FillAmountType.Midground, target);
                healthQueue.Enqueue(new ImageChangeInfo(prevHealth, value, target, t, FillAmountType.Foreground));
            }

            prevHealth = value;
            StartCoroutine(ChangeStaminaImageLerp());
        }

        private IEnumerator ChangeStaminaImageLerp()
        {
            if (healthQueue.Any())
            {
                var info = healthQueue.Dequeue();
                yield return barImage.ChangeImageFillAmount(info.Type, info.Target, info.Time);
            }
        }

        private enum GameObjects
        {
            MonsterPanel
        }
    }
}