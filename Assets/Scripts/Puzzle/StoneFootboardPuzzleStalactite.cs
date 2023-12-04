using Assets.Scripts.Managers;
using Assets.Scripts.Particle;
using Channels.Boss;
using Channels.Components;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Puzzle
{
    public class StoneFootboardPuzzleStalactite : MonoBehaviour
    {
        public GameObject hitEffect;
        public GameObject displayEffect;
        public Material material;

        private Rigidbody rb;
        private LineRenderer lineRenderer;
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
            InitLineRenderer();
        }

        private void OnEnable()
        {
            rb.isKinematic = true;
            SetLineRendererPosition();
            lineRenderer.enabled = true;
        }

        private void InitLineRenderer()
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.3f;
            lineRenderer.endWidth = 0.3f;
            lineRenderer.material = material;
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;
        }

        

        public void SetLineRendererPosition()
        {
            RaycastHit hit;
            int layerMask = LayerMask.GetMask("Ground");

            if (particle != null)
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
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "puzzle2_stone3", transform.position);
            }

            if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                ParticleManager.Instance.GetParticle(hitEffect, transform, 0.7f);
                gameObject.SetActive(false);
                SoundManager.Instance.PlaySound(SoundManager.SoundType.UISfx, "puzzle2_stone2");
            }
        }
    }
}