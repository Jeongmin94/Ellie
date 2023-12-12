using Assets.Scripts.Managers;
using Assets.Scripts.Particle;
using UnityEngine;

namespace Assets.Scripts.Puzzle
{
    public class StoneFootboardPuzzleStalactite : MonoBehaviour
    {
        [SerializeField] private float lineRenderStartWidth = 0.5f;
        [SerializeField] private float lineRenderEndWidth = 0.5f;
        [SerializeField] private GameObject hitEffect;
        [SerializeField] private GameObject displayEffect;
        [SerializeField] private Material material;
        [SerializeField] private string attackSound = "puzzle2_stone2";
        [SerializeField] private string hitSound = "puzzle2_stone3";

        private Rigidbody rb;
        private LineRenderer lineRenderer;
        private ParticleController particle;

        private bool isFallen = false;

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
            lineRenderer.startWidth = lineRenderStartWidth;
            lineRenderer.endWidth = lineRenderEndWidth;
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
                particle.Stop();
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
            if (!isFallen)
            {
                if (collision.transform.CompareTag("Stone"))
                {
                    SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, hitSound, transform.position);
                    
                    rb.useGravity = true;
                    rb.isKinematic = false;
                    
                    isFallen = true;
                    particle.Stop();
                    particle = null;
                } 
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isFallen)
            {
                if (other.transform.CompareTag("Ground"))
                {
                    Debug.Log($"{other} 충돌!");

                    var puzzle = other.GetComponent<StoneFootboardPuzzle>();
                    if(puzzle == null)
                    {
                        Debug.LogError($"{other} 퍼즐 에러");
                        return;
                    }
                    puzzle.Freeze();

                    SoundManager.Instance.PlaySound(SoundManager.SoundType.UISfx, attackSound);

                    ParticleManager.Instance.GetParticle(hitEffect, transform, 0.7f);

                    lineRenderer.enabled = false;
                    Destroy(this.gameObject);
                }
            }
        }
    }
}