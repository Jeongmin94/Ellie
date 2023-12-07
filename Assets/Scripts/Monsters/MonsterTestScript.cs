using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Centers;
using UnityEngine;

public class MonsterTestScript : MonoBehaviour
{
    void Start()
    {
        SceneLoadManager.Instance.FinishLoading();
    }
}
