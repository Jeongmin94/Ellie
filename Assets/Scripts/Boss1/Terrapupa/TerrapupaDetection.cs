using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrapupaDetection : MonoBehaviour
{
    [SerializeField] private Transform myTerrapupa;

    private void Start()
    {
        if(myTerrapupa == null)
        {
            Debug.LogError($"{transform} 테라푸파 트랜스폼 정보가 없습니다");
        }
    }

    public Transform MyTerrapupa 
    { 
        get { return myTerrapupa; }
    }
}
