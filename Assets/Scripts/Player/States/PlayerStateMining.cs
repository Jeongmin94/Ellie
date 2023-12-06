using Assets.Scripts.InteractiveObjects;
using UnityEngine;

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
            LookOre();
            Controller.Pickaxe.gameObject.SetActive(true);
            Controller.Anim.SetBool("IsMining", true);
            Controller.GetComponent<PlayerInteraction>().isInteracting = true;
            Controller.GetComponent<PlayerInteraction>().DeactivateInteractiveUI();
        }

        public override void OnExitState()
        {
            Controller.SetAnimLayerToDefault(PlayerController.AnimLayer.Mining);
            Controller.Pickaxe.gameObject.SetActive(false);
            Controller.Anim.SetBool("IsMining", false);
            Controller.GetComponent<PlayerInteraction>().isInteracting = false;
            Controller.GetComponent<PlayerInteraction>().ActivateInteractiveUI();

        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
            Controller.IncreaseAnimLayerWeight(PlayerController.AnimLayer.Mining, 1);
            //입력이 들어오면 스테이트 탈출
            if (Controller.MoveInput.magnitude > 0)
            {
                Controller.ChangeState(PlayerStateName.Idle);
            }
            //점프 입력이나 공격 입력이 들어오면 스테이트 탈출
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Controller.ChangeState(PlayerStateName.Jump);
            }
            if (Input.GetMouseButtonDown(0))
            {
                Controller.ChangeState(PlayerStateName.Zoom);
            }
            if (curTime >= miningTime)
            {
                curTime = 0f;
                Mine();
            }
            else
            {
                curTime += Time.deltaTime;
            }
            if(!Controller.CurOre.canMine)
            {
                Controller.ChangeState(PlayerStateName.Idle);
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
            Debug.Log("Mine");
            int smithPower = UnityEngine.Random.Range(Controller.Pickaxe.MinSmithPower, Controller.Pickaxe.MaxSmithPower + 1);
            int damage = smithPower >= Controller.CurOre.hardness ? smithPower - Controller.CurOre.hardness : 0;
            //광석에 데미지 주기
            if (damage > 0)
            {
                Controller.CurOre.Smith(damage);
            }
            //Controller.Pickaxe.Durability--;
            //if (Controller.Pickaxe.Durability <= 0)
            //{
            //    Debug.Log("Pickaxe is Broken");
            //    Controller.ChangeState(PlayerStateName.Idle);
            //    Controller.isPickaxeAvailable = false;
            //}
        }
    }
}
