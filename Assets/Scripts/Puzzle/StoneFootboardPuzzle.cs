using Assets.Scripts.Managers;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Puzzle
{
    public class StoneFootboardPuzzle : MonoBehaviour
    {
        private new Rigidbody rigidbody;
        private Vector3 InitialPosition;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;

        public bool isPlayerTouched;
        private bool isRigid;

        private Coroutine returnToFirstHeightCoroutine;
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            InitialPosition = transform.position;
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
                else
                {
                    //rigidbody.velocity = Vector3.zero;
                }
            }
            if(collision.gameObject.CompareTag("InteractionObject"))
            {
                //rigidbody.velocity = Vector3.zero;
                isRigid = true;
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                SaveLoadManager.Instance.SaveData();
            }

            if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isPlayerTouched = false;
                returnToFirstHeightCoroutine = StartCoroutine(OnPlayerExit());
            }
        }
        //private void OnCollisionExit(Collision collision)
        //{
        //    if (collision.gameObject.CompareTag("Player"))
                
        //}

        private IEnumerator OnPlayerExit()
        {
            rigidbody.useGravity = false;
            GetComponent<BoxCollider>().isTrigger = true;
            while (transform.position.y < InitialPosition.y)
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

            if(transform.position.y > InitialPosition.y)
            {
                transform.position = InitialPosition;
                rigidbody.velocity = Vector3.zero;
            }

        }

    }
}
