using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("마우스 감도")]
    [SerializeField] float mouseCoeff;
    [Header("이동 속도(실제 이동 속도는 Rigidbody 컴포넌트의 Speed를 확인)")]
    [SerializeField] float moveForce;
    [Header("달리기 계수(달리는 동안 이동 속도에 곱해집니다.)")]
    [SerializeField] float maxSprintCoeff;
   
    [Header("회피 힘(회피 시 바라보는 방향으로 가해지는 힘의 크기입니다.)")]
    [SerializeField] float rollingForce;

    [Header("기본 Fov")]
    [SerializeField] float defaultFov;
    [Header("조준 시 Fov")]
    [SerializeField] float zoomFov;
    [Header("달리기 Fov")]
    [SerializeField] float sprintFov;

    [Header("조준 시 카메라 줌 인 속도(조준할 시 fov가 감소하는 속도입니다.")]
    [SerializeField] float zoomFovReductionSpeed;
    [Header("조준 해제 시 카메라 줌 아웃 속도(조준 해제 시 fov가 회복되는 속도입니다.")]
    [SerializeField] float zoomOutFovRecoverySpeed;

    [Header("달리기 시 카메라 줌 아웃 속도(달릴 시 Fov가 증가하는 속도입니다.")]
    [SerializeField] float sprintFovIncreaseSpeed;
    [Header("달리기 종료 시 카메라 줌 인 속도(달리기를 멈출 시 fov가 회복되는 속도입니다.")]
    [SerializeField] float sprintEndFovRecoverySpeed;
    

    [Header("기타")]
    [SerializeField] Transform characterBody;
    [SerializeField] Transform cameraArm;
    [SerializeField] GameObject crossHair;
    [SerializeField] GameObject arrowInHand;
    [SerializeField] Transform shootPos;

    Ray shootRay;

    Animator animator;
    Rigidbody rb;

    private float rollStartTime;

    private bool isDead;
    private bool isRolling;
    private bool isSprinting;
    private bool isZooming;
    private float zoomCoeff;
    private float sprintCoeff;

    private Vector2 moveInput;


    void Start()
    {

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        crossHair.SetActive(false);
        zoomCoeff = 1f;
        sprintCoeff = 1f;
        arrowInHand.SetActive(false);


        isDead = false;

        shootRay = new Ray();
    }



    void Update()
    {
        if (isDead) return;
        Move();
        LookAround();
        FocusOnCamera();
        AnimControl();
        if (!isRolling && !isZooming && Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(Roll());

        Sprint();
        Zoom();
    }
    private void FixedUpdate()
    {

    }

    private void Zoom()
    {
        float fov = Camera.main.fieldOfView;
        if (Input.GetMouseButton(0))
        {

            if (isSprinting)
                EndSprint();
            if (!isZooming)
            {
                crossHair.SetActive(true);
                animator.SetBool("IsDrawing", true);
                isZooming = true;
                zoomCoeff = 0.1f;
            }
            fov -= Time.deltaTime * zoomFovReductionSpeed;
            fov = Mathf.Clamp(fov, zoomFov, defaultFov);
            Camera.main.fieldOfView = fov;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (arrowInHand.activeSelf)
                arrowInHand.SetActive(false);
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.OverDrawing"))
            {
                crossHair.SetActive(false);
                zoomCoeff = 1f;
                isZooming = false;
                animator.SetBool("IsDrawing", false);
                if (arrowInHand.activeSelf)
                    arrowInHand.SetActive(false);
            }
            else
            {
                animator.SetBool("IsDrawing", false);
            }
        }

    }

    public void ActivateArrowInHand()
    {
        arrowInHand.SetActive(true);
    }
    private void FocusOnCamera()
    {
        if (!isZooming) return;
        Vector3 cameraForward = cameraArm.transform.forward;
        characterBody.rotation = Quaternion.LookRotation(new Vector3(cameraForward.x, 0f, cameraForward.z));
    }
    public void Shoot()
    {
        crossHair.SetActive(false);
        zoomCoeff = 1f;
        isZooming = false;
        StartCoroutine(LooseZoom());
    }
    IEnumerator LooseZoom()
    {
        float fov = Camera.main.fieldOfView;
        while (fov < 60f)
        {
            fov += Time.deltaTime * zoomOutFovRecoverySpeed;
            Camera.main.fieldOfView = fov;
            yield return null;
        }
    }

    private void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartSprint();
            return;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            EndSprint();
            return;
        }
    }

    private void StartSprint()
    {
        isSprinting = true;
        sprintCoeff = maxSprintCoeff;
        animator.SetBool("Sprint", true);
    }

    private void EndSprint()
    {
        isSprinting = false;
        sprintCoeff = 1f;
        animator.SetBool("Sprint", false);
    }
    private void AnimControl()
    {
        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    private void Move()
    {
        if (isRolling) return;
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMoving = moveInput.magnitude != 0;
        if (isMoving)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            characterBody.forward = Vector3.Slerp(characterBody.forward, moveDir, 0.1f);
            //rb.AddForce(moveDir * moveForce * sprintCoeff * zoomCoeff);
            rb.velocity = moveDir.normalized * moveForce * sprintCoeff * zoomCoeff;

        }
    }
    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * mouseCoeff;
        Vector3 camAngle = cameraArm.rotation.eulerAngles;

        float x = camAngle.x - mouseDelta.y;
        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
        //cameraArm.position *= sprintCoeff;

        if (isZooming) return;
        float fov = Camera.main.fieldOfView;


        if (isSprinting)
        {
            fov += Time.deltaTime * sprintFovIncreaseSpeed;
            fov = Mathf.Clamp(fov, defaultFov, sprintFov);
            Camera.main.fieldOfView = fov;
        }
        else
        {
            fov -= Time.deltaTime * sprintEndFovRecoverySpeed;
            fov = Mathf.Clamp(fov, defaultFov, sprintFov);
            Camera.main.fieldOfView = fov;
        }
    }

    IEnumerator Roll()
    {
        isRolling = true;
        animator.SetTrigger("Roll");
        rollStartTime = Time.time;
        while (Time.time - rollStartTime < 0.5f)
        {
            //rb.AddForce(characterBody.forward * rollingForce * Time.deltaTime);
            rb.velocity = characterBody.forward * moveForce * sprintCoeff * zoomCoeff * rollingForce;
            yield return null;
        }
        isRolling = false;
    }
}
