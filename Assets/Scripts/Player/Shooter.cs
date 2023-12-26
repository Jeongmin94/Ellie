using Assets.Scripts.Channels.Item;
using Assets.Scripts.Data.ActionData.Player;
using Assets.Scripts.ElliePhysics.Utils;
using Assets.Scripts.Managers;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using System;
using System.Linq;
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
        [SerializeField] private bool withPlayer = false;

        public bool isTargetingEnemy;

        private Vector3 lastPointOfTraj;
        public Vector3 LastPointOfTraj() => lastPointOfTraj;
        

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
            int layer = gameObject.layer;
            for (int i = 0; i < 32; i++)
            {
                if (!Physics.GetIgnoreLayerCollision(layer, i))
                {
                    trajectoryCollisionMask |= (1 << i);
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
                StoneStrength = shootingPower * chargingRatio,
                StoneIdx = stoneIdx
            };
            ticketMachine.SendMessage(ChannelType.Stone, payload);
            //UI에 이벤트 보내기
            UIPayload uIPayload = new()
            {
                uiType = UIType.Notify,
                actionType = ActionType.ConsumeSlotItem,
                groupType = UI.Inventory.GroupType.Stone
            };
            lineRenderer.enabled = false;
            chargingTime = 0.0f;
            chargingData.ChargingValue.Value = 0.0f;
        }
        private void OnMouseAction()
        {
            if (Input.GetMouseButton(0))
            {
                float ts = Time.timeScale == 0f ? 1 : Time.timeScale;
                chargingTime = Mathf.Clamp(chargingTime + Time.deltaTime / ts, 0.0f,
                    chargingData.timeSteps[chargingData.timeSteps.Length - 1]);
                chargingData.ChargingValue.Value = chargingTime;

                launchDirection = CalculateDirection();
                DrawTrajectory(launchDirection, shootingPower * chargingRatio);
            }
        }

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