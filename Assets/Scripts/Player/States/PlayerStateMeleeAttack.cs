namespace Assets.Scripts.Player.States
{
    public class PlayerStateMeleeAttack : PlayerBaseState
    {
        PlayerStatus status;
        public PlayerStateMeleeAttack(PlayerController controller) : base(controller)
        {
            status = Controller.GetComponent<PlayerStatus>();
        }
        public override void OnEnterState()
        {
            //Controller.TurnOnMeleeAttackCollider();
            Controller.Anim.SetBool("IsMeleeAttacking", true);
            Controller.TurnOnSlingshot();
            status.ConsumeStamina(20f);
            status.isRecoveringStamina = false;
        }

        public override void OnExitState()
        {
            status.isRecoveringStamina = true;

            Controller.TurnOffSlingshot();
            Controller.Anim.SetBool("IsMeleeAttacking", false);
            Controller.TurnOffMeleeAttackCollider();

        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
        }
    }
}
