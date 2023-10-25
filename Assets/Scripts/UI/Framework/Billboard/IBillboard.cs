using UnityEngine;

namespace Assets.Scripts.UI.Framework.Billboard
{
    public interface IBillboard
    {
        public void InitBillboard(Transform target);

        public void UpdateBillboard();
    }
}