using Assets.Scripts.Managers.Singleton;
using Centers;
using UnityEngine;

namespace Assets.Scripts
{
    public class OpeningCenter : BaseCenter
    {
        private void Awake()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            MangerControllers.ClearAction(ManagerType.Input);
            MangerControllers.ClearAction(ManagerType.Sound);

            Init();
        }

        protected override void Start()
        {
            base.InitObjects();
        }
    }
}