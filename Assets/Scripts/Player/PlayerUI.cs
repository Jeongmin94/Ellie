using Assets.Scripts.Managers;
using Data.ActionData.Player;
using Managers.UI;
using UI.Player;
using UI.Status;
using UnityEngine;

namespace Player
{
    public class PlayerUI : MonoBehaviour
    {
        private const string UIHealthAndStamina = "Player/HealthAndStamina";
        private const string UIStatusCanvas = "Player/StatusCanvas";

        private const string UIStoneInven = "Item/StoneInven";
        private const string UIItemInven = "Item/ItemInven";

        [SerializeField] private PlayerHealthData healthData;
        [SerializeField] private StaminaData staminaData;

        private UIStatusBar statusBar;

        public UIBarImage StaminaBarImage { get; private set; }

        private void Awake()
        {
            healthData.InitHealth();
            staminaData.InitStamina();
        }

        private void Start()
        {
            var healthAndStamina = UIManager.Instance.MakeStatic<UIHealthAndStamina>(UIHealthAndStamina);
            StaminaBarImage = healthAndStamina.BarImage;

            // !TODO 상태이상 추가되면 활성화
            // statusBar = UIManager.Instance.MakeStatic<UIStatusBar>(UIStatusCanvas);
        }
    }
}