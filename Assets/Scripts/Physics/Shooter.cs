using Assets.Scripts.Data.UI;
using Assets.Scripts.Item;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Trajectory References")]
    [SerializeField] private Transform releasePosition;
    [SerializeField] private LineRenderer lineRenderer;

    [Header("Trajectory Configurations")]
    [Range(10, 100)]
    [SerializeField] private int linePoints = 25;
    [Range(0.01f, 0.25f)]
    [SerializeField] private float timeBetweenPoints = 0.1f;

    [Header("Shooting Configurations")]
    [SerializeField] private float shootingPower;
    [SerializeField] private float maxValidDistance;
    [SerializeField] private float referenceSpeed;

    [Header("Objects")]
    [SerializeField] private BaseStone stone;
    [SerializeField] private SliderData sliderData;

    private LayerMask trajectoryCollisionMask;
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
        stone.MoveStone(direction, strength, referenceSpeed, validDistance);

        // stone의 시작 속도에서 서서히 referenceSpeed로 떨어짐
        // 처음 이동 시작 속도 => validDistance까지 이동하면서 referenceSpeed까지 떨어짐
    }

    private void DrawTrajectory(Vector3 direction, float strength)
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;

        Vector3 startPosition = releasePosition.position;
        Vector3 startVelocity = direction * strength;
        float distance = 0.0f;

        lineRenderer.SetPosition(0, startPosition);
        for (int i = 1; i < lineRenderer.positionCount; i++)
        {
            float time = i * timeBetweenPoints;
            Vector3 currentPosition = startPosition + startVelocity * time;
            currentPosition.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2.0f * time * time);

            Vector3 prevPosition = lineRenderer.GetPosition(i - 1);
            Vector3 prevToCurrent = (currentPosition - prevPosition);
            Ray ray = new Ray(prevPosition, prevToCurrent.normalized);
            if (Physics.Raycast(ray, out RaycastHit hit, prevToCurrent.magnitude, trajectoryCollisionMask))
            {
                distance += (hit.point - prevPosition).magnitude;
                lineRenderer.SetPosition(i, hit.point);
                lineRenderer.positionCount = i + 1;
                break;
            }
            else
            {
                distance += prevToCurrent.magnitude;
                lineRenderer.SetPosition(i, currentPosition);
            }
        }
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
        float strength = shootingPower * ratio;

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
#if UNITY_EDITOR
                DrawTrajectory((hit.point - releasePosition.position).normalized, strength);
#endif
            }

            if (!onCharge)
                onCharge = true;
        }
        else if (Input.GetMouseButtonUp(0) && onCharge)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                ReleaseStone((hit.point - releasePosition.position).normalized, strength, referenceSpeed, maxValidDistance * ratio);
                onCharge = false;
                lineRenderer.enabled = false;
            }
        }
    }
}
