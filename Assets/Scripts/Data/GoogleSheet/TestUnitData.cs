using Assets.Scripts.Managers;
using UnityEngine;

public class TestUnitData : MonoBehaviour
{
    [SerializeField] private MonsterDataParsingInfo parsingInfo;
    [SerializeField] private MonsterData currentData;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            parsingInfo = DataManager.Instance.GetData<MonsterDataParsingInfo>();
            currentData = DataManager.Instance.GetIndexData<MonsterData, MonsterDataParsingInfo>(1000);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            parsingInfo = DataManager.Instance.GetData<MonsterDataParsingInfo>();
            currentData = DataManager.Instance.GetIndexData<MonsterData, MonsterDataParsingInfo>(1001);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            parsingInfo = DataManager.Instance.GetData<MonsterDataParsingInfo>();
            currentData = DataManager.Instance.GetIndexData<MonsterData, MonsterDataParsingInfo>(1002);
        }
    }
}
