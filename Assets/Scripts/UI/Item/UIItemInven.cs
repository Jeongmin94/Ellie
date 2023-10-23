using System;
using Assets.Scripts.UI.Framework.Static;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Item
{
    public class UIItemInven : UIStatic
    {
        // left: w, h 늘어난만큼 posX 감소
        // top: w, h 늘어난만큼 posY 증가
        // right: w, h 늘어난만큼 posX 증가
        // bottom: w, h 늘어난만큼 posY 감소
        private enum Buttons
        {
            LeftButton,
            TopButton,
            RightButton,
            BottomButton
        }

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            Bind<Button>(typeof(Buttons));
        }
    }
}