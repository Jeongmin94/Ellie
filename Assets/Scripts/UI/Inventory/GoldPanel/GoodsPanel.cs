using Assets.Scripts.UI.Framework;

namespace Assets.Scripts.UI.Inventory
{
    public class GoodsPanel : UIBase
    {
        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            Bind();
            InitObjects();
        }

        private void Bind()
        {
        }

        private void InitObjects()
        {
        }
    }
}