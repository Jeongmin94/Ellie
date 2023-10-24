using Assets.Scripts.Player;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects
{
    public class Ore : MonoBehaviour, IInteractiveObject
    {
        public int hardness;
        public int hp;

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
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

        public void Interact(GameObject obj)
        {
            obj.GetComponent<PlayerController>().SetCurOre(this);
        }
    }
}