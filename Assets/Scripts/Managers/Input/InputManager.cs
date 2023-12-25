using System;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers.Input
{
    public enum InputType
    {
        Key,
        Mouse,
        Escape
    }

    public class InputManager : Singleton<InputManager>
    {
        private bool canInput;
        public Action escapeAction;

        private bool isMousePressed;
        public Action keyAction;
        public Action mouseAction;

        public bool CanInput
        {
            get => canInput;
            set
            {
                canInput = value;
                Debug.Log($"호출시점 확인 :: 인풋 매니저 밸류 {value}");
            }
        }

        public bool PrevCanInput { get; private set; }


        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                escapeAction?.Invoke();
            }

            PrevCanInput = CanInput;
            if (!CanInput)
            {
                return;
            }

            if (EventSystem.current && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (UnityEngine.Input.anyKey)
            {
                keyAction?.Invoke();
            }

            if (UnityEngine.Input.GetMouseButton(0))
            {
                mouseAction?.Invoke();
                isMousePressed = true;
            }
            else
            {
                if (isMousePressed)
                {
                    mouseAction?.Invoke();
                }

                isMousePressed = false;
            }
        }

        public void Subscribe(InputType type, Action listener)
        {
            switch (type)
            {
                case InputType.Key:
                    keyAction -= listener;
                    keyAction += listener;
                    break;
                case InputType.Mouse:
                    mouseAction -= listener;
                    mouseAction += listener;
                    break;
                case InputType.Escape:
                    escapeAction -= listener;
                    escapeAction += listener;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public override void ClearAction()
        {
            keyAction = null;
            mouseAction = null;
            escapeAction = null;
        }
    }
}