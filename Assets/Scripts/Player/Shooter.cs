using System;
using Assets.Scripts.Data.ActionData.Player;
using Assets.Scripts.ElliePhysics.Utils;
using Assets.Scripts.Item;
using Assets.Scripts.Item.Stone;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Combat;
using Channels.Components;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(LineRenderer))]
    public class Shooter : MonoBehaviour
    {
        [Header("Trajectory Configurations")]
        [SerializeField]
        private LineRenderer lineRenderer;

        [Range(10, 25)] [SerializeField] private int linePoints = 10;

        [Range(0.0f, 10.0f)] [SerializeField] private float calculatingTime = 1.0f;

        [Header("Shooting Configurations")]
        [SerializeField]
        private Transform releasePosition;

        [Range(10.0f, 100.0f)]
        [SerializeField]
        private float shootingPower = 25.0f;

        [SerializeField] private ChargingData chargingData;
        [SerializeField] private AimTargetData aimTargetData;

        // !TODO: stone을 가져와서 사용할 수 있도록 변경해야 함(인벤토리 시스템 추가 후 변경)
        // !TODO: 현재는 테스트 용도로 인스턴스 받아와 사용
        //[Header("Objects")] [SerializeField] private BaseStone stone;

        [SerializeField] private bool withPlayer = false;


        //public BaseStone Stone
        //{
        //    get { return stone; }
        //    set { stone = value; }
        //}

        public float ChargingRatio
        {
            get { return chargingRatio; }
        }

        public ChargingData ChargingData
        {
            get { return chargingData; }
        }

        private LayerMask trajectoryCollisionMask;
        private Vector3 aimTarget;
        private Vector3 launchDirection;
        private float chargingTime = 0.0f;
        private float chargingRatio = 0.0f;

       
        private void OnEnable()
        {
            SubscribeAction();
        }

        private void Start()
        {
            lineRenderer.enabled = false;

            SetLineRendererLayerMask();
        }
        

        private void SubscribeAction()
        {
            chargingData.ChargingValue.ValueChangeAction -= OnChangeChargingValue;
            chargingData.ChargingValue.ValueChangeAction += OnChangeChargingValue;

            InputManager.Instance.OnMouseAction -= OnMouseAction;
            InputManager.Instance.OnMouseAction += OnMouseAction;

            aimTargetData.TargetPosition.ValueChangeAction -= OnChangeAimTarget;
            aimTargetData.TargetPosition.ValueChangeAction += OnChangeAimTarget;
        }

        private void SetLineRendererLayerMask()
        {
            int layer = gameObject.layer;
            for (int i = 0; i < 32; i++)
            {
                if (!Physics.GetIgnoreLayerCollision(layer, i))
                {
                    trajectoryCollisionMask |= (1 << i);
                }
            }
        }
        public void Shoot(TicketMachine ticketMachine)
        {
            Debug.Log("Shoot");
            CombatPayload payload = new()
            {
                Type = CombatType.RequestStone,
                StoneSpawnPos = releasePosition.position,
                StoneDirection = launchDirection,
                StoneStrength = shootingPower * chargingRatio
            };
            ticketMachine.SendMessage(Channels.Type.ChannelType.Combat, payload);
            lineRenderer.enabled = false;
            chargingTime = 0.0f;
            chargingData.ChargingValue.Value = 0.0f;
        }
        //private void ReleaseStone(BaseStone stone, Vector3 direction, float strength)
        //{
        //    stone.SetPosition(releasePosition.position);
        //    stone.MoveStone(direction, strength);

        //}

        private void OnMouseAction()
        {
            if (Input.GetMouseButton(0))
            {
                chargingTime = Mathf.Clamp(chargingTime + Time.deltaTime / Time.timeScale, 0.0f,
                    chargingData.timeSteps[chargingData.timeSteps.Length - 1]);
                chargingData.ChargingValue.Value = chargingTime;

                launchDirection = CalculateDirection();
                DrawTrajectory(launchDirection, shootingPower * chargingRatio);
            }
            //else if (Input.GetMouseButtonUp(0))
            //{
            //    // shooting
            //    Shoot(launchDirection, shootingPower * chargingRatio);

            //    // after shooting
            //    lineRenderer.enabled = false;
            //    chargingTime = 0.0f;
            //    chargingData.ChargingValue.Value = 0.0f;
            //}
        }
        //public void Shoot(Poolable obj)
        //{
        //    // !TODO : SendMessage를 통해 StoneHatchery가 지정된 위치에서 돌 생성, 돌 발사 로직 실행
        //    BaseStone stone = obj as BaseStone;
        //    ReleaseStone(stone, launchDirection, shootingPower * chargingRatio);
        //    lineRenderer.enabled = false;
        //    chargingTime = 0.0f;
        //    chargingData.ChargingValue.Value = 0.0f;
        //}
        private Vector3 CalculateDirection()
        {
            Vector3 direction = Vector3.zero;
            if (withPlayer)
            {
                direction = (aimTarget - releasePosition.position).normalized;
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    direction = (hit.point - releasePosition.position).normalized;
                }
            }

            return direction;
        }

        private void OnChangeChargingValue(float value)
        {
            if (chargingData.timeSteps.Length == 0)
            {
                chargingRatio = 1.0f;
                return;
            }

            int count = Math.Min(chargingData.timeSteps.Length, chargingData.percentages.Length);
            ref float[] steps = ref chargingData.timeSteps;
            ref float[] percentages = ref chargingData.percentages;

            chargingRatio = Mathf.Lerp(percentages[0], percentages[count - 1], value / steps[count - 1]);
        }

        private void OnChangeAimTarget(Vector3 value)
        {
            aimTarget = value;
        }

        private void DrawTrajectory(Vector3 direction, float strength)
        {
            lineRenderer.enabled = true;

            Vector3[] points = PhysicsUtil.CalculateTrajectoryPoints(releasePosition.position, direction, strength,
                calculatingTime, linePoints, trajectoryCollisionMask);

            lineRenderer.positionCount = points.Length;
            lineRenderer.SetPositions(points);
        }
    }
}