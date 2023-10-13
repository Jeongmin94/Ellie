using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateAirbourn : PlayerBaseState
    {
        Rigidbody rb;
        public PlayerStateAirbourn(PlayerController controller) : base(controller)
        {
            rb = Controller.Rb;
        }

        public override void OnEnterState()
        {
            Debug.Log("Airbourn");
            Controller.isFalling = true;
            Controller.Anim.SetBool("IsFalling", true);
        }

        public override void OnExitState()
        {
            Controller.isFalling = false;
            Controller.Anim.SetBool("IsFalling", false);
        }   

        public override void OnFixedUpdateState()
        {
            rb.AddForce(-rb.transform.up * Controller.AdditionalGravityForce, ForceMode.Force);
        }

        public override void OnUpdateState()
        {
        }
    }
}
