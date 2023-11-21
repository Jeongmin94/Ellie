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

        private bool isPlayerTouching;
        private bool isRigid;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            InitialPosition = transform.position;
            isRigid = false;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
                isPlayerTouching = true;
            if(collision.gameObject.CompareTag("InteractionObject"))
                isRigid = true;
        }
        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
                isPlayerTouching = false;
        }

        private void Update()
        {
            if (isRigid)
            {
                rigidbody.velocity = Vector3.zero;  
                transform.position = InitialPosition;
                return;
            }
            Vector3 pos = transform.position;
            if (!isPlayerTouching)
            {
                rigidbody.useGravity = false;
                if(pos.y<InitialPosition.y)
                {
                    pos.y += moveSpeed * Time.deltaTime;
                    transform.position = pos;
                }
            }
            else
            {
                rigidbody.useGravity = true;
            }
        }
        private IEnumerator ResetPos()
        {
            Vector3 pos = transform.position;
            while (pos.y < InitialPosition.y)
            {
                Debug.Log("Exit");
                
                yield return null;
            }
            transform.position = InitialPosition;
        }
    }
}
