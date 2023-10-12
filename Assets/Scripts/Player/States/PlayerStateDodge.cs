using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateDodge : PlayerBaseState
    {
        private readonly Rigidbody rb;
        private Vector3 dodgeDir;
        private float dodgeTime;
        private float beforeDrag;
        public PlayerStateDodge(PlayerController controller) : base(controller)
        {
            rb = Controller.Rb;
        }

        public override void OnEnterState()
        {
            beforeDrag = Controller.groundDrag;
            Controller.groundDrag = 0f;
            dodgeDir = Controller.MoveDirection.normalized;
            //Controller.cam.RotationSpeed = 100f;
            
            dodgeTime = 0f;
            rb.transform.GetComponentInChildren<Renderer>().material.color = Color.red;
        }

        public override void OnExitState()
        {
            Controller.groundDrag = beforeDrag;
            //Controller.cam.RotationSpeed = 10f;
            Controller.isDodging = false;
            rb.transform.GetComponentInChildren<Renderer>().material.color = Color.white;
        }

        public override void OnFixedUpdateState()
        {
            if(!Controller.isDodging) 
            { 
                Controller.isDodging = true;
                rb.velocity = Vector3.zero;
                rb.AddForce(dodgeDir * Controller.DodgeSpeed,ForceMode.VelocityChange);
            }
            //rb.velocity = dodgeDir * Controller.DodgeSpeed;
            dodgeTime += Time.fixedDeltaTime;
            if(dodgeTime > Controller.DodgeInvulnerableTime)
            {
                Controller.ChangeState(PlayerStateName.Idle);
            }
        }

        public override void OnUpdateState()
        {
        }
    }
}
