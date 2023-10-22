using System;
using Assets.Scripts.Data.ActionData.Player;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Player;
using UnityEngine;

namespace Assets.Scripts.UI.Framework
{
    public class UITestClient : MonoBehaviour
    {
        private const string UIButtonCanvas = "ButtonCanvas";
        private const string UIPlayerHealthCanvas = "PlayerHealthCanvas";

        [SerializeField] private PlayerHealthData healthData;
        [SerializeField] private int damage = 1;

        private void Awake()
        {
            healthData.InitHealth();
        }

        private void Start()
        {
            UIManager.Instance.MakePopup<UIPopupButton>(UIButtonCanvas);
            UIManager.Instance.MakeStatic<UIPlayerHealth>(UIPlayerHealthCanvas);
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
        }
    }
}