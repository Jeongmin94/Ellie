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