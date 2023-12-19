using Assets.Scripts.Centers;
using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using Assets.Scripts.UI.Inventory;
using Boss.Terrapupa;
using Channels.Components;
using Channels.Dialog;
using Channels.Type;
using Channels.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatClient : SerializedMonoBehaviour
{
    [InfoBox("치트 사용 가능 여부")] public bool canCheat = true;

    [Required] public Transform player;
    [Required] public Transform terrapupa;
    [Required] public Transform terra;
    [Required] public Transform pupa;
    [Required] public List<Transform> minions;

    private GameObject canvas;
    private GameObject canvas2;

    private void Update()
    {
        if(canCheat)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                // 영상용 캔버스 끄기
                OnOffCanvas();
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {

            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                // ELLIE POWER OVERWHELMING!!
                // 구르면 풀림
                player.gameObject.tag = "Untagged";
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                // SAVE DATA
                Save();
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                // LOAD DATA
                Load();
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                // KILL CURRENT PHASE BOSS
                KillCurrentBoss();
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                // HIT CURRENT BOSS (-1 DAMAGE)
                DamageTerrapupa();
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                // DEACTIVATE BOSS BATTLE
                DeactivateBoss();
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                SkipToEnding();
            }
            if (Input.GetKeyDown(KeyCode.F1))
            {
                // TELEPORT START
                SetPlayerPosition1();
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                // TELEPORT BOSS ROOM
                SetPlayerPosition2();
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                // TELEPORT STONE FOOTBOARD PUZZLE
                SetPlayerPosition3();
            }
            if (Input.GetKeyDown(KeyCode.F4))
            {
                // TELEPORT BREAKABLE STONE PUZZLE
                SetPlayerPosition4();
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                // TELEPORT PILLAR PUZZLE
                SetPlayerPosition5();
            }
            if (Input.GetKeyDown(KeyCode.F6))
            {
                // TELEPORT RAIL START
                SetPlayerPosition6();
            }
            if (Input.GetKeyDown(KeyCode.F7))
            {
                // TELEPORT NPC1
                SetPlayerPosition7();
            }
            if (Input.GetKeyDown(KeyCode.F8))
            {
                // TELEPORT NPC2
                SetPlayerPosition8();
            }
            if (Input.GetKeyDown(KeyCode.F9))
            {
                // TELEPORT NPC3
                SetPlayerPosition9();
            }
            if (Input.GetKeyDown(KeyCode.F10))
            {

            }
            if (Input.GetKeyDown(KeyCode.F11))
            {

            }
            if (Input.GetKeyDown(KeyCode.F12))
            {

            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                AddItem();
            }
        }
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

    [Button("돌 부수기 퍼즐 이동", ButtonSizes.Large)]
    public void SetPlayerPosition4()
    {
        player.position = new Vector3(-115.6f, 13.02f, 390.79f);
    }

    [Button("돌 넣기 퍼즐 이동", ButtonSizes.Large)]
    public void SetPlayerPosition5()
    {
        player.position = new Vector3(39.0f, 3.28f, 14.81f);
    }

    [Button("광차 이동", ButtonSizes.Large)]
    public void SetPlayerPosition6()
    {
        player.position = new Vector3(32.96f, 13.56f, 155.13f);
    }

    [Button("NPC 1번 이동", ButtonSizes.Large)]
    public void SetPlayerPosition7()
    {
        player.position = new Vector3(-68.019f, 3.16f, 74.72f);
    }

    [Button("NPC 2번 이동", ButtonSizes.Large)]
    public void SetPlayerPosition8()
    {
        player.position = new Vector3(-68.01f, 3.16f, 112.71f);
    }

    [Button("NPC 3번 이동", ButtonSizes.Large)]
    public void SetPlayerPosition9()
    {
        player.position = new Vector3(79.38f, 3.16f, 85.80f);
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
        terrapupa.GetComponent<TerrapupaBTController>().Dead();
    }
    [EnableIf("IsRuntime")]
    [Button("2페이즈 스킵", ButtonSizes.Large)]
    public void KillTerraAndPupa()
    {
        Debug.Log("테라, 푸파 사망 치트");

        terra.GetComponent<TerrapupaBTController>().Dead();
        pupa.GetComponent<TerrapupaBTController>().Dead();
    }
    [EnableIf("IsRuntime")]
    [Button("3페이즈 스킵", ButtonSizes.Large)]
    public void KillMinions()
    {
        Debug.Log("미니언 사망 치트");

        foreach (var minion in minions)
        {
            minion.GetComponent<TerrapupaMinionBTController>().Dead();
        }
    }
    [EnableIf("IsRuntime")]
    [Button("테라푸파 데미지", ButtonSizes.Large)]
    public void DamageTerrapupa()
    {
        Debug.Log("테라, 푸파 데미지 입히기");

        terrapupa.GetComponent<TerrapupaBTController>().GetDamaged(1);
        terra.GetComponent<TerrapupaBTController>().GetDamaged(1);
        pupa.GetComponent<TerrapupaBTController>().GetDamaged(1);
        foreach (var minion in minions)
        {
            minion.GetComponent<TerrapupaMinionBTController>().GetDamaged(1);
        }
    }
    [EnableIf("IsRuntime")]
    [Button("테라푸파 데미지", ButtonSizes.Large)]
    public void DeactivateBoss()
    {
        Debug.Log("보스 비활성화");

        terrapupa.gameObject.SetActive(false);
        terra.gameObject.SetActive(false);
        pupa.gameObject.SetActive(false);
        foreach (var minion in minions)
        {
            minion.gameObject.SetActive(false);
        }
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

        for (int i = 0; i < 100; i++)
        {
            ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4000));
            ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4001));
            ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4003));
            ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4005));
            ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4010));
            ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4017));
            ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4019));
            ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4020));
            ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4021));
        }

        for (int i = 0; i < 20; i++)
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


    /// <summary>
    /// 기타
    /// </summary>
    /// 
    [Title("기타")]
    [EnableIf("IsRuntime")]
    [Button("엔딩 스킵", ButtonSizes.Large)]
    public void SkipToEnding()
    {
        Debug.Log("엔딩으로 스킵");

        SceneLoadManager.Instance.LoadScene(SceneName.Closing);
    }

    [EnableIf("IsRuntime")]
    [Button("캔버스 켰다끄기", ButtonSizes.Large)]
    public void OnOffCanvas()
    {
        if(canvas == null)
        {
            canvas = GameObject.Find("@UI_Root");
        }

        if(canvas.activeSelf)
        {
            canvas.SetActive(false);
        }
        else
        {
            canvas.SetActive(true);
        }
    }

    private void KillCurrentBoss()
    {
        if(terrapupa.gameObject.activeSelf)
        {
            KillTerrapupa();
        }
        else if (terra.gameObject.activeSelf)
        {
            KillTerraAndPupa();
        }
        else
        {
            KillMinions();
        }
    }

    private void Save()
    {
        SaveLoadManager.Instance.SaveData();
    }

    private void Load()
    {
        TicketMachine ticketMachine = player.GetComponent<TicketMachine>();

        DialogPayload payload = DialogPayload.Stop();
        payload.canvasType = DialogCanvasType.Default;
        ticketMachine.SendMessage(ChannelType.Dialog, payload);
        payload.canvasType = DialogCanvasType.Simple;
        ticketMachine.SendMessage(ChannelType.Dialog, payload);
        payload.canvasType = DialogCanvasType.SimpleRemaining;
        ticketMachine.SendMessage(ChannelType.Dialog, payload);

        SceneLoadManager.Instance.FinishLoading();
        SaveLoadManager.Instance.LoadData();
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
