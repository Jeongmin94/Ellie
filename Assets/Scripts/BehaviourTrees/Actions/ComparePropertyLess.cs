using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class ComparePropertyLess : ActionNode
{
    public NodeProperty nodeValue;
    public NodeProperty<float> compareValue;
    public NodeProperty<bool> isEqual;

    private float val;
    private float compare;

    protected override void OnStart()
    {
        compare = compareValue.Value;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        BlackboardKey<float> valFloat = nodeValue.reference as BlackboardKey<float>;

        if (valFloat != null)
        {
            val = valFloat.Value;
        }
        else
        {
            // BlackboardKey<float> 형변환이 실패 시 int값 사용
            BlackboardKey<int> valInt = nodeValue.reference as BlackboardKey<int>;
            if (valInt != null)
            {
                // 성공적으로 형변환 된 경우, float로 캐스팅하여 값을 사용합니다.
                val = (float)valInt.Value;
            }
            else
            {
                // 둘 다 형변환이 실패한 경우, Failure를 반환
                return State.Failure;
            }
        }

        if ((isEqual.Value == true && val <= compare) ||
           (isEqual.Value == false && val < compare))
        {
            return State.Success;
        }

        return State.Failure;
    }
}
