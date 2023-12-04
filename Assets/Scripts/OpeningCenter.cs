using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Singleton;
using Centers;

namespace Assets.Scripts
{
    public class OpeningCenter : BaseCenter
    {
        private void Awake()
        {
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