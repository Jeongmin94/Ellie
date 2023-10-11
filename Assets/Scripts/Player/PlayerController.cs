using Assets.Scripts.Player.States;
using System.Collections;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        private float moveSpeed;

        [SerializeField] public float walkSpeed;
        public float WalkSpeed { get { return walkSpeed; } }
        [SerializeField] public float sprintSpeed;
        public float SprintSpeed { get { return sprintSpeed; } }

        [SerializeField] public Transform orientation;
        [SerializeField] public float jumpForce;
        [SerializeField] public float additionalJumpForce;
        [SerializeField] public float jumpCooldown;
        [SerializeField] public float airMultiplier;


        [Header("Ground Check")]
        [SerializeField] public float playerHeight;
        [SerializeField] public LayerMask groundLayer;
        [SerializeField] public float groundDrag;
        [SerializeField] public float additionalGravityForce;

        public bool isGrounded;
        public bool isFalling;
        public bool isJumping;
        public bool canJump;

        [Header("KeyBinds")]
        [SerializeField] public KeyCode jumpKey = KeyCode.Space;
        [SerializeField] public KeyCode sprintKey = KeyCode.LeftShift;


        public float horizontalInput;
        public float verticalInput;

        public Vector2 MoveInput { get; private set; }
        public Vector3 MoveDirection { get; private set; }
        public Rigidbody rb;

        private PlayerStateMachine stateMachine;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            canJump = true;
            InitStateMachine();
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

        }
        private void Update()
        {
            GetInput();
            CheckGround();
            stateMachine?.UpdateState();

        }

        private void FixedUpdate()
        {
            CalculateMoveDirection();
            stateMachine?.FixedUpdateState();
        }
        public void MovePlayer(float moveSpeed)
        {
            rb.AddForce(MoveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        public void JumpPlayer()
        {
            StartCoroutine(Jump());
        }

        private IEnumerator Jump()
        {
            canJump = false;
            float jumpInputTime = 0;
     
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            while (jumpInputTime < 0.5f && Input.GetKey(KeyCode.Space))
            {
                Debug.Log(jumpInputTime);
                jumpInputTime += Time.deltaTime;
                rb.AddForce(transform.up * additionalJumpForce, ForceMode.Force);
                yield return null;
            }
            isFalling = true;
            yield return new WaitForSeconds(jumpCooldown - jumpInputTime);
            canJump = true;
            //착지 스테이트로 전이인데 일단 Idle로 보내자
            //Controller.ChangeState(PlayerStateName.Idle);
        }

        public void Fall()
        {
            rb.AddForce(-transform.up * additionalGravityForce, ForceMode.Force);
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
            isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
            if (isGrounded)
            {
                rb.drag = groundDrag;
            }
            else
            {
                rb.drag = 0;
            }
        }
        private void CalculateMoveDirection()
        {
            MoveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(isJumping && collision.gameObject.CompareTag("Ground"))
            {
                ChangeState(PlayerStateName.Idle);
            }
        }
    }
}