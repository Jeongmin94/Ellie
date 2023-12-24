namespace Player
{
    public struct StateInfo
    {
        public float maxMoveSpeed;
        public float stateDuration;
        public float magnitude;
    }

    public abstract class PlayerBaseState
    {
        public PlayerBaseState(PlayerController controller)
        {
            Controller = controller;
        }

        protected PlayerController Controller { get; private set; }

        public abstract void OnEnterState();

        public virtual void OnEnterState(StateInfo info)
        {
        }

        public abstract void OnUpdateState();
        public abstract void OnFixedUpdateState();
        public abstract void OnExitState();
    }
}