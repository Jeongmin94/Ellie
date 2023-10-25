using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DropTableData
{
    public int index;
    public string name;
    public bool isSpecialDropTable;
    public List<(int, int)> stoneDropDataList;
    public (int, int, int) specialDropDataTuple;
}

[CreateAssetMenu(fileName = "DropTableData", menuName = "GameData List/DropTableData")]

public class DropTableDataParsingInfo : DataParsingInfo
{
    private const int NONE = -1;
    public List<DropTableData> dropTables;

    public override T GetIndexData<T>(int index)
    {
        if (typeof(T) == typeof(DropTableData))
        {
            return dropTables.Find(m => m.index == index) as T;
        }
        return default(T);
    }

    public override void Parse()
    {
        dropTables.Clear();

        string[] lines = tsv.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i])) continue;

            string[] entries = lines[i].Split('\t');

            DropTableData data = new DropTableData();
            data.stoneDropDataList = new List<(int, int)>();
            //특별드롭인지 아닌지 판단 ->
            data.isSpecialDropTable = true;
            try
            {
                //인덱스
                data.index = int.Parse(entries[0].Trim());
                //이름
                data.name = entries[1].Trim();
                //드롭테이블 데이터(돌맹이 티어, 개수)
                for (int j = 0; j < 2; j++)
                {
                    int tier = int.Parse(entries[2 + 2 * j].Trim());
                    int num = int.Parse(entries[3 + 2 * j].Trim());
                    if (tier == NONE || num == NONE)
                        break;

                    data.stoneDropDataList.Add((tier, num));
                }
                //특별 드롭 데이터(돌맹이 인덱스, 개수, 확률)
                int stoneIdx = int.Parse(entries[6].Trim());
                int stoneNum = int.Parse(entries[7].Trim());
                int chance = int.Parse(entries[8].Trim());
                if (stoneIdx == NONE || stoneNum == NONE || chance == NONE)
                {
                    data.isSpecialDropTable = false;
                }
                data.specialDropDataTuple = (stoneIdx, stoneNum, chance);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing line {i + 1}: {entries[i]}");
                Debug.LogError(e);
                continue;
            }
            dropTables.Add(data);
        }
    }
}
