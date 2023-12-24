using UnityEngine;

namespace InteractiveObjects.NPC
{
    public class TalkingSkullEldestNPCCollider : MonoBehaviour
    {
        private TalkingSkullEldestNPC parentObj;

        private void Awake()
        {
            parentObj = GetComponentInParent<TalkingSkullEldestNPC>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Stone"))
            {
                parentObj.HitOnStone();
            }
        }
    }
}