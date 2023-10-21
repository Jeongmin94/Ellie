using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Managers
{
    public class InputManager : Singleton<InputManager>
    {
        public Action OnKeyAction;
        public Action OnMouseAction;

        private bool isMousePressed = false;

        private void Update()
        {
            if (EventSystem.current && EventSystem.current.IsPointerOverGameObject())
                return;

            if (Input.anyKey)
                OnKeyAction?.Invoke();

            if (Input.GetMouseButton(0))
            {
                OnMouseAction?.Invoke();
                isMousePressed = true;
            }
            else
            {
                if (isMousePressed)
                    OnMouseAction?.Invoke();

                isMousePressed = false;
            }
        }
    }
}