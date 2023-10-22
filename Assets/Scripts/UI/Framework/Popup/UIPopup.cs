using Assets.Scripts.Managers;

namespace Assets.Scripts.UI.Framework.Popup
{
    public class UIPopup : UIBase
    {
        protected override void Init()
        {
            UIManager.Instance.SetCanvas(gameObject, true);
        }

        public virtual void ClosePopup()
        {
            UIManager.Instance.ClosePopup(this);
        }
    }
}