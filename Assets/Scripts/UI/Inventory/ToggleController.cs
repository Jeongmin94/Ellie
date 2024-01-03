using UnityEngine;
using UnityEngine.Events;
using Utils;
using Toggle = UnityEngine.UI.Toggle;

namespace UI.Inventory
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleController : MonoBehaviour
    {
        protected Toggle toggle;

        public bool IsOn
        {
            get { return toggle.isOn; }
            set { toggle.isOn = value; }
        }

        public bool Interactable
        {
            get { return toggle.interactable; }
            set { toggle.interactable = value; }
        }

        protected virtual void OnDestroy()
        {
            toggle.onValueChanged.RemoveAllListeners();
        }

        protected void InitToggle()
        {
            toggle = gameObject.GetOrAddComponent<Toggle>();
        }

        protected void SubscribeToggleEvent(UnityAction<bool> listener)
        {
            toggle.onValueChanged.AddListener(listener);
        }
    }
}