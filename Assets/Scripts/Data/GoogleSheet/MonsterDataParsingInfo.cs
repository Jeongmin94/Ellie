using System;
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
        
    }
}
