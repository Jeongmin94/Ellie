using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{
    Normal,
    Earth,
}

[System.Serializable]
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
public class MonsterDataParsing : DataParsing
{

    public List<MonsterData> monsters;
    
    public override void Parse()
    {
        
    }
}
