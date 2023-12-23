using Assets.Scripts.Monsters.Attacks;
using UnityEngine;

namespace Assets.Scripts.Monsters.Others
{
    public class AOE : MonoBehaviour
    {
        public AOEPrefabAttack spawner;
        [SerializeField] private ParticleSystem[] particle;

        private float accumulatedTime;
        private bool isActivated;

        private void Start()
        {
            Destroy(gameObject, spawner.attackData.attackDuration);
            Invoke("ActivateAOE", 1.5f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isActivated)
            {
                return;
            }

            if (other.tag == "Player")
            {
                accumulatedTime = 0.0f;
                spawner.SetAndAttack(other.transform);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!isActivated)
            {
                return;
            }

            if (accumulatedTime > 1.0f)
            {
                if (other.tag == "Player")
                {
                    spawner.SetAndAttack(other.transform);
                    Debug.Log("++STAY IN JANGPAN");
                }

                accumulatedTime = 0.0f;
            }

            accumulatedTime += Time.deltaTime;
        }

        private void ActivateAOE()
        {
            isActivated = true;
            particle[0].Stop();
            particle[1].Play();
        }
    }
}