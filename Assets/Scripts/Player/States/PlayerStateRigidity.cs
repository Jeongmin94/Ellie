using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Player.States
{
    public class PlayerStateRigidity : PlayerBaseState
    {
        public PlayerStateRigidity(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
            Controller.Anim.SetTrigger("EnterRigidity");
            Controller.canTurn = false;
            Controller.SetTimeScale(1f);
            Controller.TurnOffAimCam();
            Controller.SetAimingAnimLayerToDefault();
            Controller.ActivateShootPos(false);
            Controller.isRigid = true;
        }

        public override void OnExitState()
        {
            Controller.Anim.SetTrigger("ExitRigidity");
            Controller.canTurn = true;
            Controller.isRigid = false;
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
        }
    }
}
