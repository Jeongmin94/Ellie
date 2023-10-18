using Assets.Scripts.Data.ActionData.Player;
using Assets.Scripts.ElliePhysics.Utils;
using Assets.Scripts.Item;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Shooter : MonoBehaviour
{
    [Header("Trajectory Configurations")]
    [SerializeField]
    private Transform releasePosition;
    [SerializeField] private LineRenderer lineRenderer;
    [Range(10, 25)]
    [SerializeField] private int linePoints = 10;

    [Header("Shooting Configurations")]
    [Range(10.0f, 100.0f)]
    [SerializeField] private float shootingPower = 25.0f;
    [SerializeField] private float maxChargingTime;

    [Header("Objects")]
    // !TODO: stone을 가져와서 사용할 수 있도록 변경해야 함
    // !TODO: 현재는 테스트 용도로 인스턴스 받아와 사용
    [SerializeField] private BaseStone stone;
    [SerializeField] private ChargingData chargingData;

    public BaseStone Stone
    {
        get { return stone; }
        set { stone = value; }
    }

    private LayerMask trajectoryCollisionMask;
    private Vector3 startVelocity = Vector3.zero;
    private float chargingTime = 0.0f;
    private bool onCharge = false;

    private void Start()
    {
        lineRenderer.enabled = false;

        SetLayerMask();

        chargingData.ChargingValue.OnChange.Subscribe(OnChangeChargingValue);
        InputManager.Instance.OnMouseAction.Subscribe(OnMouseAction);
    }

    private void SetLayerMask()
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

    private void ReleaseStone(Vector3 direction, float strength)
    {
        stone.SetPosition(releasePosition.position);
        stone.MoveStone(direction, strength);
    }

    private void DrawTrajectory(Vector3 direction, float strength)
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = linePoints + 1;

        Vector3[] points =
            PhysicsUtil.CalculateTrajectoryPoints(releasePosition.position, direction, strength, 1.0f, linePoints, trajectoryCollisionMask);
        lineRenderer.SetPositions(points);
    }


    private void OnMouseAction()
    {
        if (Input.GetMouseButton(0))
        {
            chargingTime += Time.deltaTime / Time.timeScale;
            chargingTime = Mathf.Clamp(chargingTime + Time.deltaTime / Time.timeScale, 0.0f, maxChargingTime);
        }
        else if (Input.GetMouseButtonUp(0))
        {
        }
        chargingData.ChargingValue.Value = chargingTime / maxChargingTime;
    }

    // !TODO: AimTarget을 외부에서 주입 받아 사용할 수 있도록 변경
    // !TODO: min charging value 설정(0.5)

    private void OnChangeChargingValue(float value)
    {
        float ratio = 0.0f;
        if (0.0f <= value && value < 0.33f)
        {
            ratio = Mathf.Lerp(0.0f, 0.33f, value / 0.33f);
        }
        else if (0.33f <= value && value < 0.66f)
        {
            ratio = Mathf.Lerp(0.33f, 0.66f, (value - 0.33f) / 0.33f);
        }
        else if (0.66f <= value && value < 0.99f)
        {
            ratio = Mathf.Lerp(0.66f, 0.99f, (value - 0.66f) / 0.33f);
        }
        else if (0.99f <= value)
        {
            ratio = 1.0f;
        }


        Vector3 AimTarget = GetComponentInParent<PlayerController>().AimTarget;
        Vector3 launchDirection = (AimTarget - releasePosition.position).normalized;

        //Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        //if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (Input.GetMouseButton(0))
            {

                //startVelocity = PhysicsUtil.CalculateInitialVelocity(launchDirection, moveTime, validDistance);
                // Debug.Log($"direction: {launchDirection}, start: {startVelocity.normalized}");

#if UNITY_EDITOR
                DrawTrajectory(startVelocity.normalized, startVelocity.magnitude);
#endif
                if (!onCharge)
                    onCharge = true;
            }
            else if (Input.GetMouseButtonUp(0) && onCharge)
            {
                Debug.Log($"시작 속도: {startVelocity.magnitude}, {startVelocity}");
                // expected value

                ReleaseStone(startVelocity.normalized, startVelocity.magnitude);

                onCharge = false;
                lineRenderer.enabled = false;
            }
        }
    }
}