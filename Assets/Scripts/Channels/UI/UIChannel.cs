using Item;
using UI.Inventory.CategoryPanel;
using UI.Inventory.Slot;
using UnityEngine;

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
        ShowBlurEffect
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
        Acquisition
    }

    public class UIPayload : IBaseEventPayload
    {
        public ActionType actionType;

        // ScreenDamage
        public float blurClarity = -1.0f;
        public int equipmentSlotIdx;

        public GroupType groupType;

        // Interactive
        public InteractiveType interactiveType;
        public bool isStoneNull;

        //ItemMetaData는 UI에 출력할 데이터들만 포함합니다
        public ItemMetaData itemData;
        public Transform onDragParent;

        // Quest
        public QuestInfo questInfo;
        public SlotAreaType slotAreaType;
        public UIType uiType;

        public static UIPayload Notify()
        {
            var payload = new UIPayload();
            payload.uiType = UIType.Notify;
            return payload;
        }
    }

    public class UIChannel : BaseEventChannel
    {
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            if (payload is not UIPayload uiPayload)
            {
                return;
            }

            if (uiPayload.uiType == UIType.Notify)
            {
                Publish(payload);
            }

            // do something
        }
    }
}