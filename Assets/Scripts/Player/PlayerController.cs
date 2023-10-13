using Assets.Scripts.Camera;
using Assets.Scripts.Player.States;
using System.Collections;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

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

        private const float ADDITIONAL_GROUND_CHECK_DIST = 0.2f;

        [Header("Dodge")]
        [SerializeField] private float dodgeInvulnerableTime;
        public float DodgeInvulnerableTime { get { return dodgeInvulnerableTime; } }
        [SerializeField] private float dodgeForce;
        public float DodgeForce { get { return dodgeForce; } }

        [Header("Attack")]
        [SerializeField] private bool hasRock;


        public bool isGrounded;
        public bool isFalling;
        public bool isJumping;
        public bool isDodging;
        public bool canJump;

        private float horizontalInput;
        private float verticalInput;

        public Vector2 MoveInput { get; private set; }
        public Vector3 MoveDirection { get; private set; }
        public Rigidbody Rb { get; private set; }
        public Animator Anim { get; private set; }

        private PlayerStateMachine stateMachine;

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            Rb = GetComponent<Rigidbody>();
            Rb.freezeRotation = true;
            Anim = GetComponent<Animator>();
            canJump = true;
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
            isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + ADDITIONAL_GROUND_CHECK_DIST, groundLayer);
            if (isGrounded)
            {
                Rb.drag = groundDrag;
            }
            else
            {
                Rb.drag = 0;
            }
        }
        private void CalculateMoveDirection()
        {
            Vector3 viewDir = transform.position - new Vector3(mainCam.transform.position.x, transform.position.y, mainCam.transform.position.z);
            orientation.forward = viewDir.normalized;
            MoveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
            
        }

        private void Turn()
        {
            if (MoveDirection != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, MoveDirection.normalized, Time.deltaTime * rotationSpeed);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (isJumping && collision.gameObject.CompareTag("Ground"))
            {
                //Anim.SetBool("IsFalling", false);
                isFalling = false;
                ChangeState(PlayerStateName.Land);
                //Anim.SetTrigger("Idle");
            }
        }

        public void OnLandAnimEnd()
        {
            ChangeState(PlayerStateName.Idle);
        }
    }
}