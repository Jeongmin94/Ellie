using Assets.Scripts.Particle;
using System;

namespace Assets.Scripts.Managers.Singleton
{
    public enum ManagerType
    {
        Data,
        Game,
        Input,
        Resource,
        Ticket,
        UI,
        Sound,
        EventBus,
        Particle,
    }

    public static class MangerControllers
    {
        public static void ClearAction(ManagerType type)
        {
            switch (type)
            {
                case ManagerType.Data:
                    DataManager.Instance.ClearAction();
                    break;
                case ManagerType.Game:
                    GameManager.Instance.ClearAction();
                    break;
                case ManagerType.Input:
                    InputManager.Instance.ClearAction();
                    break;
                case ManagerType.Resource:
                    ResourceManager.Instance.ClearAction();
                    break;
                case ManagerType.Ticket:
                    TicketManager.Instance.ClearAction();
                    break;
                case ManagerType.UI:
                    UIManager.Instance.ClearAction();
                    break;
                case ManagerType.Sound:
                    SoundManager.Instance.ClearAction();
                    break;
                case ManagerType.EventBus:
                    EventBus.Instance.ClearAction();
                    break;
                case ManagerType.Particle:
                    ParticleManager.Instance.ClearAction();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}