using Centers;
using Managers;
using UnityEngine;

namespace Ending
{
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
}