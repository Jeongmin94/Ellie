using CodiceApp;
using System.Collections;
using System.Collections.Generic;
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
        //i = 1;
        ////Starts with z = -2 so that the elastic line starts at the size of the elastic.
        //z = -2;
        ////If the ElasticManager script is not inside the slingshot, the value of pulled is set to - 7.
        //if (this.GetComponent<ElasticManager>() != null)
        //{
        //    //Getting elastic resistance value in the ElasticManager script.
        //    pulled = (this.GetComponent<ElasticManager>().elasticResistance - 8)/10f;
        //}
        //else
        //{
        //    pulled = -7/10f;
        //}
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
        //If you press the left mouse button, the elastic starts to stretch.
        //rightLine.SetActive(false);
        //leftLine.SetActive(false);
        
    }

    void Update()
    {
        leftLineRenderer.SetPosition(0, leftPos.position);
        rightLineRenderer.SetPosition(0, rightPos.position);
        leftLineRenderer.SetPosition(1, leather.transform.position);
        rightLineRenderer.SetPosition(1, leather.transform.position);
        //z -= 0.01f;
        //if (i == 1)
        //{
        //    //if the i(increment) is equal to one, it means that there is no metal sphere in the slingshot, then the sphere is created to be thrown next.
        //    //The metal sphere is parented to the slingshot, so that it can move with the slingshot.
        //    i = 0;                           
        //}

        ////Disables the elastic mesh renderer.

        if (Input.GetMouseButton(0))
        {
            //rightElasticLine.enabled = true;
            //leftElasticLine.enabled = true;
            //rightLine.SetActive(true);
            //leftLine.SetActive(true);
            //leftLineRenderer.SetPosition(0, leftPos.position);
            //rightLineRenderer.SetPosition(0, rightPos.position);
            //leftLineRenderer.SetPosition(1, leather.transform.position);
            //rightLineRenderer.SetPosition(1, leather.transform.position);
            ////z -= 0.01f;
            ////if (i == 1)
            ////{
            ////    //if the i(increment) is equal to one, it means that there is no metal sphere in the slingshot, then the sphere is created to be thrown next.
            ////    //The metal sphere is parented to the slingshot, so that it can move with the slingshot.
            ////    i = 0;                           
            ////}

            //////Disables the elastic mesh renderer.
            //rightElastic.GetComponent<SkinnedMeshRenderer>().enabled = false;
            //leftElastic.GetComponent<SkinnedMeshRenderer>().enabled = false;
            //leather.GetComponent<SkinnedMeshRenderer>().enabled = false;

            ////Activates the elastic line, so the movement becomes more fluid and beautiful.

            //leatherLine.GetComponent<SkinnedMeshRenderer>().enabled = true;

            //rightElasticLine = rightLine.transform.GetComponent<LineRenderer>();
            //leftElasticLine = leftLine.transform.GetComponent<LineRenderer>();

            //if (z >= pulled)
            //{
            //    //For the elastic to stretch the value of the z axis is increased to - 7 or pulled value, maximum of the stretch.
            //    rightElasticLine.SetPosition(1, new Vector3(0, 0, z));
            //    //The lines are growing and the value of the z axis is increased.
            //    leftElasticLine.SetPosition(1, new Vector3(0, 0, z));
            //    //Leather and metallic sphere follow the movement of the line.
            //    leather.transform.localPosition = new Vector3(-0.142f, 0.2286f, z + 0.12f);
            //    leatherLine.transform.localPosition = new Vector3(-0.142f, 0.2286f, z + 0.12f);
            //}
            //else
            //{
            //    //If the z axis value reaches the maximum -7 or pulled value, that value will remain and the slingshot elastic will be completely stretched.
            //    rightElasticLine.SetPosition(1, new Vector3(0, 0, pulled));
            //    leftElasticLine.SetPosition(1, new Vector3(0, 0, pulled));
            //    leather.transform.localPosition = new Vector3(-0.142f, 0.2286f, pulled + 0.12f);
            //    leatherLine.transform.localPosition = new Vector3(-0.142f, 0.2286f, pulled + 0.12f);
            //}

        }
        //When the left mouse button is released, the metallic sphere will be thrown and the elastic will be decompressed with the movement.
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("line renderer deactivate");
            leftLineRenderer.enabled = false;
            rightLineRenderer.enabled = false;
            //rightLine.SetActive(false);
            //leftLine.SetActive(false);
            ////i receives the value of one so that a new metallic sphere is created when the mouse is pressed again.
            //i = 1;

            ////Activates the elastic mesh renderer.
            //rightElastic.GetComponent<SkinnedMeshRenderer>().enabled = true;
            //leftElastic.GetComponent<SkinnedMeshRenderer>().enabled = true;
            //leather.GetComponent<SkinnedMeshRenderer>().enabled = true;

            ////Disables the elastic line.
            //rightLine.SetActive(false);
            //leftLine.SetActive(false);
            //leatherLine.GetComponent<SkinnedMeshRenderer>().enabled = false;

            ////Activates the sphere's gravity as soon as it is thrown.

            ////Adds a forward force on the rigidbody of the metallic sphere, depending on the amount that the elastic is stretched.
            ////The metal sphere is taken (parent = null) in the slingshot, so that the sphere stops moving with the slingshot and the camera.
            //z = -2;                      
        }
    }

    public void TurnLineRenderer(bool b)
    {
        leftLineRenderer.enabled = b;
        rightLineRenderer.enabled = b;
    }
}
