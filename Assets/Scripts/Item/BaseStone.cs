using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public class BaseStone : MonoBehaviour
    {
        [SerializeField] private Transform stoneBody;

        private new Rigidbody rigidbody;
        public Rigidbody StoneRigidBody { get { return rigidbody; } }

        private void Awake()
        {
            rigidbody = stoneBody.GetComponent<Rigidbody>();
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
            rigidbody.position = position;

            transform.rotation = Quaternion.identity;
            rigidbody.rotation = Quaternion.identity;
        }

        public void MoveStone(Vector3 direction, float strength, float referenceSpeed, float validDistance)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.isKinematic = false;
            rigidbody.freezeRotation = false;

            rigidbody.velocity = direction * strength;

            StartCoroutine(SlowDown(referenceSpeed, validDistance));
        }

        /// <summary>
        /// Stone의 이동량이 validDistance에 가까워질 수록 speed를 referenceSpeed까지 낮춤
        /// </summary>
        /// <param name="referenceSpeed"></param>
        /// <param name="validDistance"></param>
        /// <returns></returns>
        private IEnumerator SlowDown(float referenceSpeed, float validDistance)
        {
            Vector3 prevPosition = rigidbody.position;
            Vector3 startVelocity = rigidbody.velocity;
            float startSpeed = rigidbody.velocity.magnitude;
            float distanceAcc = 0.0f;

            while (distanceAcc < validDistance)
            {
                yield return new WaitForFixedUpdate();

                Vector3 prevToCurrent = (rigidbody.position - prevPosition);
                distanceAcc += prevToCurrent.magnitude;
                prevPosition = rigidbody.position;

                //Debug.Log($"acc: {distanceAcc}");
                //float speed = Mathf.Lerp(startSpeed, referenceSpeed, distanceAcc / validDistance);
                //Debug.Log($"speed: {speed}");

                if (rigidbody.velocity.magnitude < referenceSpeed)
                    break;
            }

            Debug.Log($"유효거리: {validDistance}, 이동거리: {distanceAcc}, 시작속력: {startSpeed}, 최종속력: {rigidbody.velocity.magnitude}, 기준속력: {referenceSpeed}");

            while (rigidbody.velocity.magnitude > referenceSpeed)
            {
                yield return new WaitForFixedUpdate();
                rigidbody.velocity *= 0.99f;
            }

        }
    }
}
