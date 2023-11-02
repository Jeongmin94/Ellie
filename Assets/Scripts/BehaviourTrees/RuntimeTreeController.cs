using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RuntimeTreeController : MonoBehaviour
{
    public List<SubTreeController> subTreeList = new List<SubTreeController>();

    public virtual void Init()
    {
        // 각자 초기화 해서 사용
    }
}
