using System;
using Assets.Scripts.Data.ActionData.Player;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Item;
using Assets.Scripts.UI.Player;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Framework
{
    public class UITestClient : MonoBehaviour
    {
        private const string UIButtonCanvas = "ButtonCanvas";
        private const string UIHealthAndStamina = "HealthAndStamina";
        private const string UIStoneInven = "StoneInven";

        [SerializeField] private PlayerHealthData healthData;
        [SerializeField] private StaminaData staminaData;
        [SerializeField] private int damage = 1;
        [SerializeField] private int staminaCost = 10;

        private void Awake()
        {
            healthData.InitHealth();
            staminaData.InitStamina();
        }

        private void Start()
        {
            UIManager.Instance.MakePopup<UIPopupButton>(UIButtonCanvas);
            UIManager.Instance.MakeStatic<UIHealthAndStamina>(UIHealthAndStamina);
            UIManager.Instance.MakeStatic<UIStoneInven>(UIStoneInven);
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
        }
    }
}