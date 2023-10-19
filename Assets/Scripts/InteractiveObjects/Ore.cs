using Assets.Scripts.Player;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects
{
    public class Ore : MonoBehaviour
    {
        public int hardness;
        public int hp;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                other.gameObject.GetComponentInParent<PlayerController>().canStartMining = true;
                other.gameObject.GetComponentInParent<PlayerController>().SetCurOre(this);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.gameObject.GetComponentInParent<PlayerController>().canStartMining = false;
                other.gameObject.GetComponentInParent<PlayerController>().SetCurOre(null);
            }
        }
    }
}