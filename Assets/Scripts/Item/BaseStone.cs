using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public class BaseStone : MonoBehaviour
    {
        private new Rigidbody rigidbody;

        public Rigidbody StoneRigidBody
        {
            get { return rigidbody; }
        }

        private void Awake()
        {
            rigidbody = gameObject.GetComponent<Rigidbody>();
        }

        public void SetPosition(Vector3 position)
        {
            rigidbody.position = position;
            rigidbody.rotation = Quaternion.identity;
        }

        public void MoveStone(Vector3 direction, float strength)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.isKinematic = false;
            rigidbody.freezeRotation = false;

            rigidbody.velocity = direction * strength;
        }

        // referenceTime이 지난 뒤에
        // stone의 비거리가 validDistance 이상
        // stone의 속도가 referenceSpeed 이상
        public void ValidateStoneMovement(float referenceTime, float referenceSpeed, float validDistance)
        {
            StartCoroutine(Validate(referenceTime, referenceSpeed, validDistance));
        }

        private IEnumerator Validate(float referenceTime, float referenceSpeed, float validDistance)
        {
            float currentTime = 0.0f;
            Vector3 startPosition = rigidbody.position;

            while (currentTime <= referenceTime)
            {
                yield return new WaitForFixedUpdate();
                currentTime += Time.fixedDeltaTime;
            }

            Vector3 currentPosition = rigidbody.position;
            Vector3 currentVelocity = rigidbody.velocity;

            Debug.Log($"{currentTime}초 뒤의 이동거리: {(currentPosition - startPosition).magnitude}");
            Debug.Log($"{currentTime}초 뒤의 속도: {currentVelocity.magnitude}");

            // var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // go.transform.position = currentPosition;
            // go.GetComponent<Collider>().enabled = false;

            currentPosition.y = 0.0f;
            startPosition.y = 0.0f;
            float d = Vector3.Distance(startPosition, currentPosition);
            Debug.Log($"시작: {startPosition}, 도착: {currentPosition}");
            Debug.Log($"비거리: {d}");
        }
    }
}