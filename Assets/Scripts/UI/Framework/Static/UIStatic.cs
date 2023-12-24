using Assets.Scripts.Managers;

namespace UI.Framework.Static
{
    public class UIStatic : UIBase
    {
        protected override void Init()
        {
            UIManager.Instance.SetCanvas(gameObject, false);
        }
    }
}