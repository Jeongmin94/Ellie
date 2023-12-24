using Assets.Scripts.Managers;
using Channels.Components;
using Channels.Type;
using UI.Monster;
using UnityEngine;
using Utils;

namespace Centers.Test
{
    public class TestCenterClient : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 20;
        [SerializeField] private int damage = 2;
        [SerializeField] private Transform billboardPosition;

        private readonly MonsterDataContainer dataContainer = new();
        private UIMonsterBillboard billboard;

        private TicketMachine ticketMachine;

        private void Awake()
        {
            SetTicketMachine();
            InitUI();
            InitData();
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 20), "attack client"))
            {
                dataContainer.CurrentHp.Value -= damage;
                //ticketMachine.SendMessage(ChannelType.Combat, new CombatPayload { Type = CombatType.Test, HP = dataContainer.CurrentHp.Value }); ;
            }

            if (GUI.Button(new Rect(10, 30, 100, 20), "heal client"))
            {
                dataContainer.CurrentHp.Value += damage;
            }
        }

        private void SetTicketMachine()
        {
            Debug.Log("TestCenterClient SetTicketMachine()");
            // ticket 설정
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();

            // 기본 Ticket 추가
            ticketMachine.AddTickets(ChannelType.Combat);
        }

        private void InitUI()
        {
            billboard = UIManager.Instance.MakeStatic<UIMonsterBillboard>(billboardPosition,
                UIManager.UIMonsterBillboard);
            billboard.scaleFactor = 0.003f;
            billboard.InitBillboard(billboardPosition);
        }

        private void InitData()
        {
            dataContainer.MaxHp = maxHealth;
            dataContainer.CurrentHp.Value = maxHealth;
            dataContainer.Name = "I'm monster";

            billboard.InitData(dataContainer);
        }
    }
}