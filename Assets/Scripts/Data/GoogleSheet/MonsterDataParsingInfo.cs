using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{
    Normal,
    Earth,
}

[Serializable]
public class MonsterData
{
    public int Index;
    public string Name;
    public Element Element;
    public int HP;
    public float Movement;
    public float Range;
    public float AttackInterval;
    public List<Element> Immune;
    public bool Aggressive;
    public float WeakRatio;
}

[CreateAssetMenu(fileName = "MonsterData", menuName = "GameData List/MonsterData")]
public class MonsterDataParsingInfo : DataParsingInfo
{

    public List<MonsterData> monsters;

    public override void Parse()
    {
        // Split the tsv data into lines
        string[] lines = tsv.Split('\n');

        // Starting from 1 to skip the header line
        for (int i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i]))
                continue; // skip empty lines

            string[] entries = lines[i].Split('\t');

            MonsterData data = new MonsterData();

            // Assuming the tsv format matches the given example
            try
            {
                data.Index = int.Parse(entries[0].Trim());
                data.Name = entries[1].Trim();
                data.Element = (Element)Enum.Parse(typeof(Element), entries[2].Trim());
                data.HP = int.Parse(entries[3].Trim());
                data.Movement = float.Parse(entries[4].Trim());
                data.Range = float.Parse(entries[5].Trim());
                data.AttackInterval = float.Parse(entries[6].Trim());

                // Splitting Immune elements by comma and then parsing
                string[] immuneElements = entries[7].Split(',');
                foreach (var element in immuneElements)
                {
                    if (element.Trim() == "None") continue;
                    data.Immune.Add((Element)Enum.Parse(typeof(Element), element.Trim()));
                }

                data.Aggressive = bool.Parse(entries[8].Trim());
                data.WeakRatio = float.Parse(entries[9].Trim());
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error parsing line {i + 1}: {entries[i]}");
                Debug.LogError(ex);
                continue;
            }

            Debug.Log(data.ToString());
            monsters.Add(data);
        }
    }
}
