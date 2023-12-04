using Centers;
using UnityEngine;

public class EndingCenter : BaseCenter
{
    public GameObject Canvas;
    public GameObject Client;

    private void Awake()
    {
        Init();
    }

    protected override void Start()
    {
        CheckTicket(Client.gameObject);
        CheckTicket(Canvas.gameObject);

        Client.GetComponent<VideoStartClient>().SendEndingPayload();
    }
}
