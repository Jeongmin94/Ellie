using Assets.Scripts.Managers;
using UnityEngine;

namespace Boss1.BossRoomObjects
{
    public class BossRoomSavePoint : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                SaveLoadManager.Instance.SaveData();
            }
        }
    }
}