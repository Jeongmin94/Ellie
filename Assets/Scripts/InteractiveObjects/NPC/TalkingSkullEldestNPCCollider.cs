using System.Collections;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects.NPC
{
    public class TalkingSkullEldestNPCCollider : MonoBehaviour
    {
        TalkingSkullEldestNPC parentObj;

        private void Awake()
        {
            parentObj = GetComponentInParent<TalkingSkullEldestNPC>();
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Stone"))
                parentObj.HitOnStone();
        }
    }
}