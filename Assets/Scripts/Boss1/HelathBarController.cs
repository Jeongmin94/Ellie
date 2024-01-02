using System;
using Managers.UI;
using Monsters.Utility;
using Sirenix.OdinInspector;
using UI.Monster;
using UnityEngine;

namespace Boss1
{
    public abstract class HelathBarController : SerializedMonoBehaviour
    {
        [SerializeField][Required] private Transform billboardObject;
        
        public float scaleFactor = 0.003f;
        
        protected readonly MonsterDataContainer dataContainer = new();
        protected UIMonsterBillboard billboard;
        
        private Transform cameraObj;
        private bool isBillboardOn;

        public Transform BillboardObject
        {
            get { return billboardObject; }
        }
        
        private void Awake()
        {
            cameraObj = Camera.main.transform;

            InitUI();
        }
        
        private void Update()
        {
            CheckHelathBar();
        }
    
        private void InitUI()
        {
            billboard = UIManager.Instance.MakeStatic<UIMonsterBillboard>(billboardObject, UIManager.UIMonsterBillboard);
            billboard.scaleFactor = scaleFactor;
            billboard.InitBillboard(billboardObject);
        }
        
        private void CheckHelathBar()
        {
            if (isBillboardOn)
            {
                var direction = transform.position - cameraObj.position;
                var dot = Vector3.Dot(direction.normalized, cameraObj.forward.normalized);

                if (dot > -0.6f)
                {
                    ShowBillboard();
                }
                else
                {
                    HideBillboard();
                }
            }
        }

        public void ShowBillboard()
        {
            billboardObject.transform.localScale = Vector3.one;
            isBillboardOn = true;
        }

        public void HideBillboard()
        {
            billboardObject.transform.localScale = Vector3.zero;
        }
        
        public void RenewHealthBar(int currentHP)
        {
            if (currentHP >= 0)
            {
                dataContainer.CurrentHp.Value = currentHP;
            }
        }

        public abstract void InitData(BaseBTData data);
    }
}
