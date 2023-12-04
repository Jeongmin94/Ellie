using Assets.Scripts.Managers;
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

            PoolManager.Instance.DestroyAllPools();
            SoundManager.Instance.ClearAudioControllers();
            SoundManager.Instance.InitAudioSourcePool();
            Init();
        }

        protected override void Start()
        {
            base.InitObjects();
        }
    }
}