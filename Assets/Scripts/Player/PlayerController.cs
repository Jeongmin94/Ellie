using Assets.Scripts.Player.States;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject mainCam;
        public Transform playerObj;
        public float rotationSpeed;

        public GameObject cinematicMainCam;
        public GameObject cinematicAimCam;
        [SerializeField] private float walkSpeed;
        public float WalkSpeed { get { return walkSpeed; } }
        [SerializeField] private float sprintSpeed;
        public float SprintSpeed { get { return sprintSpeed; } }
        [SerializeField] private float dodgeSpeed;
        public float DodgeSpeed { get { return dodgeSpeed; } }

        private const float MOVE_FORCE = 10f;

        [SerializeField] private Transform orientation;
        [Header("Jump")]
        [SerializeField] private float jumpForce;
        [SerializeField] private float additionalJumpForce;
        public float AdditionalJumpForce { get { return additionalJumpForce; } }
        [SerializeField] private float jumpCooldown;
        [SerializeField] private float maximumAdditionalJumpInputTime;
        public float MaximumJumpInputTime { get { return maximumAdditionalJumpInputTime; } }

        [SerializeField] private float landStateDuration;
        public float LandStateDuration { get { return landStateDuration; } }


        [Header("Ground Check")]
        [SerializeField] private float playerHeight;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] public float groundDrag;
        [SerializeField] private float additionalGravityForce;
        public float AdditionalGravityForce { get { return additionalGravityForce; } }

        private const float ADDITIONAL_GROUND_CHECK_DIST = 0.3f;

        [Header("Dodge")]
        [SerializeField] private float dodgeInvulnerableTime;
        public float DodgeInvulnerableTime { get { return dodgeInvulnerableTime; } }
        [SerializeField] private float dodgeForce;
        public float DodgeForce { get { return dodgeForce; } }

        [Header("Attack")]
        [SerializeField] private bool hasRock;

        [Header("Slope")]
        public float maxSlopeAngle;
        private RaycastHit slopeHit;

        [Header("Getting Over Step")]
        [SerializeField] private GameObject stepRayUpper;
        [SerializeField] private GameObject stepRayLower;
        [SerializeField] float stepHeight;
        [SerializeField] float stepSmooth;

        public bool isGrounded;
        public bool isSprinting;
        public bool isFalling;
        public bool isJumping;
        public bool isDodging;
        public bool canJump;
        public bool canTurn;

        private float horizontalInput;
        private float verticalInput;

        public Vector2 MoveInput { get; private set; }
        public float inputMagnitude;
        public Vector3 MoveDirection { get; private set; }
        public Rigidbody Rb { get; private set; }
        public Animator Anim { get; private set; }

        public CapsuleCollider playerCollider;

        private PlayerStateMachine stateMachine;

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            Rb = GetComponent<Rigidbody>();
            Rb.freezeRotation = true;
            Anim = GetComponent<Animator>();
            canJump = true;
            isGrounded = true;
            InitStateMachine();
            cinematicAimCam.SetActive(false);

            //getting over step
            stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight,
                stepRayUpper.transform.position.z);

        }
        private void Update()
        {
            GetInput();
            CheckGround();
            Turn();
            stateMachine?.UpdateState();
            //Aim Camera Test
            if (Input.GetMouseButtonDown(0))
            {
                cinematicMainCam.gameObject.SetActive(false);
                cinematicAimCam.SetActive(true);
            }
            if (Input.GetMouseButtonUp(0))
            {
                cinematicMainCam.gameObject.SetActive(true);
                cinematicAimCam.SetActive(false);
            }
            //moving blend tree
            //Anim.SetFloat("Velocity", Rb.velocity.magnitude);
            inputMagnitude = Mathf.Clamp01(MoveInput.magnitude);
            if (isSprinting)
            {
                inputMagnitude *= 1.5f;
            }
            Anim.SetFloat("Input Magnitude", inputMagnitude, 0.1f, Time.deltaTime);

            if (isJumping || isFalling)
                playerCollider.height = 0f;
            else
                playerCollider.height = 1.5f;
            //resetPlayer
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                transform.position = new Vector3(0f, 1f, 0f);
                ChangeState(PlayerStateName.Idle);
            }
        }
        private void FixedUpdate()
        {
            CalculateMoveDirection();
            //ClimbStep();
            stateMachine?.FixedUpdateState();
        }
        private void InitStateMachine()
        {
            PlayerStateIdle playerStateIdle = new PlayerStateIdle(this);
            stateMachine = new PlayerStateMachine(PlayerStateName.Idle, playerStateIdle);

            PlayerStateWalk playerStateWalk = new PlayerStateWalk(this);
            stateMachine.AddState(PlayerStateName.Walk, playerStateWalk);
            PlayerStateSprint playerStateSprint = new PlayerStateSprint(this);
            stateMachine.AddState(PlayerStateName.Sprint, playerStateSprint);
            PlayerStateJump playerStateJump = new PlayerStateJump(this);
            stateMachine.AddState(PlayerStateName.Jump, playerStateJump);
            PlayerStateDodge playerStateDodge = new PlayerStateDodge(this);
            stateMachine.AddState(PlayerStateName.Dodge, playerStateDodge);
            PlayerStateAirbourn playerStateAirbourn = new PlayerStateAirbourn(this);
            stateMachine.AddState(PlayerStateName.Airbourn, playerStateAirbourn);
            PlayerStateLand playerStateLand = new PlayerStateLand(this);
            stateMachine.AddState(PlayerStateName.Land, playerStateLand);
        }
        public void MovePlayer(float moveSpeed)
        {
            if (CheckSlope())
            {
                Rb.AddForce(GetSlopeMoveDirection() * moveSpeed * MOVE_FORCE, ForceMode.Force);
            }
            else
            {
                ClimbStep();
                Rb.AddForce(MOVE_FORCE * moveSpeed * MoveDirection.normalized, ForceMode.Force);
            }
        }

        private void ClimbStep()
        {

            RaycastHit lowerHit;
            if (Physics.Raycast(stepRayLower.transform.position, playerObj.transform.forward,
                out lowerHit, 0.1f, groundLayer))
            {
                Debug.Log("stair detected");
                RaycastHit hitUpper;
                if (!Physics.Raycast(stepRayUpper.transform.position, playerObj.transform.forward,
                    out hitUpper, 0.2f, groundLayer))
                {
                    Debug.Log("Getting over Stair");
                    Rb.position -= new Vector3(0f, -stepSmooth * Time.fixedDeltaTime, 0f);
                }
            }
            Debug.DrawLine(stepRayLower.transform.position, stepRayLower.transform.position +
                playerObj.TransformDirection(Vector3.forward) * 0.1f, Color.green);
            Debug.DrawLine(stepRayUpper.transform.position, stepRayUpper.transform.position +
                playerObj.TransformDirection(Vector3.forward) * 0.2f, Color.red);

            //RaycastHit hitLower;
            //if (Physics.Raycast(stepRayLower.transform.position,
            //    playerObj.TransformDirection(Vector3.forward), out hitLower, 0.1f, groundLayer))
            //{
            //    RaycastHit hitUpper;
            //    if (!Physics.Raycast(stepRayUpper.transform.position,
            //        playerObj.TransformDirection(Vector3.forward), out hitUpper, 0.2f, groundLayer))
            //    {
            //        Rb.position += new Vector3(0f, stepSmooth * Time.fixedDeltaTime, 0f);
            //    }
            //}

            //RaycastHit hitLower45;
            //if (Physics.Raycast(stepRayLower.transform.position,
            //    playerObj.TransformDirection(1.5f, 0, 1), out hitLower45, 0.1f, groundLayer))
            //{

            //    RaycastHit hitUpper45;
            //    if (!Physics.Raycast(stepRayUpper.transform.position,
            //        playerObj.TransformDirection(1.5f, 0, 1), out hitUpper45, 0.2f, groundLayer))
            //    {
            //        Rb.position += new Vector3(0f, stepSmooth * Time.fixedDeltaTime, 0f);
            //    }
            //}

            //RaycastHit hitLowerMinus45;
            //if (Physics.Raycast(stepRayLower.transform.position,
            //    playerObj.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, 0.1f, groundLayer))
            //{

            //    RaycastHit hitUpperMinus45;
            //    if (!Physics.Raycast(stepRayUpper.transform.position,
            //        playerObj.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, 0.2f, groundLayer))
            //    {
            //        Rb.position += new Vector3(0f, stepSmooth * Time.fixedDeltaTime, 0f);
            //    }
            //}
        }
        public void JumpPlayer()
        {
            StartCoroutine(Jump());
        }
        private IEnumerator Jump()
        {
            canJump = false;

            Rb.velocity = new Vector3(Rb.velocity.x, 0f, Rb.velocity.z);
            Rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            yield return new WaitForSeconds(jumpCooldown);
            canJump = true;
        }
        public void ChangeState(PlayerStateName nextStateName)
        {
            stateMachine.ChangeState(nextStateName);
        }
        private void GetInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            MoveInput = new Vector2(horizontalInput, verticalInput);
        }
        private void CheckGround()
        {
            bool curIsGrounded = Physics.Raycast(transform.position,
                Vector3.down, playerHeight * 0.5f + ADDITIONAL_GROUND_CHECK_DIST, groundLayer);

            if (curIsGrounded != isGrounded)
            {
                if (curIsGrounded)
                {
                    //공중 -> 땅
                    ChangeState(PlayerStateName.Land);
                }
                else
                {
                    if (!Input.GetKey(KeyCode.Space))
                    {
                        //점프 중이 아닌 상태 : 땅 -> 공중
                        ChangeState(PlayerStateName.Airbourn);
                    }
                }
                isGrounded = curIsGrounded;
            }
            if (isGrounded)
            {
                Rb.drag = groundDrag;
            }
            else
            {
                Rb.drag = 0;
            }
        }

        private bool CheckSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + ADDITIONAL_GROUND_CHECK_DIST))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
            }
            return false;
        }

        private Vector3 GetSlopeMoveDirection()
        {
            return Vector3.ProjectOnPlane(MoveDirection, slopeHit.normal).normalized;
        }
        private void CalculateMoveDirection()
        {
            Vector3 viewDir = transform.position - new Vector3(mainCam.transform.position.x, transform.position.y, mainCam.transform.position.z);
            orientation.forward = viewDir.normalized;
            MoveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        }

        private void Turn()
        {
            if (!canTurn) return;
            if (MoveDirection != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, MoveDirection.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 20), "Player Status: " + stateMachine.CurrentStateName);

        }
    }
}
