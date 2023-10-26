using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.ActionData.Monster;
using Assets.Scripts.UI.Framework.Images;
using Assets.Scripts.UI.Framework.Static;
using Assets.Scripts.UI.Player;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using Codice.Client.BaseCommands;
using TMPro;
using UnityEngine;

// !TODO: HealthData 사용하지 않도록 변경
namespace Assets.Scripts.UI.Monster
{
    public class UIMonsterCanvas : UIStatic
    {
        private enum GameObjects
        {
            MonsterPanel
        }

        private const string NameBarImage = "BarImage";
        private const string NameMonsterText = "MonsterText";

        [SerializeField] protected MonsterHealthData healthData;
        [SerializeField] protected float time = 1.0f;

        public int MaxHealth { get; set; } = 20;
        public int Health { get; set; } = 20;

        protected TextMeshProUGUI monsterText;
        protected UIBarImage barImage;

        private TicketMachine<IBaseEventPayload> ticketMachine;
        private readonly Queue<ImageChangeInfo> healthQueue = new Queue<ImageChangeInfo>();
        private int prevHealth;

        private void Awake()
        {
            Init();
            if (healthData)
                healthData.InitHealth();
            
            // ui
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine<IBaseEventPayload>>();
            ticketMachine.SetTicketType(ChannelType.Combat);
        }

        protected override void Init()
        {
            base.Init();

            Bind<GameObject>(typeof(GameObjects));
        }

        private void OnEnable()
        {
            SubscribeAction();
        }

        private void SubscribeAction()
        {
            if (healthData)
            {
                healthData.CurrentHealth.OnChange -= OnChangeHealth;
                healthData.CurrentHealth.OnChange += OnChangeHealth;
            }
        }

        public void SetName(string monsterName)
        {
            monsterText.text = monsterName;
        }

        protected virtual void Start()
        {
            var go = GetGameObject((int)GameObjects.MonsterPanel);

            var image = go.FindChild(NameBarImage, true);
            barImage = image.GetOrAddComponent<UIBarImage>();

            if (healthData)
            {
                prevHealth = healthData.CurrentHealth.Value;
                Health = MaxHealth = prevHealth;
            }
        }

        private void OnChangeHealth(int value)
        {
            if (prevHealth == value)
                return;

            float target = value / (float)MaxHealth;
            float t = time;

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
    }
}