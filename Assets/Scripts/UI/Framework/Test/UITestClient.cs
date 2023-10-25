using System;
using Assets.Scripts.ActionData.Monster;
using Assets.Scripts.Data.ActionData.Player;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Item;
using Assets.Scripts.UI.Monster;
using Assets.Scripts.UI.Player;
using Assets.Scripts.UI.Status;
using UnityEngine;

namespace Assets.Scripts.UI.Framework
{
    public class UITestClient : MonoBehaviour
    {
        private const string UIButtonCanvas = "ButtonCanvas";
        private const string UIHealthAndStamina = "Player/HealthAndStamina";
        private const string UIStoneInven = "Item/StoneInven";
        private const string UIItemInven = "Item/ItemInven";
        private const string UIStatusCanvas = "Player/StatusCanvas";
        private const string UIMonsterCanvas = "Monster/MonsterCanvas";
        private const string UIMonsterBillboard = "Monster/MonsterBillboard";

        [SerializeField] private Transform billBoardPosition;
        [SerializeField] private PlayerHealthData healthData;
        [SerializeField] private StaminaData staminaData;
        [SerializeField] private MonsterHealthData monsterHealthData;
        [SerializeField] private int damage = 1;
        [SerializeField] private int staminaCost = 10;

        private UIStatusBar statusBar;

        private void Awake()
        {
            healthData.InitHealth();
            monsterHealthData.InitHealth();
            staminaData.InitStamina();
        }

        private void Start()
        {
            UIManager.Instance.MakePopup<UIPopupButton>(UIButtonCanvas);
            UIManager.Instance.MakeStatic<UIHealthAndStamina>(UIHealthAndStamina);
            UIManager.Instance.MakeStatic<UIStoneInven>(UIStoneInven);
            UIManager.Instance.MakeStatic<UIItemInven>(UIItemInven);
            statusBar = UIManager.Instance.MakeStatic<UIStatusBar>(UIStatusCanvas);

            // UIManager.Instance.MakeStatic<UIMonsterCanvas>(UIMonsterCanvas);

            var billboard = UIManager.Instance.MakeStatic<UIMonsterBillboard>(transform, UIMonsterBillboard);
            billboard.scaleFactor = 0.003f;
            billboard.InitBillboard(billBoardPosition);
        }

        private void OnGUI()
        {
            float w = Screen.width;
            float h = Screen.height;
            if (GUI.Button(new Rect(10, h - 30, 100, 20), "attack player"))
            {
                int val = Math.Clamp(healthData.CurrentHealth.Value - damage, 0, healthData.MaxHealth);
                healthData.CurrentHealth.Value = val;
            }

            if (GUI.Button(new Rect(10, h - 60, 100, 20), "heal player"))
            {
                int val = Math.Clamp(healthData.CurrentHealth.Value + damage, 0, healthData.MaxHealth);
                healthData.CurrentHealth.Value = val;
            }

            if (GUI.Button(new Rect(10, h - 90, 100, 20), "use stamina"))
            {
                int val = Math.Clamp(staminaData.CurrentStamina.Value - staminaCost, 0, staminaData.MaxStamina);
                staminaData.CurrentStamina.Value = val;
            }

            if (GUI.Button(new Rect(10, h - 120, 100, 20), "restore stamina"))
            {
                int val = Math.Clamp(staminaData.CurrentStamina.Value + staminaCost, 0, staminaData.MaxStamina);
                staminaData.CurrentStamina.Value = val;
            }

            if (GUI.Button(new Rect(10, h - 150, 100, 20), "add status"))
            {
                statusBar.AddStatus();
            }

            if (GUI.Button(new Rect(10, h - 180, 100, 20), "remove status"))
            {
                statusBar.RemoveStatus();
            }

            if (GUI.Button(new Rect(300, h - 30, 100, 20), "attack monster"))
            {
                int val = Math.Clamp(monsterHealthData.CurrentHealth.Value - damage, 0, monsterHealthData.MaxHealth);
                monsterHealthData.CurrentHealth.Value = val;
            }

            if (GUI.Button(new Rect(300, h - 60, 100, 20), "heal monster"))
            {
                int val = Math.Clamp(monsterHealthData.CurrentHealth.Value + damage, 0, monsterHealthData.MaxHealth);
                monsterHealthData.CurrentHealth.Value = val;
            }
        }
    }
}