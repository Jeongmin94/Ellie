using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.Events;
using Toggle = UnityEngine.UI.Toggle;

namespace Assets.Scripts.UI.Inventory
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleController : MonoBehaviour
    {
        private Toggle toggle;

        public bool IsOn
        {
            get => toggle.isOn;
            set => toggle.isOn = value;
        }

        protected void InitToggle()
        {
            toggle = gameObject.GetOrAddComponent<Toggle>();
        }

        protected void SubscribeToggleEvent(UnityAction<bool> listener)
        {
            toggle.onValueChanged.AddListener(listener);
        }

        protected void OnDestroy()
        {
            toggle.onValueChanged.RemoveAllListeners();
        }
    }
}