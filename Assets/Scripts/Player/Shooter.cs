using System;
using Assets.Scripts.Managers;
using Channels.Components;
using Channels.Stone;
using Channels.Type;
using Channels.UI;
using Data.ActionData.Player;
using ElliePhysics.Utils;
using UI.Inventory.CategoryPanel;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(LineRenderer))]
    public class Shooter : MonoBehaviour
    {
        [Header("Trajectory Configurations")] [SerializeField]
        private LineRenderer lineRenderer;

        [Range(10, 25)] [SerializeField] private int linePoints = 10;

        [Range(0.0f, 10.0f)] [SerializeField] private float calculatingTime = 1.0f;

        [Header("Shooting Configurations")] [SerializeField]
        private Transform releasePosition;

        [Range(10.0f, 100.0f)] [SerializeField]
        private float shootingPower = 25.0f;

        [SerializeField] private ChargingData chargingData;
        [SerializeField] private AimTargetData aimTargetData;
        [SerializeField] private bool withPlayer;

        public bool isTargetingEnemy;
        private Vector3 aimTarget;
        private float chargingTime;

        private Vector3 lastPointOfTraj;
        private Vector3 launchDirection;

        private LayerMask trajectoryCollisionMask;


        public float ChargingRatio { get; private set; }

        public ChargingData ChargingData => chargingData;

        private void Start()
        {
            lineRenderer.enabled = false;

            SetLineRendererLayerMask();
        }

        private void OnEnable()
        {
            SubscribeAction();
        }

        public Vector3 LastPointOfTraj()
        {
            return lastPointOfTraj;
        }

        public void Init()
        {
            SubscribeAction();
        }

        private void SubscribeAction()
        {
            chargingData.ChargingValue.Subscribe(OnChangeChargingValue);

            InputManager.Instance.mouseAction -= OnMouseAction;
            InputManager.Instance.mouseAction += OnMouseAction;

            aimTargetData.TargetPosition.Subscribe(OnChangeAimTarget);
        }

        private void SetLineRendererLayerMask()
        {
            var layer = gameObject.layer;
            for (var i = 0; i < 32; i++)
            {
                if (!Physics.GetIgnoreLayerCollision(layer, i))
                {
                    trajectoryCollisionMask |= 1 << i;
                }
            }
        }

        public void Shoot(TicketMachine ticketMachine, int stoneIdx)
        {
            //해처리로 이벤트 보내기
            StoneEventPayload payload = new()
            {
                Type = StoneEventType.ShootStone,
                StoneSpawnPos = releasePosition.position,
                StoneDirection = launchDirection,
                StoneStrength = shootingPower * ChargingRatio,
                StoneIdx = stoneIdx
            };
            ticketMachine.SendMessage(ChannelType.Stone, payload);
            //UI에 이벤트 보내기
            UIPayload uIPayload = new()
            {
                uiType = UIType.Notify,
                actionType = ActionType.ConsumeSlotItem,
                groupType = GroupType.Stone
            };
            lineRenderer.enabled = false;
            chargingTime = 0.0f;
            chargingData.ChargingValue.Value = 0.0f;
        }

        private void OnMouseAction()
        {
            if (Input.GetMouseButton(0))
            {
                var ts = Time.timeScale == 0f ? 1 : Time.timeScale;
                chargingTime = Mathf.Clamp(chargingTime + Time.deltaTime / ts, 0.0f,
                    chargingData.timeSteps[chargingData.timeSteps.Length - 1]);
                chargingData.ChargingValue.Value = chargingTime;

                launchDirection = CalculateDirection();
                DrawTrajectory(launchDirection, shootingPower * ChargingRatio);
            }
        }

        private Vector3 CalculateDirection()
        {
            var direction = Vector3.zero;
            if (withPlayer)
            {
                direction = (aimTarget - releasePosition.position).normalized;
            }
            else
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
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
                ChargingRatio = 1.0f;
                return;
            }

            var count = Math.Min(chargingData.timeSteps.Length, chargingData.percentages.Length);
            ref var steps = ref chargingData.timeSteps;
            ref var percentages = ref chargingData.percentages;

            ChargingRatio = Mathf.Lerp(percentages[0], percentages[count - 1], value / steps[count - 1]);
        }

        private void OnChangeAimTarget(Vector3 value)
        {
            aimTarget = value;
        }

        private void DrawTrajectory(Vector3 direction, float strength)
        {
            lineRenderer.enabled = true;

            var points = PhysicsUtil.CalculateTrajectoryPoints(releasePosition.position, direction, strength,
                calculatingTime, linePoints, trajectoryCollisionMask);

            lastPointOfTraj = points.Length > 0 ? points[^1] : releasePosition.position;
            lineRenderer.positionCount = points.Length;
            lineRenderer.SetPositions(points);
        }

        public void ChangeLineRendererColor(bool _isTargeting)
        {
            //조준하지 않고 있다가 새롭게 조준
            if (_isTargeting && !isTargetingEnemy)
            {
                lineRenderer.material.color = Color.red;
                isTargetingEnemy = true;
            }

            if (!_isTargeting && isTargetingEnemy)
            {
                lineRenderer.material.color = Color.white;
                isTargetingEnemy = false;
            }
        }
    }
}