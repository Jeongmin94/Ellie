using System;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Managers.Game
{
    public struct Resolution
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public bool IsChanged(int width, int height)
        {
            return Width != width || Height != height;
        }
    }

    public class GameManager : Singleton<GameManager>
    {
        public Action<Resolution> changeResolutionAction;
        public Resolution resolution;

        public override void Awake()
        {
            base.Awake();

            resolution.Width = Screen.width;
            resolution.Height = Screen.height;
        }

        private void Update()
        {
            if (resolution.IsChanged(Screen.width, Screen.height))
            {
                resolution.Width = Screen.width;
                resolution.Height = Screen.height;
                changeResolutionAction?.Invoke(resolution);
            }
        }
    }
}