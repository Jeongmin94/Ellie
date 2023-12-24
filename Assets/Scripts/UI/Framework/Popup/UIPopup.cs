using Assets.Scripts.Managers;

namespace UI.Framework.Popup
{
    public class UIPopup : UIBase
    {
        protected override void Init()
        {
            UIManager.Instance.SetCanvas(gameObject);
        }

        public virtual void ClosePopup()
        {
            UIManager.Instance.ClosePopup(this);
        }
    }
}