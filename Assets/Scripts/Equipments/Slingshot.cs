using UnityEngine;

public class Slingshot : MonoBehaviour {
    public GameObject rightElastic;
    public GameObject leftElastic;
    public GameObject rightLine;
    public GameObject leftLine;
    public GameObject leather;
    public GameObject leatherLine;
    private LineRenderer leftLineRenderer;
    private LineRenderer rightLineRenderer;

    public Transform leftPos;
    public Transform rightPos;
    private float pulled;
    private int i;
    private float z;

    void Awake ()
    {
        
        leftLineRenderer = leftPos.transform.GetComponent<LineRenderer>();
        rightLineRenderer = rightPos.transform.GetComponent<LineRenderer>();

        leftLineRenderer.positionCount = 2;
        rightLineRenderer.positionCount = 2;


        leftLineRenderer.SetWidth(0.04f, 0.04f);
        rightLineRenderer.SetWidth(0.04f, 0.04f);

        leftLineRenderer.enabled = false;
        rightLineRenderer.enabled = false;

        rightElastic.GetComponent<SkinnedMeshRenderer>().enabled = false;
        leftElastic.GetComponent<SkinnedMeshRenderer>().enabled = false;
        leather.GetComponent<SkinnedMeshRenderer>().enabled = false;
    }

    void Update()
    {
        leftLineRenderer.SetPosition(0, leftPos.position);
        rightLineRenderer.SetPosition(0, rightPos.position);
        leftLineRenderer.SetPosition(1, leather.transform.position);
        rightLineRenderer.SetPosition(1, leather.transform.position);
        
        if (Input.GetMouseButtonUp(0))
        {
            leftLineRenderer.enabled = false;
            rightLineRenderer.enabled = false;
        }
    }

    public void TurnLineRenderer(bool b)
    {
        leftLineRenderer.enabled = b;
        rightLineRenderer.enabled = b;
    }
}
