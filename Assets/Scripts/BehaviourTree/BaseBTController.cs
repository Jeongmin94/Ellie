using System.Collections;
using UnityEngine;

public class BaseBTData : ScriptableObject
{
    [Header("구르기 공격")]
    [Tooltip("패턴 사용 여부")] public bool rollUsable = true;
    [Tooltip("공격 감지 범위")] public float rollDetectionDistance = 25.0f;
    [Tooltip("구르기 종료 벽 인식 거리(Raycast 길이)")] public float rollRayCastLength = 7.0f;
    [Tooltip("구르기 이동속도")] public float rollMovementSpeed = 20.0f;
    [Tooltip("구르기 공격력")] public int rollAttackValue = 5;
}

public class BaseBTController : MonoBehaviour
{
    public BaseBTData data;

    public virtual void Init()
    {
        // 각자 초기화 해서 사용
    }
}