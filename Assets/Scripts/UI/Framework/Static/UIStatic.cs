using Assets.Scripts.Managers;
using Managers.UI;

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