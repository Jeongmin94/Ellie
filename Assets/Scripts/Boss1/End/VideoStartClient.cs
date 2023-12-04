using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using UnityEngine;

public class VideoStartClient : MonoBehaviour
{
    private TicketMachine ticketMachine;

    private void Awake()
    {
        ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
        ticketMachine.AddTickets(ChannelType.UI);
    }

    public void SendEndingPayload()
    {
        // ������ ��� ���̷ε�
        UIPayload payload = UIPayload.Notify();
        payload.actionType = ActionType.PlayVideo;
        ticketMachine.SendMessage(ChannelType.UI, payload);
    }
}