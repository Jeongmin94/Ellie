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

        public bool isGrounded;
        public bool isFalling;
        public bool isJumping;
        public bool isDodging;
        public bool canJump;
        public bool canTurn;

        private float horizontalInput;
        private float verticalInput;

        public Vector2 MoveInput { get; private set; }
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
            Anim.SetFloat("Velocity", Rb.velocity.magnitude);

            if (isJumping || isFalling)
                playerCollider.height = 0f;
            else
                playerCollider.height = 1.5f;
            //resetPlayer
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                transform.position = new Vector3(0f,1f,0f);
                ChangeState(PlayerStateName.Idle);
            }
        }
        private void FixedUpdate()
        {
            CalculateMoveDirection();
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
            if(CheckSlope())
            {
                Rb.AddForce(GetSlopeMoveDirection() * moveSpeed * MOVE_FORCE, ForceMode.Force);
            }
            else
            Rb.AddForce(MOVE_FORCE * moveSpeed * MoveDirection.normalized, ForceMode.Force);
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

            if(curIsGrounded != isGrounded)
            {
                if(curIsGrounded)
                {
                    //공중 -> 땅
                    ChangeState(PlayerStateName.Land);
                }
                else
                {
                    if(!Input.GetKey(KeyCode.Space))
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
            if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + ADDITIONAL_GROUND_CHECK_DIST))
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
                //playerObj.forward = MoveDirection.normalized;
            }
        }


        

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 20), "Player Status: " + stateMachine.CurrentStateName);
           
        }
    }
}