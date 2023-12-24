using System.Collections;
using Assets.Scripts.Managers;
using Channels.Type;
using Channels.UI;
using Data.GoogleSheet._4000Stone;
using Item.Stone;
using Player;
using UI.Inventory.CategoryPanel;
using UI.Inventory.Slot;
using UnityEngine;

namespace Item
{
    public class BaseStone : Poolable, ILootable
    {
        public StoneData data;

        public StoneHatchery hatchery { get; set; }


        public Rigidbody StoneRigidBody { get; private set; }

        private void Awake()
        {
            StoneRigidBody = gameObject.GetComponent<Rigidbody>();
        }

        private void OnDisable()
        {
            data = null;
        }

        public void Visit(PlayerLooting player)
        {
            Debug.Log("Player Loot : " + name);
            // !TODO : PlayerLooting에서 Inventory에 AddItem 메시지를 UIChannel에 발송해야됨
            player.gameObject.GetComponentInParent<PlayerController>().TicketMachine
                .SendMessage(ChannelType.UI, GenerateStoneAcquirePayload());
            hatchery.CollectStone(this);
        }

        public void SetPosition(Vector3 position)
        {
            StoneRigidBody.position = position;
            StoneRigidBody.rotation = Quaternion.identity;
            StartCoroutine(EnableStoneTrail());
        }

        private IEnumerator EnableStoneTrail()
        {
            yield return new WaitForSeconds(0.02f);
            if (transform.childCount > 0)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        public void MoveStone(Vector3 direction, float strength)
        {
            StoneRigidBody.velocity = Vector3.zero;
            StoneRigidBody.angularVelocity = Vector3.zero;
            StoneRigidBody.isKinematic = false;
            StoneRigidBody.freezeRotation = false;

            StoneRigidBody.velocity = direction * strength;
        }

        private UIPayload GenerateStoneAcquirePayload()
        {
            var payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.AddSlotItem;
            payload.slotAreaType = SlotAreaType.Item;
            payload.groupType = GroupType.Stone;
            payload.itemData = data;
            return payload;
        }
    }
}