using Assets.Scripts.Data.ActionData.Player;
using Assets.Scripts.Equipments;
using Assets.Scripts.InteractiveObjects;
using Assets.Scripts.Managers;
using Assets.Scripts.Player.States;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Dialog;
using Channels.Type;
using Channels.UI;
using Cinemachine;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private const float MOVE_FORCE = 10f;
        private const float ADDITIONAL_GROUND_CHECK_DIST = 0.3f;
        public enum SlopeStat
        {
            Flat,
            Climable,
            CantClimb
        }
        public enum AnimLayer
        {
            Base,
            Aiming,
            Mining
        }
        [Header("Player references")]
        [SerializeField] private Transform playerObj;
        [SerializeField] private CapsuleCollider playerCollider;
        [SerializeField] private Transform orientation;
        private PlayerStatus playerStatus;


        [Header("Camera")]
        public Camera mainCam;
        public CinemachineVirtualCamera cinematicMainCam;
        public CinemachineVirtualCamera cinematicAimCam;
        public CinemachineVirtualCamera cinematicDialogCam;

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
        [SerializeField] private float lowerRayLength;
        [SerializeField] private float upperRayLength;

        [Header("Dodge")]
        [SerializeField] private float dodgeInvulnerableTime;
        [SerializeField] private float timeToDodgeAfterDown;

        [Header("ActionData")]
        [SerializeField] private AimTargetData aimTargetData;

        [Header("Attack")]
        [SerializeField] private GameObject slingshot;
        [SerializeField] private GameObject rightHand;
        public bool hasStone;
        public GameObject shooter;
        public GameObject MeleeAttackCollider;
        [SerializeField] private int curStoneIdx;
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

        public Transform aimTransform;

        public float zoomMultiplier;

        [SerializeField] private float recoilTime;
        [SerializeField] private GameObject stone;

        [Header("Mining")]
        [SerializeField] private float miningTime;
        [SerializeField] private Pickaxe pickaxe;
        public Pickaxe Pickaxe { get { return pickaxe; } }
        private Ore curOre = null;
        public bool isPickaxeAvailable = false;
        public Pickaxe.Tier curPickaxeTier;


        [Header("Boolean Properties")]
        public bool isGrounded;
        public bool isSprinting;
        public bool isFalling;
        public bool isJumping;
        public bool isDodging;
        public bool isZooming;
        public bool canMove;
        public bool canJump;
        public bool canTurn;
        public bool canAttack;
        public bool isRigid;

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
        public float TimeToDodgeAfterDown { get { return  timeToDodgeAfterDown; } }
        public Ore CurOre { get { return curOre; } }
        public float MiningTime { get { return miningTime; } }
        public bool IsPickaxeAvailable { get { return isPickaxeAvailable; } }
        public Vector2 MoveInput { get; private set; }
        public Vector3 MoveDirection { get; private set; }
        public Rigidbody Rb { get; private set; }
        public Animator Anim { get; private set; }
        public float AimingAnimLayerWeight { get; set; }
        public float RecoilTime { get { return recoilTime; } }
        public int CurStoneIdx { get { return curStoneIdx; } }

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

            ticketMachine.AddTickets(ChannelType.Combat, ChannelType.Stone, ChannelType.UI, ChannelType.Dialog);
            ticketMachine.RegisterObserver(ChannelType.UI, OnNotifyAction);
            ticketMachine.RegisterObserver(ChannelType.Dialog, GetComponent<PlayerQuest>().SetIsPlaying);
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
            //SetColliderHeight();
            ResetPlayerPos();
            SetMovingAnim();
            stateMachine?.UpdateState();
            GrabSlingshotLeather();
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
            canMove = true;
            canJump = true;
            canAttack = true;
            isGrounded = true;
            isRigid = false;
            initialFixedDeltaTime = Time.fixedDeltaTime;
            TurnOffAimCam();
            TurnOffDialogCam();

            stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepRayLower.transform.position.y + stepHeight,
                stepRayUpper.transform.position.z);
            SetCurOre(null);
            Pickaxe.gameObject.SetActive(false);
            TurnOffSlingshot();
            TurnOffMeleeAttackCollider();

        }
        private void SetMovingAnim()
        {
            if (stateMachine.CurrentStateName == PlayerStateName.Conversation || !canMove)
            {
                Anim.SetFloat("Input Magnitude", 0f);

                return;
            }
            inputMagnitude = Mathf.Clamp01(MoveInput.magnitude);
            if (isSprinting)
            {
                inputMagnitude *= 1.5f;
            }

            Anim.SetFloat("Input Magnitude", inputMagnitude, 0.1f, Time.deltaTime);
        }
        private void ResetPlayerPos()
        {
            //Test용
            //if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    transform.position = new Vector3(0f, 1f, 0f);
            //    ChangeState(PlayerStateName.Idle);
            //}
        }
        public void SetColliderHeight(float colliderHeight)
        {

            //if (isJumping || isFalling)
            //    playerCollider.height = 1f;
            //else
            //    playerCollider.height = 1.5f;
            playerCollider.height = colliderHeight;
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
            PlayerStateAirborne playerStateAirbourn = new(this);
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
            PlayerStateDown playerStateDown = new(this);
            stateMachine.AddState(PlayerStateName.Down, playerStateDown);
            PlayerStateGetUp playerStateGetUp = new(this);
            stateMachine.AddState(PlayerStateName.GetUp, playerStateGetUp);
            PlayerStateConversation playerStateConversation = new(this);
            stateMachine.AddState(PlayerStateName.Conversation, playerStateConversation);
            PlayerStateMeleeAttack playerStateMeleeAttack = new(this);
            stateMachine.AddState(PlayerStateName.MeleeAttack, playerStateMeleeAttack);

        }
        public void MovePlayer(float moveSpeed)
        {
            if (!canMove) return;
            switch (CheckSlope())
            {
                // !TODO : 경사로에서 흘러내리는 문제 수정
                case SlopeStat.Flat:
                    ClimbStep();
                    Rb.AddForce(MOVE_FORCE * moveSpeed * MoveDirection.normalized, ForceMode.Force);
                    break;
                case SlopeStat.Climable:
                    ClimbStep();
                    Rb.AddForce(GetSlopeMoveDirection() * moveSpeed * MOVE_FORCE, ForceMode.Force);
                    break;
                case SlopeStat.CantClimb:
                    break;
            }
        }

        private void ClimbStep()
        {
            bool flag = false;
            RaycastHit[] hitLower = Physics.RaycastAll(stepRayLower.transform.position, PlayerObj.TransformDirection(Vector3.forward), lowerRayLength, groundLayer);
            if (hitLower.Any())
            {
                RaycastHit[] hitUpper = Physics.RaycastAll(stepRayUpper.transform.position, PlayerObj.TransformDirection(Vector3.forward), upperRayLength, groundLayer);
                if (!hitUpper.Any())
                {
                    //Rb.position += new Vector3(0f, stepSmooth * Time.fixedDeltaTime, 0f);
                    flag = true;
                }
            }

            RaycastHit[] hitLower45 = Physics.RaycastAll(stepRayLower.transform.position, PlayerObj.TransformDirection(Vector3.forward), lowerRayLength, groundLayer);
            if (hitLower45.Any())
            {
                RaycastHit[] hitUpper45 = Physics.RaycastAll(stepRayUpper.transform.position, PlayerObj.TransformDirection(Vector3.forward), upperRayLength, groundLayer);
                if (!hitUpper45.Any())
                {
                    //Rb.position += new Vector3(0f, stepSmooth * Time.fixedDeltaTime, 0f);
                    flag = true;
                }
            }
            RaycastHit[] hitLowerMinus45 = Physics.RaycastAll(stepRayLower.transform.position, PlayerObj.TransformDirection(-1.5f, 0, 1), lowerRayLength, groundLayer);
            if (hitLowerMinus45.Any())
            {
                RaycastHit[] hitUpperMinus45 = Physics.RaycastAll(stepRayUpper.transform.position, PlayerObj.TransformDirection(-1.5f, 0, 1), upperRayLength, groundLayer);
                if (!hitUpperMinus45.Any())
                {
                    //Rb.position += new Vector3(0f, stepSmooth * Time.fixedDeltaTime, 0f);
                    flag = true;
                }
            }

            if (flag)
            {
                Rb.AddForce(Vector3.up * 60f, ForceMode.Force);
                //Rb.position += new Vector3(0f, stepSmooth * Time.fixedDeltaTime, 0f);
            }

        }
        public void Jump()
        {
            StartCoroutine(JumpCoroutine());
        }
        private IEnumerator JumpCoroutine()
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

        public void ChangeState(PlayerStateName nextStateName, StateInfo info)
        {
            if (stateMachine.CurrentStateName == PlayerStateName.Dead) return;
            stateMachine.ChangeState(nextStateName, info);
        }
        private void GetInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            MoveInput = new Vector2(horizontalInput, verticalInput);
        }
        private void CheckGround()
        {
            if (isRigid) return;
            // !TODO : 플레이어의 State들에서 처리할 수 있도록 수정
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

        private SlopeStat CheckSlope()
        {
            // 평지 : 0, 경사로 : 1, 올라갈 수 없는 경사로 : -1
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + ADDITIONAL_GROUND_CHECK_DIST))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                //Debug.Log("CurrAngle : " + angle.ToString());
                if (angle < 10f)
                    return SlopeStat.Flat;
                if (angle > maxSlopeAngle)
                    return SlopeStat.CantClimb;
                else
                    return SlopeStat.Climable;
            }
            return 0;
        }
        private Vector3 GetSlopeMoveDirection()
        {
            return Vector3.ProjectOnPlane(MoveDirection, slopeHit.normal).normalized;
        }
        private void CalculateMoveDirection()
        {
            //orientation의 forward를 플레이어가 카메라를 향하는 방향으로 갱신합니다
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
            cinematicMainCam.gameObject.SetActive(false);
            cinematicAimCam.gameObject.SetActive(true);
        }
        public void TurnOffAimCam()
        {
            cinematicMainCam.gameObject.SetActive(true);
            cinematicAimCam.gameObject.SetActive(false);

        }

        public void TurnOnDialogCam()
        {
            cinematicDialogCam.gameObject.SetActive(true);
            cinematicMainCam.gameObject.SetActive(false);
        }

        public void TurnOffDialogCam()
        {
            cinematicMainCam.gameObject.SetActive(true);
            cinematicDialogCam.gameObject.SetActive(false);
        }
        public void SetTimeScale(float expectedTimeScale)
        {
            //현재 timeScale과 fixedDeltatime을 파라미터의 값에 맞게 변경합니다
            Time.timeScale = expectedTimeScale;
            Time.fixedDeltaTime = initialFixedDeltaTime * Time.timeScale;
        }
        public void IncreaseAnimLayerWeight(AnimLayer layer, float weight)
        {
            // 애니메이션의 레이어의 Weight를 증가시킵니다. State의 Update에서 호출합니다
            float curWeight = Anim.GetLayerWeight((int)layer);
            if (Mathf.Equals(curWeight, weight)) return;

            float AnimLayerWeightChangeSpeed = 2 / mainCam.GetComponent<CinemachineBrain>().m_DefaultBlend.BlendTime;
            if (curWeight < weight)
            {
                curWeight += AnimLayerWeightChangeSpeed * Time.deltaTime / Time.timeScale;
            }
            Anim.SetLayerWeight((int)layer, curWeight);
        }

        public void SetAnimLayerToDefault(AnimLayer layer)
        {
            StartCoroutine(SetAnimToDefaultlayerCoroutine((int)layer));
        }
        private IEnumerator SetAnimToDefaultlayerCoroutine(int layer)
        {
            float AnimLayerWeightChangeSpeed = 2 / mainCam.GetComponent<CinemachineBrain>().m_DefaultBlend.BlendTime;
            float curWeight = Anim.GetLayerWeight(layer);
            while (curWeight > 0)
            {
                curWeight -= AnimLayerWeightChangeSpeed * Time.deltaTime / Time.timeScale;
                Anim.SetLayerWeight(layer, curWeight);
                yield return null;
            }
            Anim.SetLayerWeight(layer, 0);
        }

        public void ActivateShootPos(bool value)
        {
            shooter.SetActive(value);
        }

        public void Aim()
        {
            Ray shootRay = mainCam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(shootRay, out hit, Mathf.Infinity, ~layerToIgnore))
            {
                AimTarget = hit.point;
            }
            else
            {
                AimTarget = shootRay.origin + 50f * shootRay.direction.normalized;
            }
            aimTransform.position = shootRay.origin + 5f * shootRay.direction.normalized;
            cinematicAimCam.LookAt = aimTransform;
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

        public void StartConversation()
        {
            TurnOnDialogCam();
            cinematicDialogCam.LookAt = GetComponent<PlayerInteraction>().interactiveObject.transform;
            ChangeState(PlayerStateName.Conversation);
            GetComponent<PlayerInteraction>().isInteracting = true;
        }

        public void EndConversation()
        {
            TurnOffDialogCam();
            GetComponent<PlayerInteraction>().isInteracting = false;
            ChangeState(PlayerStateName.Idle);
        }

        public void TurnOnSlingshot()
        {
            slingshot.SetActive(true);
        }

        public void TurnOffSlingshot()
        {
            slingshot.SetActive(false);
        }

        public void TurnOnMeleeAttackCollider()
        {
            MeleeAttackCollider.SetActive(true);
        }

        public void TurnOffMeleeAttackCollider()
        {
            MeleeAttackCollider.SetActive(false);
        }

        public void GrabSlingshotLeather()
        {
            slingshot.GetComponent<Slingshot>().leather.transform.position = rightHand.transform.position;
        }

        public void TurnSlingshotLineRenderer(bool b)
        {
            slingshot.GetComponent<Slingshot>().TurnLineRenderer(b);
        }
        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 20), "Player Status: " + stateMachine.CurrentStateName);
            GUI.Label(new Rect(10, 20, 200, 20), "Current Time Scale : " + Time.timeScale);
            GUI.Label(new Rect(10, 30, 200, 20), "Current Fixed Delta Time : " + Time.fixedDeltaTime);
        }

        private void OnDrawGizmos()
        {
            Color rayColor = Color.red;
            Gizmos.color = rayColor;

            Gizmos.DrawRay(stepRayLower.transform.position, stepRayLower.transform.forward * lowerRayLength);

            Gizmos.DrawRay(stepRayUpper.transform.position, stepRayUpper.transform.forward * upperRayLength);
        }

        private void OnNotifyAction(IBaseEventPayload payload)
        {
            //UI페이로드 처리 로직입니다
            UIPayload uiPayload = payload as UIPayload;
            //인벤토리 닫는 이벤트일 경우
            if (uiPayload.actionType == ActionType.ClickCloseButton)
            {
                GetComponent<PlayerInventory>().OnInventoryToggle();
            }
            if (uiPayload.actionType != ActionType.SetPlayerProperty) return;
            switch (uiPayload.groupType)
            {
                case UI.Inventory.GroupType.Item:
                    break;
                case UI.Inventory.GroupType.Stone:
                    Debug.Log("!!");
                    hasStone = !uiPayload.isStoneNull;
                    if (uiPayload.itemData != null)
                    {
                        curStoneIdx = uiPayload.itemData.index;
                    }
                    else
                        curStoneIdx = 0;
                    break;
                case UI.Inventory.GroupType.Etc:
                    break;
                default:
                    break;
            }
        }

        public void GetPickaxe(int pickaxeIdx)
        {
            UIPayload payload = new()
            {
                uiType = UIType.Notify,
                groupType = UI.Inventory.GroupType.Etc,
                slotAreaType = UI.Inventory.SlotAreaType.Item,
                actionType = ActionType.AddSlotItem,
                itemData = DataManager.Instance.GetIndexData<PickaxeData, PickaxeDataParsingInfo>(pickaxeIdx)
            };
            ticketMachine.SendMessage(ChannelType.UI, payload);

            DialogPayload dPayload = DialogPayload.Play("곡괭이를 얻었다!!");
            dPayload.canvasType = DialogCanvasType.Simple;

            ticketMachine.SendMessage(ChannelType.Dialog, dPayload);
            isPickaxeAvailable = true;
            curPickaxeTier = (Pickaxe.Tier)pickaxeIdx;
            pickaxe.LoadPickaxeData((Pickaxe.Tier)pickaxeIdx);
        }
    }
}
