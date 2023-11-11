using Channels.Boss;
using System.Collections;
using UnityEngine;

namespace Boss.Objects
{
    public class MagicStalactite : MonoBehaviour
    {
        public float respawnValue = 10.0f;

        private Rigidbody rb;
        private LineRenderer lineRenderer;

        private int myIndex;

        public int MyIndex
        {
            get { return myIndex; }
            set { myIndex = value; }
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.magenta;
        }

        private void Update()
        {
            RaycastHit hit;
            // 광선을 아래 방향으로 발사
            if (Physics.Raycast(transform.position, -Vector3.up, out hit))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, hit.point);
                }
                else
                {
                    lineRenderer.enabled = false;
                }
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Stone"))
            {
                Debug.Log(collision.transform.name);

                rb.useGravity = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Boss"))
            {
                Debug.Log($"{other} 충돌!");

                EventBus.Instance.Publish(EventBusEvents.DropMagicStalactite,
                    new BossEventPayload
                    {
                        IntValue = myIndex,
                        FloatValue = respawnValue,
                        TransformValue1 = transform,
                        TransformValue2 = other.transform.root
                    });

                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                gameObject.SetActive(false);
            }
            else if (other.transform.CompareTag("Ground"))
            {
                Debug.Log($"{other} 충돌!");

                EventBus.Instance.Publish(EventBusEvents.DropMagicStalactite,
                    new BossEventPayload
                    {
                        IntValue = myIndex,
                        FloatValue = respawnValue,
                        TransformValue1 = transform,
                    });

                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                gameObject.SetActive(false);
            }
        }
    }
}