using System;
using Assets.Scripts.Combat;
using UnityEngine;

namespace Assets.Scripts.Boss1.TerrapupaMinion
{
    public class TerrapupaMinionWeakPoint : MonoBehaviour, ICombatant
    {
        private Action<IBaseEventPayload> collisionAction;

        public void Attack(IBaseEventPayload payload)
        {
        }

        public void ReceiveDamage(IBaseEventPayload payload)
        {
            // 플레이어 총알 -> Combat Channel -> TerrapupaMinionWeakPoint :: ReceiveDamage() -> TerrapupaMinionController
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