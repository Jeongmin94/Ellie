using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Boss.Objects
{
    public class FountainOfMana : MonoBehaviour
    {
        public float cooldownValue = 3.0f;
        public float respawnValue = 3.0f;

        [SerializeField] private bool isCooldown;
        [SerializeField] private bool isBroken;

        public bool IsCooldown
        {
            get { return isCooldown; }
            private set { isCooldown = value; }
        }

        public bool IsBroken
        {
            get { return isBroken; }
            private set { isBroken = value; }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!isBroken)
            {
                if (other.transform.CompareTag("Stone") && !isCooldown)
                {
                    Debug.Log("돌과 충돌");

                    isCooldown = true;
                    StartCoroutine(StartCooldown());
                }
                else if (other.transform.CompareTag("Player"))
                {
                    Debug.Log("보스와 충돌");

                    isBroken = true;
                    StartCoroutine(StartRespawn());
                }
            }
        }

        private IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(cooldownValue);

            isCooldown = false;
            Debug.Log($"{name} 쿨타임 완료");
        }

        private IEnumerator StartRespawn()
        {
            yield return new WaitForSeconds(respawnValue);

            isBroken = false;
            Debug.Log($"{name} 리스폰 완료");
        }
    }
}
