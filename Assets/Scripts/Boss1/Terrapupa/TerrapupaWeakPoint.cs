using System;
using Combat;
using UnityEngine;

namespace Boss1.Terrapupa
{
    public class TerrapupaWeakPoint : MonoBehaviour, ICombatant
    {
        private Action<IBaseEventPayload> collisionAction;

        public void Attack(IBaseEventPayload payload)
        {
        }

        public void ReceiveDamage(IBaseEventPayload payload)
        {
            // 플레이어 총알 -> Combat Channel -> TerrapupaWeakPoint :: ReceiveDamage() -> TerrapupaController
            Debug.Log($"{name} ReceiveDamage :: 약점 충돌");
            collisionAction?.Invoke(payload);
        }

        public void SubscribeCollisionAction(Action<IBaseEventPayload> action)
        {
            collisionAction -= action;
            collisionAction += action;
        }
    }
}