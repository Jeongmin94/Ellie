using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Boss.Terrapupa
{
    [CreateAssetMenu(fileName = "TerrapupaData", menuName = "Boss/TerrapupaData")]
    public class TerrapupaDataInfo : ScriptableObject
    {
        [Header("테라푸파 속성")]
        [Tooltip("보스의 체력")] public int hp;
        [Tooltip("보스의 이동 속도")] public float movementSpeed;
        [Tooltip("보스의 회전 속도")] public float rotationSpeed;

        [Header("1. 돌 던지기 패턴")]
        [Tooltip("돌 던지기 패턴 감지 범위")] public float stoneDetectionDistance;
        [Tooltip("돌의 공격력")] public int stoneAttackValue;
        [Tooltip("돌의 속도")] public float stoneSpeed;
        [Tooltip("돌의 크기")] public float stoneScale;
    }
}