using System.Collections;
using System.Collections.Generic;
using Channels.Combat;
using UnityEngine;

[CreateAssetMenu(fileName = "FleeSkill", menuName = "GameData List/Monsters/FleeSkillData", order = int.MaxValue)]
public class FleeSkillData : ScriptableObject
{
    public string attackName;
    public CombatType combatType;
    public float activatableDistance;
    public float activateInterval;
    public float fleeDistance;
    public float fleeSpeed;
}
