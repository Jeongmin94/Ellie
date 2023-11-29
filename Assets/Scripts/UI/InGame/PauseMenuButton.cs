using Assets.Scripts.UI.Opening;
using Data.UI.Opening;
using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class PauseMenuButton : OpeningText
    {
        public new static readonly string Path = "Pause/PauseMenuButton";

        [SerializeField] private TextTypographyData typographyData;
    }
}