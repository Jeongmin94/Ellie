using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Inventory;
using Boss.Terrapupa;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using Sirenix.OdinInspector;
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

    [Button("���� ��� ����", ButtonSizes.Large)]
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
    /// ����� ���� �̵�
    /// </summary>

    [Title("����� ���� �ڷ���Ʈ")]
    [ValueDropdown("savePositionList")]
    public Vector3 savePosition = Vector3.zero;
    private List<Vector3> savePositionList = new List<Vector3>();

    [Button("�÷��̾� ������ ����", ButtonSizes.Large)]
    public void SavePosition()
    {
        savePositionList.Add(player.position);
    }

    [Button("�÷��̾� ���� ��ġ ����Ʈ ����", ButtonSizes.Large)]
    public void ResetPositionList()
    {
        savePositionList.Clear();
        savePosition = Vector3.zero;
    }

    [Button("������ ������ġ�� �÷��̾� ��ġ �̵�", ButtonSizes.Large)]
    public void SetPlayerPositionToSavedPosition()
    {
        player.position = savePosition;
    }

    /// <summary>
    /// ���� ��ġ �̵�
    /// </summary>

    [Title("���� ��� �̵�")]
    [Button("���� ���� �̵�", ButtonSizes.Large)]
    public void SetPlayerPosition1()
    {
        player.position = new Vector3(-5.44f, 7.03f, -10.4f);
    }

    [Button("���� �� �� �̵�", ButtonSizes.Large)]
    public void SetPlayerPosition2()
    {
        player.position = new Vector3(-152f, 13.92f, 645.51f);
    }

    [Button("�� ���� ���� �̵�", ButtonSizes.Large)]
    public void SetPlayerPosition3()
    {
        player.position = new Vector3(33.5f, 11.8f, 98.8f);
    }

    /// <summary>
    /// ���� ġƮŰ
    /// </summary>

    [Title("���� ġƮŰ")]
    [EnableIf("IsRuntime")]
    [Button("1������ ��ŵ", ButtonSizes.Large)]
    public void KillTerrapupa()
    {
        Debug.Log("�׶�Ǫ�� ��� ġƮ");
        terrapupa.GetComponent<TerrapupaBTController>().terrapupaData.currentHP.Value = 0;
    }
    [EnableIf("IsRuntime")]
    [Button("2������ ��ŵ", ButtonSizes.Large)]
    public void KillTerraAndPupa()
    {
        Debug.Log("�׶�, Ǫ�� ��� ġƮ");

        terra.GetComponent<TerrapupaBTController>().terrapupaData.currentHP.Value = 0;
        pupa.GetComponent<TerrapupaBTController>().terrapupaData.currentHP.Value = 0;
    }
    [EnableIf("IsRuntime")]
    [Button("2������ ��ŵ", ButtonSizes.Large)]
    public void DamageTerrapupa()
    {
        Debug.Log("�׶�, Ǫ�� ������ ������");

        terrapupa.GetComponent<TerrapupaBTController>().GetDamaged(1);
        terra.GetComponent<TerrapupaBTController>().GetDamaged(1);
        pupa.GetComponent<TerrapupaBTController>().GetDamaged(1);
    }

    /// <summary>
    /// ������ ġƮŰ
    /// </summary>
    /// 
    [Title("������ ȹ��")]
    [EnableIf("IsRuntime")]
    [EnableIf("IsParsingDone")]
    [Button("������ ȹ��", ButtonSizes.Large)]
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
