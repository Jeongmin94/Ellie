using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Managers
{
    public class InputManager : Singleton<InputManager>
    {
        public Action keyAction;
        public Action mouseAction;
        public Action escapeAction;

        private bool isMousePressed = false;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
                escapeAction?.Invoke();
            
            if (EventSystem.current && EventSystem.current.IsPointerOverGameObject())
                return;

            if (Input.anyKey)
                keyAction?.Invoke();

            if (Input.GetMouseButton(0))
            {
                mouseAction?.Invoke();
                isMousePressed = true;
            }
            else
            {
                if (isMousePressed)
                    mouseAction?.Invoke();

                isMousePressed = false;
            }
        }
    }
}