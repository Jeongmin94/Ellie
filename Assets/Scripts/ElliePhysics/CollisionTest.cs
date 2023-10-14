using UnityEngine;

namespace Assets.Scripts.ElliePhysics
{
    public class CollisionTest : MonoBehaviour
    {
        [SerializeField] string[] tags;

        private void OnCollisionEnter(Collision collision)
        {
            if (tags.Length == 0)
                return;

            for (int i = 0; i < tags.Length; i++)
            {
                if (collision.gameObject.CompareTag(tags[i]))
                {
                    Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                    if (rb)
                    {
                        //Debug.Log($"speed: {rb.velocity.magnitude}");
                        rb.velocity *= 0.8f;
                    }
                }
            }
        }
    }
}
