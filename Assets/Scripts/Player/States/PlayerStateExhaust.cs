using Assets.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateExhaust : PlayerBaseState
    {
        private float moveSpeed;
        public PlayerStateExhaust(PlayerController controller) : base(controller)
        {

        }

        public override void OnEnterState()
        {
            Controller.canTurn = true;

            moveSpeed = Controller.WalkSpeed * 0.5f;
            Controller.PlayerStatus.isRecoveringStamina = true;
            Controller.Anim.SetBool("IsExhausted", true);
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "ellie_sound7");
        }

        public override void OnExitState()
        {
            Controller.canTurn = true;
            Controller.Anim.SetBool("IsExhausted", false);

        }

        public override void OnFixedUpdateState()
        {
            // !TODO : 탈진 상태에서 움직일 때와 아닐 때에 대한 애니메이션 처리를 진행해야 합니다
            Controller.MovePlayer(moveSpeed);
        }

        public override void OnUpdateState()
        {
            if(Controller.PlayerStatus.Stamina >= 50)
            {
                Controller.ChangeState(PlayerStateName.Idle);
            }
        }
    }
}
