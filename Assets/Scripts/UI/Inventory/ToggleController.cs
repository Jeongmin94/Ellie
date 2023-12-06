using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.Events;
using Toggle = UnityEngine.UI.Toggle;

namespace Assets.Scripts.UI.Inventory
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleController : MonoBehaviour
    {
        protected Toggle toggle;

        public bool IsOn
        {
            get => toggle.isOn;
            set => toggle.isOn = value;
        }

        public bool Interactable
        {
            get => toggle.interactable;
            set => toggle.interactable = value;
        }

        protected void InitToggle()
        {
            toggle = gameObject.GetOrAddComponent<Toggle>();
        }

        protected void SubscribeToggleEvent(UnityAction<bool> listener)
        {
            toggle.onValueChanged.AddListener(listener);
        }

        protected virtual void OnDestroy()
        {
            toggle.onValueChanged.RemoveAllListeners();
        }
    }
}