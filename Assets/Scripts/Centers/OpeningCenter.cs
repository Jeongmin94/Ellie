using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Singleton;

namespace Centers
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
            InitObjects();
        }
    }
}