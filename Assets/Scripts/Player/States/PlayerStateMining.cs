using Assets.Scripts.InteractiveObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateMining : PlayerBaseState
    {
        private Ore curOre;
        private float miningTime;
        private float curTime;

        public PlayerStateMining(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
            curOre = Controller.CurOre;
            miningTime = Controller.MiningTime;
            curTime = 0f;
            Controller.Anim.SetLayerWeight(2, 1);
            LookOre();
            Controller.Pickaxe.SetActive(true);
            Controller.Anim.SetBool("IsMining", true);
        }

        public override void OnExitState()
        {
            Controller.Anim.SetLayerWeight(2, 0);
            Controller.Pickaxe.SetActive(false);
            Controller.Anim.SetBool("IsMining", false);
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
            //입력이 들어오면 스테이트 탈출
            if(Controller.MoveInput.magnitude>0)
            {
                Controller.ChangeState(PlayerStateName.Idle);
            }
            //점프 입력이나 공격 입력이 들어오면 스테이트 탈출
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Controller.ChangeState(PlayerStateName.Jump);
            }
            if(Input.GetMouseButtonDown(0))
            {
                Controller.ChangeState(PlayerStateName.Zoom);
            }
            if(curTime>=miningTime)
            {
                curTime = 0f;
                Mine();
            }
            else
            {
                curTime += Time.deltaTime;
            }
        }

        private void LookOre()
        {
            Vector3 directionToTarget = curOre.transform.position - Controller.PlayerObj.position;
            directionToTarget.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            Controller.PlayerObj.rotation = targetRotation;
        }
        private void Mine()
        {
            Debug.Log("Mining!");
        }
    }
}
