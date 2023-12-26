using Assets.Scripts.Managers;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Puzzle
{
    public class StoneFootboardPuzzle : MonoBehaviour
    {
        private new Rigidbody rigidbody;
        private Vector3 initialPosition;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;

        public bool isPlayerTouched;
        private bool isRigid;

        private Coroutine returnToFirstHeightCoroutine;
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            initialPosition = transform.position;
            isRigid = false;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player") && !isPlayerTouched)
            {
                if (!isRigid)
                {
                    if(returnToFirstHeightCoroutine != null)
                        StopCoroutine(returnToFirstHeightCoroutine);
                    rigidbody.useGravity = true;
                    SoundManager.Instance.PlaySound(SoundManager.SoundType.UISfx, "puzzle2_stone1");
                    isPlayerTouched = true;
                }
            }
            if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isPlayerTouched = false;
                returnToFirstHeightCoroutine = StartCoroutine(OnPlayerExit());
            }
        }

        public void Freeze()
        {
            isRigid = true;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            SaveLoadManager.Instance.SaveData();
        }

        private IEnumerator OnPlayerExit()
        {
            rigidbody.useGravity = false;
            GetComponent<BoxCollider>().isTrigger = true;
            while (transform.position.y < initialPosition.y)
            {
                Vector3 pos = transform.position;
                pos.y += moveSpeed * Time.deltaTime;
                transform.position = pos;
                yield return null;
            }
            GetComponent<BoxCollider>().isTrigger = false;
        }
        private void FixedUpdate()
        {
            if(isPlayerTouched)
            {
                rigidbody.AddForce(-Vector3.up * 5f, ForceMode.Force);
            }

            if(transform.position.y > initialPosition.y)
            {
                transform.position = initialPosition;
                rigidbody.velocity = Vector3.zero;
            }
        }
    }
}
