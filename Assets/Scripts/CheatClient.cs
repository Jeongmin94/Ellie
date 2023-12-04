using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Inventory;
using Boss.Terrapupa;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheatClient : SerializedMonoBehaviour
{
    public Transform player;
    public Transform terrapupa;
    public Transform terra;
    public Transform pupa;

    private bool IsRuntime
    {
        get { return Application.isPlaying; }
    }
    private bool IsParsingDone
    {
        get { return DataManager.Instance.isParseDone; }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            FindObject();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetPlayerPosition1();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetPlayerPosition2();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetPlayerPosition3();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SaveLoadManager.Instance.SaveData();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SaveLoadManager.Instance.LoadData();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            KillTerrapupa();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            KillTerraAndPupa();
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            DamageTerrapupa();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {

        }
    }

    [Button("참조 목록 갱신", ButtonSizes.Large)]
    private void FindObject()
    {
        player = FindObject("Player");
        terrapupa = FindObject("Terrapupa");
        terra = FindObject("Terra");
        pupa = FindObject("Pupa");
    }

    private Transform FindObject(string objName)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var obj in allObjects)
        {
            if (obj.name == objName && !EditorUtility.IsPersistent(obj))
            {
                return obj.transform;
            }
        }

        return null;
    }

    private void Awake()
    {
        FindObject();
    }

    /// <summary>
    /// 사용자 지정 이동
    /// </summary>

    [Title("사용자 지정 텔레포트")]
    [ValueDropdown("savePositionList")]
    public Vector3 savePosition = Vector3.zero;
    private List<Vector3> savePositionList = new List<Vector3>();

    [Button("플레이어 포지션 저장", ButtonSizes.Large)]
    public void SavePosition()
    {
        savePositionList.Add(player.position);
    }

    [Button("플레이어 저장 위치 리스트 리셋", ButtonSizes.Large)]
    public void ResetPositionList()
    {
        savePositionList.Clear();
        savePosition = Vector3.zero;
    }

    [Button("선택한 저장위치로 플레이어 위치 이동", ButtonSizes.Large)]
    public void SetPlayerPositionToSavedPosition()
    {
        player.position = savePosition;
    }

    /// <summary>
    /// 고정 위치 이동
    /// </summary>

    [Title("지정 장소 이동")]
    [Button("시작 지점 이동", ButtonSizes.Large)]
    public void SetPlayerPosition1()
    {
        player.position = new Vector3(-5.44f, 7.03f, -10.4f);
    }

    [Button("보스 방 앞 이동", ButtonSizes.Large)]
    public void SetPlayerPosition2()
    {
        player.position = new Vector3(-152f, 13.92f, 645.51f);
    }

    [Button("돌 발판 퍼즐 이동", ButtonSizes.Large)]
    public void SetPlayerPosition3()
    {
        player.position = new Vector3(33.5f, 11.8f, 98.8f);
    }

    /// <summary>
    /// 보스 치트키
    /// </summary>

    [Title("보스 치트키")]
    [EnableIf("IsRuntime")]
    [Button("1페이즈 스킵", ButtonSizes.Large)]
    public void KillTerrapupa()
    {
        Debug.Log("테라푸파 사망 치트");
        terrapupa.GetComponent<TerrapupaBTController>().terrapupaData.currentHP.Value = 0;
    }
    [EnableIf("IsRuntime")]
    [Button("2페이즈 스킵", ButtonSizes.Large)]
    public void KillTerraAndPupa()
    {
        Debug.Log("테라, 푸파 사망 치트");

        terra.GetComponent<TerrapupaBTController>().terrapupaData.currentHP.Value = 0;
        pupa.GetComponent<TerrapupaBTController>().terrapupaData.currentHP.Value = 0;
    }
    [EnableIf("IsRuntime")]
    [Button("2페이즈 스킵", ButtonSizes.Large)]
    public void DamageTerrapupa()
    {
        Debug.Log("테라, 푸파 데미지 입히기");

        terrapupa.GetComponent<TerrapupaBTController>().GetDamaged(1);
        terra.GetComponent<TerrapupaBTController>().GetDamaged(1);
        pupa.GetComponent<TerrapupaBTController>().GetDamaged(1);
    }

    /// <summary>
    /// 아이템 치트키
    /// </summary>
    /// 
    [Title("아이템 획득")]
    [EnableIf("IsRuntime")]
    [EnableIf("IsParsingDone")]
    [Button("아이템 획득", ButtonSizes.Large)]
    public void AddItem()
    {
        TicketMachine ticketMachine = player.GetComponent<TicketMachine>();

        for (int i = 0; i < 20; i++)
        {
            ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4017));
            ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4000));
        }

        for (int i = 0; i < 5; i++)
        {
            ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4020));
            ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4021));
        }

        for (int i = 0; i < 5; i++)
        {
            ticketMachine.SendMessage(ChannelType.UI, new UIPayload
            {
                uiType = UIType.Notify,
                groupType = GroupType.Item,
                slotAreaType = SlotAreaType.Item,
                actionType = ActionType.AddSlotItem,
                itemData = DataManager.Instance.GetIndexData<ItemData, ItemDataParsingInfo>(4100),
            });

            ticketMachine.SendMessage(ChannelType.UI, new UIPayload
            {
                uiType = UIType.Notify,
                groupType = GroupType.Item,
                slotAreaType = SlotAreaType.Item,
                actionType = ActionType.AddSlotItem,
                itemData = DataManager.Instance.GetIndexData<ItemData, ItemDataParsingInfo>(4101),
            });
        }
    }

    private UIPayload GenerateStoneAcquirePayloadTest(int index)
    {
        //for test
        UIPayload payload = new UIPayload();
        payload.uiType = UIType.Notify;
        payload.actionType = ActionType.AddSlotItem;
        payload.slotAreaType = SlotAreaType.Item;
        payload.groupType = GroupType.Stone;
        payload.itemData = DataManager.Instance.GetIndexData<StoneData, StoneDataParsingInfo>(index);
        return payload;
    }
}
