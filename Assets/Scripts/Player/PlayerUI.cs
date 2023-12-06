using Assets.Scripts.Data.ActionData.Player;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Item;
using Assets.Scripts.UI.Player;
using Assets.Scripts.UI.Status;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
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
        private UIBarImage staminaBarImage;
        public UIBarImage StaminaBarImage
        {
            get { return staminaBarImage; }
        }

        private void Awake()
        {
            healthData.InitHealth();
            staminaData.InitStamina();
        }
        private void Start()
        {

            var healthAndStamina = UIManager.Instance.MakeStatic<UIHealthAndStamina>(UIHealthAndStamina);
            staminaBarImage = healthAndStamina.BarImage;
            
            // !TODO 상태이상 추가되면 활성화
            // statusBar = UIManager.Instance.MakeStatic<UIStatusBar>(UIStatusCanvas);
        }
    }
}