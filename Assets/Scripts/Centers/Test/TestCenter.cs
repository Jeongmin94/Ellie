using UnityEngine;

namespace Centers.Test
{
    public class TestCenter : BaseCenter
    {
        public TestCenterClient client;

        private void Awake()
        {
            Init();

            Debug.Log($"TestCenter Awake");
        }

        protected override void Start()
        {
            CheckTicket(client.gameObject);
        }
    }
}