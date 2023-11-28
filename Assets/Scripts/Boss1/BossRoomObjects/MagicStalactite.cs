using Assets.Scripts.Managers;
using Assets.Scripts.Particle;
using Channels.Boss;
using Channels.Components;
using UnityEngine;

namespace Boss.Objects
{
    public class MagicStalactite : MonoBehaviour
    {
        public float respawnValue = 10.0f;
        public GameObject hitEffect;
        public GameObject displayEffect;
        public Material material;

        private Rigidbody rb;
        private LineRenderer lineRenderer;
        private TicketMachine ticketMachine;
        private ParticleController particle;

        private int myIndex;
        private bool isFallen = false;

        public int MyIndex
        {
            get { return myIndex; }
            set { myIndex = value; }
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.5f;
            lineRenderer.endWidth = 0.5f;
            lineRenderer.material = material;
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;
        }

        private void OnEnable()
        {
            rb.isKinematic = true;
            SetLineRendererPosition();
            lineRenderer.enabled = true;
        }

        public void InitTicketMachine(TicketMachine ticketMachine)
        {
            this.ticketMachine = ticketMachine;
        }

        public void SetLineRendererPosition()
        {
            RaycastHit hit;
            int layerMask = LayerMask.GetMask("Ground");

            if(particle != null)
            {
                PoolManager.Instance.Push(particle);
            }

            if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, layerMask))
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hit.point);

                particle = ParticleManager.Instance.GetParticle(displayEffect, new ParticlePayload
                {
                    Position = hit.point + new Vector3(0.0f, 0.1f, 0.0f),
                    Scale = new Vector3(1.0f, 1.0f, 1.0f),
                    IsLoop = true,
                }).GetComponent<ParticleController>();
            }
            else
            {
                lineRenderer.enabled = true;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Stone"))
            {
                Debug.Log(collision.transform.name);

                rb.useGravity = true;
                rb.isKinematic = false;
                isFallen = true;
                particle.Stop();
                particle = null;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isFallen)
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
                            TransformValue2 = other.transform.root,
                            Sender = other.transform.root,
                        });

                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;
                    isFallen = false;
                    lineRenderer.enabled = false;
                    gameObject.SetActive(false);
                }
                else if (other.transform.CompareTag("Ground") || other.transform.CompareTag("InteractionObject"))
                {
                    Debug.Log($"{other} 충돌!");

                    EventBus.Instance.Publish(EventBusEvents.DropMagicStalactite,
                        new BossEventPayload
                        {
                            IntValue = myIndex,
                            FloatValue = respawnValue,
                            TransformValue1 = transform,
                        });

                    ParticleManager.Instance.GetParticle(hitEffect, transform, 0.7f);

                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;
                    isFallen = false;
                    lineRenderer.enabled = false;
                    gameObject.SetActive(false);
                } 
            }
        }
    }
}