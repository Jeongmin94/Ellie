using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Boss.Terrapupa
{
    [CreateAssetMenu(fileName = "TerrapupaData", menuName = "Boss/TerrapupaData")]
    public class TerrapupaDataInfo : ScriptableObject
    {
        [Header("기본 수치")]
        [Tooltip("보스의 체력")] public int hp = 45;

        [Header("섭취")]
        [Tooltip("섭취 지속시간")] public float intakeDuration = 10.0f;
        [Tooltip("섭취 시 체력 회복량")] public int intakeHealValue = 10;

        [Header("기절")]
        [Tooltip("기절 지속시간")] public float stunDuration = 5.0f;

        [Header("유인")]
        [Tooltip("유인 이동속도")] public float temptMovementSpeed = 2.0f;
        [Tooltip("섭취상태로 변경 시 감지 범위")] public float temptStateChangeDetectionDistance = 1.0f;

        [Header("구르기 공격")]
        [Tooltip("패턴 사용 여부")] public bool rollUsable = true;
        [Tooltip("공격 감지 범위")] public float rollDetectionDistance = 25.0f;
        [Tooltip("구르기 종료 벽 인식 거리(Raycast 길이)")] public float rollRayCastLength = 7.0f;
        [Tooltip("구르기 이동속도")] public float rollMovementSpeed = 20.0f;
        [Tooltip("구르기 공격력")] public int rollAttackValue = 5;

        [Header("돌 던지기, 땅 뒤집기 공통")]
        [Tooltip("공격 감지 범위")] public float MidRangeDetectionDistance = 15.0f;

        [Header("돌 던지기 공격")]
        [Tooltip("패턴 사용 여부")] public bool stoneUsable = true;
        [Tooltip("타겟팅 회전 속도")] public float stoneRotationSpeed = 1.0f;
        [Tooltip("돌의 이동 속도")] public float stoneMovementSpeed = 7.0f;
        [Tooltip("돌의 크기")] public float stoneScale = 2.72f;
        [Tooltip("돌의 공격력")] public int stoneAttackValue = 5;

        [Header("땅 뒤집기 공격")]
        [Tooltip("패턴 사용 여부")] public bool earthQuakeUsable = true;
        [Tooltip("타겟팅 회전 속도")] public float earthQuakeRotationSpeed = 4.0f;
        [Tooltip("땅 뒤집기 시 이동 거리")] public float earthQuakeMoveDistance = 10.0f;
        [Tooltip("땅 뒤집기 시 이동 속도")] public float earthQuakeMovementSpeed = 8.0f;
        [Tooltip("땅 뒤집기 공격력")] public int earthQuakeAttackValue = 5;

        [Header("근접 공격")]
        [Tooltip("공격 감지 범위")] public float ShortRangeDetectionDistance = 5.0f;

        [Header("하단 공격")]
        [Tooltip("패턴 사용 여부")] public bool lowAttackQuakeUsable = true;
        [Tooltip("타겟팅 회전 속도")] public float lowAttackRotationSpeed = 4.0f;
        [Tooltip("하단 공격 공격력")] public int lowAttackAttackValue = 5;

        [Header("회전 공격")]
        [Tooltip("패턴 사용 여부")] public bool spinningAttackUsable = true;
        [Tooltip("플레이어에게 타겟팅 회전 속도")] public float spinningAttackRotationSpeed = 2.0f;
        [Tooltip("회전 공격 중 이동 속도")] public float spinningAttackMovementSpeed = 1.0f;
        [Tooltip("회전 공격 공격력")] public int spinningAttackAttackValue = 5;
    }
}