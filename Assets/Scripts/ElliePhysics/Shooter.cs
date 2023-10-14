using Assets.Scripts.Data.UI;
using Assets.Scripts.ElliePhysics.Utils;
using Assets.Scripts.Item;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Trajectory References")]
    [SerializeField] private Transform releasePosition;
    [SerializeField] private LineRenderer lineRenderer;

    [Header("Trajectory Configurations")]
    [Range(10, 25)]
    [SerializeField] private int linePoints = 10;
    [SerializeField] private float moveTime = 1.0f;

    [Header("Shooting Configurations")]
    [SerializeField] private float maxValidDistance;
    [SerializeField] private float referenceSpeed;

    [Header("Objects")]
    [SerializeField] private BaseStone stone;
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

    private Vector3[] CalculateTrajectoryPoints(Vector3 direction, float strength)
    {
        Vector3 startPosition = releasePosition.position;
        Vector3 startVelocity = direction * strength;

        Vector3[] points = new Vector3[linePoints + 1];

        float timeInterval = moveTime / (float)(linePoints + 1);
        int i = 0;

        points[0] = startPosition;
        for (float accInterval = timeInterval; accInterval < moveTime; accInterval += timeInterval)
        {
            i++;

            float time = accInterval;
            Vector3 currentPosition = startPosition + startVelocity * time;
            currentPosition.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2.0f * time * time);

            points[i] = currentPosition;
        }

        return points;
    }

    private void DrawTrajectory(Vector3 direction, float strength)
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = linePoints + 1;

        Vector3[] points = CalculateTrajectoryPoints(direction, strength);
        lineRenderer.SetPositions(points);

        float distance = 0.0f;
        Vector3 finalVelocity = Vector3.zero;

        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector3 current = points[i];
            Vector3 next = points[i + 1];
            Vector3 currentToNext = (next - current);

            finalVelocity = currentToNext;
            distance += (currentToNext).magnitude;
        }

        Vector3 dist = PhysicsUtil.GetDistance(direction * strength, Physics.gravity, moveTime);
        //Debug.Log($"예상 거리: {dist.magnitude}");
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

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            Vector3 launchDirection = (hit.point - releasePosition.position).normalized;
            float validDistance = ratio * maxValidDistance;
            if (Input.GetMouseButton(0))
            {
                float angle = Vector3.Angle(launchDirection, transform.forward);
                float strength = validDistance / moveTime * Mathf.Cos(Mathf.Deg2Rad * angle);

                Vector3 l = transform.forward * validDistance;

                startVelocity = launchDirection * strength;
#if UNITY_EDITOR
                DrawTrajectory(launchDirection, strength);
#endif

                if (!onCharge)
                    onCharge = true;
            }
            else if (Input.GetMouseButtonUp(0) && onCharge)
            {
                Debug.Log($"발사 속도: {startVelocity.magnitude}, 유효 사거리: {validDistance}");
                Debug.Log($"공식 변위: {(startVelocity * moveTime + 0.5f * moveTime * moveTime * Physics.gravity).magnitude}");

                ReleaseStone(launchDirection, startVelocity.magnitude, referenceSpeed, validDistance);

                onCharge = false;
                lineRenderer.enabled = false;
            }
        }
    }
}
