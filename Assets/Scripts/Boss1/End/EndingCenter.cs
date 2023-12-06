using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Singleton;
using Centers;
using UnityEngine;

public class EndingCenter : BaseCenter
{
    public GameObject Canvas;
    public GameObject Client;

    private void Awake()
    {
        MangerControllers.ClearAction(ManagerType.Input);
        MangerControllers.ClearAction(ManagerType.Sound);

        Init();
    }

    protected override void Start()
    {
        CheckTicket(Client.gameObject);
        CheckTicket(Canvas.gameObject);

        Client.GetComponent<VideoStartClient>().SendEndingPayload();
    }
}
