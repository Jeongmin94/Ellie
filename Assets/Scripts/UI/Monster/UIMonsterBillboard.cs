using System;
using Assets.Scripts.UI.Framework.Billboard;
using UnityEngine;

namespace Assets.Scripts.UI.Monster
{
    public class UIMonsterBillboard : UIMonsterCanvas, IBillboard
    {
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

        public void InitBillboard(Transform target)
        {
            transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
            transform.position = target.position;
            transform.SetParent(target);
        }

        private void Update()
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);
        }
    }
}