
using UnityEngine;
using System.Collections;

using Assets.Scripts.Data;

namespace Assets.Scripts.Monsters.Characters
{

	public class MonsterController : MonoBehaviour
	{
        public SkeletonMeleeMonsterData monsterData;

        private void Awake()
        {
            monsterData = new()
            {
                HP = 10.0f,
                movementSpeed = 5.0f,
                rotationSpeed = 180.0f,
                detectPlayerDistance = 10.0f,
                chasePlayerDistance = 8.0f,
                overtravelDistance = 15.0f,
            };
        }
    }
}

