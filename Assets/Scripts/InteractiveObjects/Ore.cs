using Assets.Scripts.Player;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects
{
    public class Ore : MonoBehaviour
    {
        private bool minable = true;
        public int hardness;
        public int hp;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!minable) return;
            if(other.CompareTag("Player"))
            {
                other.gameObject.GetComponentInParent<PlayerController>().canStartMining = true;
                other.gameObject.GetComponentInParent<PlayerController>().SetCurOre(this);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (!minable) return;

            if (other.CompareTag("Player"))
            {
                other.gameObject.GetComponentInParent<PlayerController>().canStartMining = false;
                other.gameObject.GetComponentInParent<PlayerController>().SetCurOre(null);
            }
        }
        public void Smith(int damage)
        {
            hp -= damage;
            Debug.Log("Mine! cur hp : " +  hp.ToString());
            if(hp<=0)
            {
                Debug.Log("mining complete");

            }
        }
    }
}