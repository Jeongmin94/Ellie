using UnityEngine;

public class SplineWalker : MonoBehaviour
{
    public BezierSpline spline;

    public float duration;

    public bool lookForward;

    public SplineWalkerMode mode;
    private bool goingForward = true;

    public float Progress { get; private set; }

    private void Update()
    {
        if (goingForward)
        {
            Progress += Time.deltaTime / duration;
            if (Progress > 1f)
            {
                if (mode == SplineWalkerMode.Once)
                {
                    Progress = 1f;
                }
                else if (mode == SplineWalkerMode.Loop)
                {
                    Progress -= 1f;
                }
                else
                {
                    Progress = 2f - Progress;
                    goingForward = false;
                }
            }
        }
        else
        {
            Progress -= Time.deltaTime / duration;
            if (Progress < 0f)
            {
                Progress = -Progress;
                goingForward = true;
            }
        }

        var position = spline.GetPoint(Progress);
        transform.localPosition = position;
        if (lookForward)
        {
            transform.LookAt(position + spline.GetDirection(Progress));
        }
    }
}