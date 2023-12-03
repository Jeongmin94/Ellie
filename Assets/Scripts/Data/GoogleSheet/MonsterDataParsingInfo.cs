using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{
    Darkness,
    Confuse,
    Normal,
    Earth,
    Fire,
    Water,
    Grass,
    Light,
}

[Serializable]
public class MonsterData
{
    public int index;
    public string name;
    public Element element;
    public int hp;
    public float movement;
    public float range;
    public float attackInterval;
    public List<Element> immuneList;
    public bool aggression;
    public float weakRatio;
    public int dropTable;
}

[CreateAssetMenu(fileName = "MonsterData", menuName = "GameData List/MonsterData")]
public class MonsterDataParsingInfo : DataParsingInfo
{
    public List<MonsterData> monsters;

    public override T GetIndexData<T>(int index) where T : class
    {
        if (typeof(T) == typeof(MonsterData))
        {
            return monsters.Find(m => m.index == index) as T;
        }
        return default(T);
    }

    public override void Parse()
    {
        monsters.Clear();

        string[] lines = tsv.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i]))
                continue;

            string[] entries = lines[i].Split('\t');

            MonsterData data = new MonsterData();
            data.immuneList = new List<Element>();

            try
            {
                // 인덱스 코드
                data.index = int.Parse(entries[0].Trim());

                // 몬스터 이름(Kor)
                data.name = entries[1].Trim();

                // 속성
                data.element = (Element)Enum.Parse(typeof(Element), entries[2].Trim());

                // 체력
                data.hp = int.Parse(entries[3].Trim());

                // 이동 속도
                data.movement = float.Parse(entries[4].Trim());

                // 공격 감지
                data.range = float.Parse(entries[5].Trim());

                // 공격 간격(속도)
                data.attackInterval = float.Parse(entries[6].Trim());

                // 상태이상 면역 - 리스트
                string[] immuneElements = entries[7].Split(',');
                foreach (var element in immuneElements)
                {
                    if (element.Trim() == "None") continue;
                    data.immuneList.Add((Element)Enum.Parse(typeof(Element), element.Trim()));
                }

                // 선공 유무
                data.aggression = bool.Parse(entries[8].Trim());

                // 약점 계수
                data.weakRatio = float.Parse(entries[9].Trim());

                // 드랍 테이블 ID
                data.dropTable = int.Parse(entries[10].Trim());
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error parsing line {i + 1}: {entries[i]}");
                Debug.LogError(ex);
                continue;
            }

            monsters.Add(data);
        }
    }
}
