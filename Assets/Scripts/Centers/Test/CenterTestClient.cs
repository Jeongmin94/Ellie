using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using UnityEngine;

namespace Centers.Test
{
    public class CenterTestClient : MonoBehaviour
    {
        [SerializeField] private int hp = 20;
        [SerializeField] private Transform billboardPosition;

        private TicketMachine<IBaseEventPayload> ticketMachine;

        private void Awake()
        {
            // ticket 설정
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine<IBaseEventPayload>>();
            ticketMachine.SetTicketType(ChannelType.Combat, ChannelType.UI);
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 20), "attack client"))
            {
                var payload = new UIPayload();
                payload.Type = UIType.BarImage;
                ticketMachine.SendMessage(ChannelType.UI, payload);
            }
        }
    }
}