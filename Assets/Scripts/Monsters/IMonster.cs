using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Monster
{
    public interface IMonster
    {
        public void MonsterDead(IBaseEventPayload payload);
    }
}