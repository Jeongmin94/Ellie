using UnityEngine;

namespace UI.Framework.Billboard
{
    public interface IBillboard
    {
        public void InitBillboard(Transform parent);

        public void UpdateBillboard();
    }
}