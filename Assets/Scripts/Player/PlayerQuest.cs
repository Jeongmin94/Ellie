using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerQuest : MonoBehaviour
    {
        PlayerController controller;
        // Use this for initialization
        void Start()
        {
            controller = GetComponent<PlayerController>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartConversation()
        {
            controller.StartConversation();
        }
        public void EndConversation()
        {
            controller.EndConversation();
        }
    }
}