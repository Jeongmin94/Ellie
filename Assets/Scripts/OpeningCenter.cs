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

            SoundManager.Instance.ClearAudioControllers();
            Init();
        }

        protected override void Start()
        {
            base.InitObjects();
        }
    }
}