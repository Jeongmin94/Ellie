using Assets.Scripts.Data.UI;
using Assets.Scripts.ElliePhysics.Utils;
using Assets.Scripts.Item;
using Assets.Scripts.Player;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Trajectory References")]
    [SerializeField]
    private Transform releasePosition;

    [SerializeField] private LineRenderer lineRenderer;

    [Header("Trajectory Configurations")]
    [Range(10, 25)]
    [SerializeField]
    private int linePoints = 10;

    [SerializeField] private float moveTime = 1.0f;

    [Header("Shooting Configurations")]
    [SerializeField]
    private float maxValidDistance;

    [SerializeField] private float referenceSpeed;

    [Header("Objects")][SerializeField] private BaseStone stone;
    [SerializeField] private SliderData sliderData;

    private LayerMask trajectoryCollisionMask;
    private Vector3 startVelocity = Vector3.zero;
    private bool onCharge = false;

    private void Start()
    {
        lineRenderer.enabled = false;

        int layer = gameObject.layer;
        for (int i = 0; i < 32; i++)
        {
            if (!Physics.GetIgnoreLayerCollision(layer, i))
            {
                trajectoryCollisionMask |= (1 << i);
            }
        }

        sliderData.SliderValue.OnChange -= OnChangeSliderValue;
        sliderData.SliderValue.OnChange += OnChangeSliderValue;
    }

    private void ReleaseStone(Vector3 direction, float strength, float referenceSpeed, float validDistance)
    {
        stone.SetPosition(releasePosition.position);
        stone.MoveStone(direction, strength);
        stone.ValidateStoneMovement(moveTime, referenceSpeed, validDistance);
    }

    private void DrawTrajectory(Vector3 direction, float strength)
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = linePoints + 1;

        Vector3[] points =
            PhysicsUtil.CalculateTrajectoryPoints(releasePosition.position, direction, strength, moveTime, linePoints);
        lineRenderer.SetPositions(points);

        float distance = 0.0f;
        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector3 current = points[i];
            Vector3 next = points[i + 1];
            Vector3 currentToNext = (next - current);

            distance += (currentToNext).magnitude;
        }

        Vector3 dist = PhysicsUtil.GetDistance(direction * strength, Physics.gravity, moveTime);
    }

    private void OnChangeSliderValue(float value)
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
            float validDistance = ratio * maxValidDistance;
            if (Input.GetMouseButton(0))
            {

                startVelocity = PhysicsUtil.CalculateInitialVelocity(launchDirection, moveTime, validDistance);
                // Debug.Log($"direction: {launchDirection}, start: {startVelocity.normalized}");

#if UNITY_EDITOR
                Debug.DrawRay(releasePosition.position, launchDirection * validDistance, Color.blue, 1.0f);
                DrawTrajectory(startVelocity.normalized, startVelocity.magnitude);
#endif
                if (!onCharge)
                    onCharge = true;
            }
            else if (Input.GetMouseButtonUp(0) && onCharge)
            {
                Debug.Log($"시작 속도: {startVelocity.magnitude}, {startVelocity}");
                // expected value
                float distance = PhysicsUtil.GetDistance(startVelocity, Physics.gravity, moveTime).magnitude;
                Vector3 vel = PhysicsUtil.GetVelocity(startVelocity, Physics.gravity, moveTime);

                Debug.Log($"예상 이동 거리: {distance}");
                Debug.Log($"예상 속도 {moveTime}s : {vel.magnitude}");
                Debug.Log($"기대 속도: {referenceSpeed}");

                ReleaseStone(startVelocity.normalized, startVelocity.magnitude, referenceSpeed, validDistance);

                onCharge = false;
                lineRenderer.enabled = false;
            }
        }
    }
}