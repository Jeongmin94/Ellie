using UI.Opening.PopupMenu.MenuButton;
using UnityEditor;

namespace UI.Opening.PopupMenu.PopupCanvas.PopupCanvasImpl
{
    public class ExitPopup : PopupCanvas
    {
        public override void Invoke(PopupPayload payload)
        {
            if (payload.buttonType == ButtonType.Yes)
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
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