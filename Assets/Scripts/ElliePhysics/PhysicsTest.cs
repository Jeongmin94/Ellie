using System.Collections;
using UnityEngine;

namespace Assets.Scripts.ElliePhysics
{
    public enum GravityMode
    {
        Imbedded, Custom,
    }

    public class PhysicsTest : MonoBehaviour
    {
        public GravityMode gravityMode = GravityMode.Imbedded;

        [Header("Physics Configurations")]
        public Vector3 initVelocity;
        public Vector3 acceleration;
        public float time;
        public float validDistance;
        public float finalSpeed;
        public int trajectoryLinePoints = 25;

        [Header("Input Configurations")]
        public float jumpForce = 10.0f;
        public Transform releaseTransform;

        private new Rigidbody rigidbody;
        private float totalDistance = 0.0f;


        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.position = releaseTransform.position;
        }

        Vector3 startPosition;

        private void Start()
        {
            startPosition = rigidbody.position;
            rigidbody.AddForce(acceleration, ForceMode.Acceleration);
        }

        float t = 0.0f;
        private void Update()
        {
            if (Time.time > t)
            {
                // Calculate the expected distance traveled under uniform acceleration
                // using the equation d = 0.5 * a * t^2
                float expectedDistance = 0.5f * acceleration.magnitude * Mathf.Pow(time, 2);

                // Calculate the actual distance traveled
                float actualDistance = Vector3.Distance(startPosition, transform.position);

                // Output the expected and actual distances
                Debug.Log("Expected Distance: " + expectedDistance);
                Debug.Log("Actual Distance: " + actualDistance);
            }
        }

        bool isCalculating = false;
        Vector3 initVel;
        Vector3 accel;
        float calcTime = 0.0f;

        private void FixedUpdate()
        {
            if (isCalculating)
            {
                if (calcTime < time)
                {
                    calcTime += Time.fixedDeltaTime;
                }
            }
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 200, 20), "이동 테스트"))
            {
                Vector3 expected = initVelocity * time + 0.5f * time * time * acceleration;
                Debug.Log($"예상 이동거리: {expected.magnitude}");

                isCalculating = true;
                StartCoroutine(CalculateDistance(initVelocity, acceleration));
            }

            if (GUI.Button(new Rect(10, 30, 200, 20), "시작 속도 측정 테스트"))
            {
                Vector3 vf = finalSpeed * initVelocity.normalized;
                Vector3 v0 = vf - (acceleration / rigidbody.mass) * time;

                Debug.Log($"목표 거리: {validDistance}, 목표 속도: {finalSpeed}");
                Debug.Log($"예상 시작 속도(magnitude): {v0.magnitude}, {v0}");
                Debug.Log($"test");
                StartCoroutine(CalculateDistance(v0, Vector3.zero));
            }
        }

        // time 시간 동안 물체가 이동하는 거리 계산

        private IEnumerator CalculateDistance(Vector3 init, Vector3 accel)
        {
            //rigidbody.isKinematic = false;
            rigidbody.position = releaseTransform.position;

            float maxTime = time;
            float currentTime = 0.0f;
            Vector3 lastPosition = rigidbody.position;
            totalDistance = 0.0f;

            rigidbody.velocity = init;

            while (currentTime < maxTime)
            {
                yield return new WaitForFixedUpdate();
                currentTime += Time.fixedDeltaTime;
                rigidbody.AddForce(accel, ForceMode.Acceleration);

                float dist = Vector3.Distance(rigidbody.position, lastPosition);
                totalDistance += dist;
                lastPosition = rigidbody.position;
            }

            //rigidbody.isKinematic = true;

            Debug.Log($"실제 이동거리: {totalDistance}, time: {currentTime}");
        }
    }
}
