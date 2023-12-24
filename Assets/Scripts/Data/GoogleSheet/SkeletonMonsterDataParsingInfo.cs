using System;
using System.Collections.Generic;
using Monsters.AbstractClass;
using Monsters.Utility;
using UnityEngine;

namespace Data.GoogleSheet
{
    public enum MonsterName
    {
        NormalSkeleton,
        AdventureSkeleton,
        GuildguardSkeleton,
        WizardSkeleton,
        CaveBat
    }

    [Serializable]
    public class SkeletonMonsterData
    {
        public int index;
        public string monsterName;

        public float maxHP;
        public float movementSpeed;
        public float returnSpeed;
        public float rotationSpeed;
        public float detectPlayerDistance;
        public float chasePlayerDistance;
        public float overtravelDistance;
        public float stopDistance;
        public float weakRatio;
        public List<int> itemDropTable;
        public int respawnTime;


        public Enums.MonsterElement element;
        public Enums.MovementType type;
        public Enums.AttackTurnType turnType;

        public Dictionary<string, AbstractAttack> skills;
    }


    [CreateAssetMenu(fileName = "SkeletonMonster", menuName = "GameData List/Monsters/SkeletonMonsterData",
        order = int.MaxValue)]
    public class SkeletonMonsterDataParsingInfo : DataParsingInfo
    {
        public List<SkeletonMonsterData> datas = new();

        public override T GetIndexData<T>(int index)
        {
            if (typeof(T) == typeof(SkeletonMonsterData))
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

                var data = new SkeletonMonsterData();
                data.itemDropTable = new List<int>();

                try
                {
                    data.index = int.Parse(entries[0].Trim());
                    data.monsterName = entries[1].Trim();
                    data.element = (Enums.MonsterElement)Enum.Parse(typeof(Enums.MonsterElement), entries[2].Trim());
                    data.maxHP = float.Parse(entries[3]);
                    data.movementSpeed = float.Parse(entries[4]);
                    data.rotationSpeed = float.Parse(entries[5]);
                    data.detectPlayerDistance = float.Parse(entries[6]);
                    data.chasePlayerDistance = float.Parse(entries[7]);
                    data.overtravelDistance = float.Parse(entries[8]);
                    data.returnSpeed = float.Parse(entries[9]);
                    data.stopDistance = float.Parse(entries[10]);
                    data.weakRatio = float.Parse(entries[15]);

                    var dropableItem = entries[16].Trim().Split(',');
                    for (var m = 0; m < dropableItem.Length; m++)
                    {
                        var index = int.Parse(dropableItem[m].Trim());
                        data.itemDropTable.Add(index);
                    }

                    data.respawnTime = int.Parse(entries[17]);
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