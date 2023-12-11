using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Item.Stone;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using Channels.Type;
using Channels.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public class BaseStone : Poolable, ILootable
    {
        public StoneData data = null;
        
        public StoneHatchery hatchery { get; set; }

        private new Rigidbody rigidbody;
        

        public Rigidbody StoneRigidBody
        {
            get { return rigidbody; }
        }

        private void Awake()
        {
            rigidbody = gameObject.GetComponent<Rigidbody>();
        }
        private void OnDisable()
        {
            data = null;
        }
        public void SetPosition(Vector3 position)
        {
            rigidbody.position = position;
            rigidbody.rotation = Quaternion.identity;
            StartCoroutine(EnableStoneTrail());
        }

        private IEnumerator EnableStoneTrail()
        {
            yield return new WaitForSeconds(0.02f);
            if(transform.childCount > 0)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        public void MoveStone(Vector3 direction, float strength)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.isKinematic = false;
            rigidbody.freezeRotation = false;

            rigidbody.velocity = direction * strength;
        }
        public void Visit(PlayerLooting player)
        {
            Debug.Log("Player Loot : " + this.name);
            // !TODO : PlayerLooting에서 Inventory에 AddItem 메시지를 UIChannel에 발송해야됨
            player.gameObject.GetComponentInParent<PlayerController>().TicketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayload());
            hatchery.CollectStone(this);
        }

        private UIPayload GenerateStoneAcquirePayload()
        {
            UIPayload payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.AddSlotItem;
            payload.slotAreaType = UI.Inventory.SlotAreaType.Item;
            payload.groupType = UI.Inventory.GroupType.Stone;
            payload.itemData = data;
            return payload;
        }

    }
}
