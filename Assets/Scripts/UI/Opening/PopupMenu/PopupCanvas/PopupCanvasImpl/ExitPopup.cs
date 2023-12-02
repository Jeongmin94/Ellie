using Application = UnityEngine.Application;

namespace Assets.Scripts.UI.PopupMenu
{
    public class ExitPopup : PopupCanvas
    {
        public override void Invoke(PopupPayload payload)
        {
            if (payload.buttonType == ButtonType.Yes)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
            else
            {
                base.Invoke(payload);
            }
        }
    }
}