using System;
using System.Collections.Generic;
using Channels.Combat;
using Data.GoogleSheet;
using Monsters.Utility;
using UnityEngine;

namespace Data.Monster
{
    [Serializable]
    public class MonsterAttackData
    {
        public int index;
        public string monsterName;
        public string attackName;
        public Enums.AttackSkill attackType;

        public int attackValue;
        public int attackInterval;
        public float attackableDistance;
        public float animationHold;
        public float attackDuration;
        public Vector3 offset;
        public Vector3 size;
        public int projectileChase;
        public float projectileSpeed;

        public int movementSpeed;

        public int fleeDistance;
        public float stopDistance;
        public int attackableMinimumDistance;

        public CombatType combatType;

        //table 추가
        public string weaponName;
        public string projectilePrefabPath;
    }

    [CreateAssetMenu(fileName = "MonsterAttackData", menuName = "GameData List/Monsters/AttackData", order = int.MaxValue)]
    public class MonsterAttackDataparsingInfo : DataParsingInfo
    {
        public List<MonsterAttackData> datas = new();

        public override T GetIndexData<T>(int index)
        {
            if (typeof(T) == typeof(MonsterAttackData))
            {
                return datas.Find(m => m.index == index) as T;
            }

            return default;
        }

        public override void Parse()
        {
            datas.Clear();

            var lines = tsv.Split('\n');

            for (var i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                {
                    continue;
                }

                var entries = lines[i].Split('\t');

                var data = new MonsterAttackData();

                try
                {
                    data.index = int.Parse(entries[0]);
                    data.monsterName = entries[1].Trim();
                    data.attackName = entries[2].Trim();
                    data.attackType = (Enums.AttackSkill)Enum.Parse(typeof(Enums.AttackSkill), entries[3].Trim());
                    data.attackValue = int.Parse(entries[4]);
                    data.attackInterval = int.Parse(entries[5]);
                    data.attackableDistance = float.Parse(entries[6]);
                    data.animationHold = float.Parse(entries[9]);
                    data.attackDuration = float.Parse(entries[10]);
                    data.offset.x = float.Parse(entries[11]);
                    data.offset.y = float.Parse(entries[12]);
                    data.offset.z = float.Parse(entries[13]);
                    data.size.x = float.Parse(entries[14]);
                    data.size.y = float.Parse(entries[15]);
                    data.size.z = float.Parse(entries[16]);
                    data.projectileSpeed = float.Parse(entries[17]);
                    data.projectilePrefabPath = entries[20].Trim();
                    data.projectileChase = int.Parse(entries[21]);
                    data.movementSpeed = int.Parse(entries[22]);
                    data.fleeDistance = int.Parse(entries[23]);
                    data.stopDistance = float.Parse(entries[25]);
                    data.attackableMinimumDistance = int.Parse(entries[26]);
                    data.combatType = (CombatType)Enum.Parse(typeof(CombatType), entries[27].Trim());
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error parsing line {i + 1}: {entries[i]}");
                    Debug.LogError(ex);
                    continue;
                }

                datas.Add(data);
            }
        }
    }
}