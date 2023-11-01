using Assets.Scripts.Player.States;
using System.Collections;
using Assets.Scripts.Data.ActionData.Player;
using UnityEngine;
using Cinemachine;
using Assets.Scripts.InteractiveObjects;
using Assets.Scripts.Equipments;
using Assets.Scripts.StatusEffects;
using Channels.Components;
using Assets.Scripts.Utils;
using Channels.Type;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private const float MOVE_FORCE = 10f;
        private const float ADDITIONAL_GROUND_CHECK_DIST = 0.3f;

        [Header("Player references")]
        [SerializeField] private Transform playerObj;
        [SerializeField] private CapsuleCollider playerCollider;
        [SerializeField] private Transform orientation;
        private PlayerStatus playerStatus;


        [Header("Camera")]
        public GameObject mainCam;
        public GameObject cinematicMainCam;
        public GameObject cinematicAimCam;

        [Header("Move")]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float dodgeSpeed;

        [Header("Rotate")]
        [SerializeField] private float rotationSpeed;

        [Header("Jump")]
        [SerializeField] private float jumpForce;
        [SerializeField] private float additionalJumpForce;
        [SerializeField] private float jumpCooldown;
        [SerializeField] private float maximumAdditionalJumpInputTime;
        [SerializeField] private float additionalGravityForce;


        [Header("Ground Check")]
        [SerializeField] private float playerHeight;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask layerToIgnore;
        [SerializeField] public float groundDrag;
        [SerializeField] private float landStateDuration;


        [Header("Slope")]
        public float maxSlopeAngle;
        private RaycastHit slopeHit;

        [Header("Getting Over Step")]
        [SerializeField] private GameObject stepRayUpper;
        [SerializeField] private GameObject stepRayLower;
        [SerializeField] float stepHeight;
        [SerializeField] float stepSmooth;

        [Header("Dodge")]
        [SerializeField] private float dodgeInvulnerableTime;

        [Header("ActionData")]
        [SerializeField] private AimTargetData aimTargetData;

        [Header("Attack")]
        [SerializeField] private bool hasRock;
        public GameObject shooter;
        private Vector3 aimTarget;
        public Vector3 AimTarget
        {
            get { return aimTarget; }
            set
            {
                aimTargetData.TargetPosition.Value = value;
                aimTarget = value;
            }
        }
        public float zoomMultiplier;

        [SerializeField] private float recoilTime;
        [SerializeField] private GameObject stone;

        [Header("Mining")]
        [SerializeField] private float miningTime;
        [SerializeField] private Pickaxe pickaxe;
        public Pickaxe Pickaxe { get { return pickaxe; } }
        private Ore curOre = null;

        [Header("Boolean Properties")]
        public bool isGrounded;
        public bool isSprinting;
        public bool isFalling;
        public bool isJumping;
        public bool isDodging;
        public bool isZooming;
        public bool canJump;
        public bool canTurn;

        private float initialFixedDeltaTime;
        private float horizontalInput;
        private float verticalInput;


        public Transform PlayerObj { get { return playerObj; } }
        public PlayerStatus PlayerStatus
        {
            get { return playerStatus; }
            set { playerStatus = value; }
        }
        public float WalkSpeed { get { return walkSpeed; } }
        public float SprintSpeed { get { return sprintSpeed; } }
        public float DodgeSpeed { get { return dodgeSpeed; } }
        public float AdditionalJumpForce { get { return additionalJumpForce; } }
        public float MaximumJumpInputTime { get { return maximumAdditionalJumpInputTime; } }
        public float AdditionalGravityForce { get { return additionalGravityForce; } }
        public float LandStateDuration { get { return landStateDuration; } }
        public float DodgeInvulnerableTime { get { return dodgeInvulnerableTime; } }
        public Ore CurOre { get { return curOre; } }
        public float MiningTime { get { return miningTime; } }
        public Vector2 MoveInput { get; private set; }
        public Vector3 MoveDirection { get; private set; }
        public Rigidbody Rb { get; private set; }
        public Animator Anim { get; private set; }
        public float AimingAnimLayerWeight { get; set; }
        public float RecoilTime { get { return recoilTime; } }
        public GameObject Stone
        {
            get { return stone; }
            set { stone = value; }
        }


        private float inputMagnitude;

        private PlayerStateMachine stateMachine;

        private TicketMachine ticketMachine;
        public TicketMachine TicketMachine { get { return ticketMachine; } }

        private void Awake()
        {
            Rb = GetComponent<Rigidbody>();
            Anim = GetComponent<Animator>();
            playerStatus = GetComponent<PlayerStatus>();
            InitTicketMachine();
            //stateMachine.CurrentState.
        }
        private void InitTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Combat, ChannelType.Item);
        }

        private void Start()
        {
            //커서 락
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            //스테이트 머신 초기화
            InitStateMachine();

            //변수 초기화
            InitVariables();

            //ShootPos 꺼주기
            ActivateShootPos(false);
        }
        private void Update()
        {
            GetInput();
            CheckGround();
            Turn();
            SetColliderHeight();
            ResetPlayerPos();
            SetMovingAnim();
            stateMachine?.UpdateState();
        }
        private void FixedUpdate()
        {
            CalculateMoveDirection();
            AddAdditionalGravityForce();
            stateMachine?.FixedUpdateState();
        }

        private void InitVariables()
        {
            Rb.freezeRotation = true;
            canJump = true;
            isGrounded = true;
            initialFixedDeltaTime = Time.fixedDeltaTime;
            cinematicAimCam.SetActive(false);

            stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight,
                stepRayUpper.transform.position.z);
            SetCurOre(null);
            Pickaxe.gameObject.SetActive(false);
        }
        private void SetMovingAnim()
        {
            inputMagnitude = Mathf.Clamp01(MoveInput.magnitude);
            if (isSprinting)
            {
                inputMagnitude *= 1.5f;
            }
            Anim.SetFloat("Input Magnitude", inputMagnitude, 0.1f, Time.deltaTime);
        }
        private void ResetPlayerPos()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                transform.position = new Vector3(0f, 1f, 0f);
                ChangeState(PlayerStateName.Idle);
            }
        }
        private void SetColliderHeight()
        {
            if (isJumping || isFalling)
                playerCollider.height = 0f;
            else
                playerCollider.height = 1.5f;
        }
        private void AddAdditionalGravityForce()
        {
            Rb.AddForce(-Rb.transform.up * AdditionalGravityForce, ForceMode.Force);

        }
        private void InitStateMachine()
        {
            PlayerStateIdle playerStateIdle = new(this);
            stateMachine = new PlayerStateMachine(PlayerStateName.Idle, playerStateIdle);

            PlayerStateWalk playerStateWalk = new(this);
            stateMachine.AddState(PlayerStateName.Walk, playerStateWalk);
            PlayerStateSprint playerStateSprint = new(this);
            stateMachine.AddState(PlayerStateName.Sprint, playerStateSprint);
            PlayerStateJump playerStateJump = new(this);
            stateMachine.AddState(PlayerStateName.Jump, playerStateJump);
            PlayerStateDodge playerStateDodge = new(this);
            stateMachine.AddState(PlayerStateName.Dodge, playerStateDodge);
            PlayerStatusAirborne playerStateAirbourn = new(this);
            stateMachine.AddState(PlayerStateName.Airborne, playerStateAirbourn);
            PlayerStateLand playerStateLand = new(this);
            stateMachine.AddState(PlayerStateName.Land, playerStateLand);
            PlayerStateZoom playerStateZoom = new(this);
            stateMachine.AddState(PlayerStateName.Zoom, playerStateZoom);
            PlayerStateCharging playerStateCharging = new(this);
            stateMachine.AddState(PlayerStateName.Charging, playerStateCharging);
            PlayerStateShoot playerStateShoot = new(this);
            stateMachine.AddState(PlayerStateName.Shoot, playerStateShoot);
            PlayerStateMining playerStateMining = new(this);
            stateMachine.AddState(PlayerStateName.Mining, playerStateMining);
            PlayerStateExhaust playerStateExhaust = new(this);
            stateMachine.AddState(PlayerStateName.Exhaust, playerStateExhaust);
            PlayerStateRigidity playerStateRigidity = new(this);
            stateMachine.AddState(PlayerStateName.Rigidity, playerStateRigidity);
            PlayerStateDead playerStateDead = new(this);
            stateMachine.AddState(PlayerStateName.Dead, playerStateDead);
        }
        public void MovePlayer(float moveSpeed)
        {
            if (CheckSlope())
            {
                //Rb.AddForce(GetSlopeMoveDirection() * moveSpeed * MOVE_FORCE, ForceMode.Force);
            }
            else
            {
                ClimbStep();
                Rb.AddForce(MOVE_FORCE * moveSpeed * MoveDirection.normalized, ForceMode.Force);
            }
        }

        private void ClimbStep()
        {
            RaycastHit hitLower;
            if (Physics.Raycast(stepRayLower.transform.position,
                PlayerObj.TransformDirection(Vector3.forward), out hitLower, 0.1f, groundLayer))
            {
                RaycastHit hitUpper;
                if (!Physics.Raycast(stepRayUpper.transform.position,
                    PlayerObj.TransformDirection(Vector3.forward), out hitUpper, 0.2f, groundLayer))
                {
                    Rb.position += new Vector3(0f, stepSmooth * Time.fixedDeltaTime, 0f);
                }
            }

            RaycastHit hitLower45;
            if (Physics.Raycast(stepRayLower.transform.position,
                PlayerObj.TransformDirection(1.5f, 0, 1), out hitLower45, 0.1f, groundLayer))
            {

                RaycastHit hitUpper45;
                if (!Physics.Raycast(stepRayUpper.transform.position,
                    PlayerObj.TransformDirection(1.5f, 0, 1), out hitUpper45, 0.2f, groundLayer))
                {
                    Rb.position += new Vector3(0f, stepSmooth * Time.fixedDeltaTime, 0f);
                }
            }

            RaycastHit hitLowerMinus45;
            if (Physics.Raycast(stepRayLower.transform.position,
                PlayerObj.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, 0.1f, groundLayer))
            {

                RaycastHit hitUpperMinus45;
                if (!Physics.Raycast(stepRayUpper.transform.position,
                    PlayerObj.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, 0.2f, groundLayer))
                {
                    Rb.position += new Vector3(0f, stepSmooth * Time.fixedDeltaTime, 0f);
                }
            }
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
            if (stateMachine.CurrentStateName == PlayerStateName.Dead) return;
            stateMachine.ChangeState(nextStateName);
        }
        private void GetInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            Anim.SetFloat("Vertical Input", verticalInput);
            Anim.SetFloat("Horizontal Input", horizontalInput);
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
                        ChangeState(PlayerStateName.Airborne);
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
                return angle > maxSlopeAngle && angle != 0;
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
                PlayerObj.forward = Vector3.Slerp(PlayerObj.forward, MoveDirection.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        public void TurnOnAimCam()
        {
            cinematicMainCam.SetActive(false);
            cinematicAimCam.SetActive(true);
        }
        public void TurnOffAimCam()
        {
            cinematicMainCam.SetActive(true);
            cinematicAimCam.SetActive(false);
        }
        public void SetTimeScale(float expectedTimeScale)
        {

            Time.timeScale = expectedTimeScale;
            Time.fixedDeltaTime = initialFixedDeltaTime * Time.timeScale;
        }
        public void SetAimingAinmLayerWeight(float weight)
        {
            float AnimLayerWeightChangeSpeed = 2 / mainCam.GetComponent<CinemachineBrain>().m_DefaultBlend.BlendTime;
            if (AimingAnimLayerWeight < weight)
            {
                AimingAnimLayerWeight += AnimLayerWeightChangeSpeed * Time.deltaTime / Time.timeScale;
            }
            Anim.SetLayerWeight(1, AimingAnimLayerWeight);
        }

        public void SetAimingAnimLayerToDefault()
        {
            StartCoroutine(SetAnimToDefaultlayerCoroutine());
        }
        private IEnumerator SetAnimToDefaultlayerCoroutine()
        {
            float AnimLayerWeightChangeSpeed = 2 / mainCam.GetComponent<CinemachineBrain>().m_DefaultBlend.BlendTime;
            while (AimingAnimLayerWeight > 0)
            {
                AimingAnimLayerWeight -= AnimLayerWeightChangeSpeed * Time.deltaTime / Time.timeScale;
                Anim.SetLayerWeight(1, AimingAnimLayerWeight);
                yield return null;
            }
            Anim.SetLayerWeight(1, 0);
        }

        public void ActivateShootPos(bool value)
        {
            shooter.SetActive(value);
        }

        public void Aim()
        {
            Ray shootRay = mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(shootRay, out hit, Mathf.Infinity, ~layerToIgnore))
            {
                AimTarget = hit.point;
            }
            else
            {
                AimTarget = shootRay.origin + 50f * shootRay.direction.normalized;
            }
        }
        public void LookAimTarget()
        {
            Vector3 directionToTarget = AimTarget - PlayerObj.position;
            directionToTarget.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            PlayerObj.rotation = Quaternion.Slerp(PlayerObj.rotation, targetRotation, rotationSpeed * Time.deltaTime / Time.timeScale);
        }

        public void SetCurOre(Ore ore)
        {
            curOre = ore;
        }

        public PlayerStateName GetCurState()
        {
            return stateMachine.CurrentStateName;
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 20), "Player Status: " + stateMachine.CurrentStateName);
            GUI.Label(new Rect(10, 20, 200, 20), "Current Time Scale : " + Time.timeScale);
            GUI.Label(new Rect(10, 30, 200, 20), "Current Fixed Delta Time : " + Time.fixedDeltaTime);
        }
    }
}
