namespace Assets.Scripts.Player
{
    public struct StateInfo
    {
        public float maxMoveSpeed;
        public float stateDuration;
        public float magnitude;
    }
    public abstract class PlayerBaseState
    {
        protected PlayerController Controller { get; private set; }

        public PlayerBaseState(PlayerController controller)
        {
            this.Controller = controller;
        }

        public abstract void OnEnterState();
        public virtual void OnEnterState(StateInfo info) { }
        public abstract void OnUpdateState();
        public abstract void OnFixedUpdateState();
        public abstract void OnExitState();
    }
}
