namespace Assets.Scripts.Player
{
    public abstract class PlayerBaseState
    {
        protected PlayerController Controller { get; private set; }

        public PlayerBaseState(PlayerController controller)
        {
            this.Controller = controller;
        }

        public abstract void OnEnterState();
        public abstract void OnUpdateState();
        public abstract void OnFixedUpdateState();
        public abstract void OnExitState();
    }
}
