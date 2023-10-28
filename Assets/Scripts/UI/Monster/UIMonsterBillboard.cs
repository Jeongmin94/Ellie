using Assets.Scripts.UI.Framework.Billboard;
using UnityEngine;

namespace Assets.Scripts.UI.Monster
{
    public class UIMonsterBillboard : UIMonsterCanvas, IBillboard
    {
        public float scaleFactor = 0.001f;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();
            var canvas = gameObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
        }

        public void InitBillboard(Transform parent)
        {
            transform.localScale *= scaleFactor;
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
        }

        public void UpdateBillboard()
        {
            transform.rotation = Camera.main.transform.rotation;
        }

        private void Update()
        {
            UpdateBillboard();
        }
    }
}