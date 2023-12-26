using Assets.Scripts.Item;
using Assets.Scripts.UI.Inventory;
using UnityEngine;
using UnityEngine.Serialization;

namespace Channels.UI
{
    public enum UIType
    {
        BarImage,
        SlotItem,
        Notify
    }

    public enum ActionType
    {
        // Inventory UI
        AddSlotItem,
        ConsumeSlotItem,
        ToggleInventory,
        MoveClockwise,
        MoveCounterClockwise,
        SetPlayerProperty,
        ClickCloseButton,

        // Quest UI
        ClearQuest,
        SetQuestName,
        SetQuestDesc,
        SetQuestIcon,
        
        // Interactive UI
        PopupInteractive,
        CloseInteractive,
        
        // AutoSave
        OpenAutoSave,
        CloseAutoSave,
        
        // Death
        OpenDeathCanvas,
        
        // Video
        PlayVideo,

        // Pause Canvas
        OpenPauseCanvas,
        ClosePauseCanvas,
        
        // GuideCanvas
        OpenGuideCanvas,
        
        // ScreenDamage
        ShowBlurEffect,
    }

    public struct QuestInfo
    {
        public Sprite questIcon;
        public string questName;
        public string questDesc;

        private QuestInfo(Sprite questIcon, string questName, string questDesc)
        {
            this.questIcon = questIcon;
            this.questName = questName;
            this.questDesc = questDesc;
        }

        public static QuestInfo Of(Sprite sprite, string questName, string questDesc)
        {
            return new QuestInfo(sprite, questName, questDesc);
        }
    }

    public enum InteractiveType
    {
        Default,
        Chatting,
        Mining,
        Acquisition,
    }

    public class UIPayload : IBaseEventPayload
    {
        public UIType uiType;
        public ActionType actionType;
        public SlotAreaType slotAreaType;

        public GroupType groupType;

        //ItemMetaData는 UI에 출력할 데이터들만 포함합니다
        public ItemMetaData itemData;
        public Transform onDragParent;
        public bool isStoneNull;
        public int equipmentSlotIdx;

        // Quest
        public QuestInfo questInfo;
        
        // Interactive
        public InteractiveType interactiveType;
        public static UIPayload Notify()
        {
            UIPayload payload = new UIPayload();
            payload.uiType = UIType.Notify;
            return payload;
        }
        
        // ScreenDamage
        public float blurClarity = -1.0f;
    }

    public class UIChannel : BaseEventChannel
    {
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            if (payload is not UIPayload uiPayload)
                return;

            if (uiPayload.uiType == UIType.Notify)
            {
                Publish(payload);
                return;
            }

            // do something
        }
    }
}