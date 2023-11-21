using Assets.Scripts.Item;
using Assets.Scripts.StatusEffects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data.GoogleSheet
{
    [Serializable]
    public class StoneData : ItemMetaData
    {
        public int appearanceStage;
        public int tier;
        public Element element;
        public int damage;
        public StatusEffectName statusEffect;
        public float statusEffectDuration;
        public StatusEffectName debuff;
        public List<int> conditions = new();
        public string specialEffectName;
        public int specialEffectIndex;
        public int combineCost;
        public int sellCost;
        public string textureName;
    }

    [CreateAssetMenu(fileName = "StoneData", menuName = "GameData List/StoneData")]
    public class StoneDataParsingInfo : DataParsingInfo
    {
        public List<StoneData> stones;

        public override T GetIndexData<T>(int index)
        {
            if (typeof(T) == typeof(StoneData))
            {
                return stones.Find(m => m.index == index) as T;
            }

            return default(T);
        }
        public override void Parse()
        {
            stones.Clear();

            string[] lines = tsv.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i])) continue;
                string[] entries = lines[i].Split('\t');

                StoneData data = new();
                try
                {
                    data.groupType = UI.Inventory.GroupType.Stone;
                    //인덱스
                    data.index = int.Parse(entries[0].Trim());
                    //이름
                    data.name = entries[1].Trim();
                    //Description
                    data.description = entries[2].Trim();
                    //처음 등장하는 스테이지
                    data.appearanceStage = int.Parse(entries[3].Trim());
                    //티어
                    data.tier = int.Parse(entries[4].Trim());
                    //속성
                    data.element = (Element)Enum.Parse(typeof(Element), entries[5].Trim());
                    //데미지
                    data.damage = int.Parse(entries[6].Trim());
                    //유발하는 상태이상
                    data.statusEffect = (StatusEffectName)Enum.Parse(typeof(StatusEffectName), entries[7].Trim());
                    //상태이상의 지속시간
                    data.statusEffectDuration = float.Parse(entries[8].Trim());
                    //유발하는 디버프
                    data.debuff = (StatusEffectName)Enum.Parse(typeof(StatusEffectName), entries[9].Trim());
                    //특수 효과가 발생하는 조건의 list
                    string[] conditions = entries[10].Trim().Split(',');
                    if (entries[10] != "None")
                    {
                        foreach (string condition in conditions)
                        {
                            data.conditions.Add(int.Parse(condition.Trim()));
                        }
                    }
                    //특수 효과 이름
                    data.specialEffectName = entries[11].Trim();
                    //특수 효과의 인덱스
                    data.specialEffectIndex = int.Parse(entries[12].Trim());
                    //조합 비용
                    data.combineCost = int.Parse(entries[13].Trim());
                    //상인 판매 비용
                    data.sellCost = int.Parse(entries[14].Trim());
                    //이미지 이름 -> extern의 해당 폴더 찾아가서 가져오기
                    data.imageName = "UI/Item/Stone/" + entries[15].Trim();
                    //
                    data.textureName = entries[16].Trim();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error parsing line {i + 1}: {entries[i]}");
                    Debug.LogError(e);
                    continue;
                }
                stones.Add(data);
            }
        }
    }
}