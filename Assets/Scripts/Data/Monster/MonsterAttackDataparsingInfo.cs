using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Monsters.Utility;
using Channels.Combat;
using UnityEngine;
using static Assets.Scripts.Monsters.Utility.Enums;

[Serializable]
public class MonsterAttackData
{
    public int index;
    public string monsterName;
    public string attackName;
    public AttackSkill attackType;

    public int attackValue;
    public int attackInterval;
    public int attackableDistance;
    public float animationHold;
    public float attackDuration;
    public Vector3 offset;
    public Vector3 size;

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
        return default(T);
    }

    public override void Parse()
    {
        datas.Clear();

        string[] lines = tsv.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i])) continue;
            string[] entries = lines[i].Split('\t');

            MonsterAttackData data = new MonsterAttackData();

            try
            {
                data.index = int.Parse(entries[0]);
                data.monsterName = entries[1].Trim();
                data.attackName = entries[2].Trim();
                data.attackType = (AttackSkill)Enum.Parse(typeof(AttackSkill), entries[3].Trim());
                data.attackValue = int.Parse(entries[4]);
                data.attackInterval = int.Parse(entries[5]);
                data.attackableDistance = int.Parse(entries[6]);
                data.animationHold = float.Parse(entries[9]);
                data.attackDuration = float.Parse(entries[10]);
                data.offset.x = float.Parse(entries[11]);
                data.offset.y = float.Parse(entries[12]);
                data.offset.z = float.Parse(entries[13]);
                data.size.x = float.Parse(entries[14]);
                data.size.y = float.Parse(entries[15]);
                data.size.z = float.Parse(entries[16]);
                data.projectilePrefabPath = entries[20].Trim();
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
