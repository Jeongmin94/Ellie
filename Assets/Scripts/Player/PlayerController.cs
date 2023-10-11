using System.Collections;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private Transform orientation;
        [SerializeField] private float jumpForce;
        [SerializeField] private float jumpCooldown;
        [SerializeField] private float airMultiplier;

        private bool canJump;

        [Header("Ground Check")]
        [SerializeField] private float playerHeight;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundDrag;
        [SerializeField] private float additionalGravityForce;
        private bool isGrounded;

        [Header("KeyBinds")]
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;

        private float horizontalInput;
        private float verticalInput;

        public Vector3 MoveDirection { get; private set; }
        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            canJump = true;
        }

        private void Update()
        {
            GetInput();
            CheckGround();
            ControlSpeed();
        }

        private void FixedUpdate()
        {
            CalculateMoveDirection();
            MovePlayer();
        }
        private void GetInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            if (Input.GetKey(jumpKey) && canJump && isGrounded)
            {
                canJump = false;
                StartCoroutine(Jump());
            }
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
        private void MovePlayer()
        {
            if (isGrounded)
                rb.AddForce(MoveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            else
            {
                rb.AddForce(MoveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
                rb.AddForce(-transform.up * additionalGravityForce, ForceMode.Force);
            }
        }
        private void ControlSpeed()
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }

        private IEnumerator Jump()
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            yield return new WaitForSeconds(jumpCooldown);
            ResetJump();
        }

        private void ResetJump()
        {
            canJump = true;
        }
    }
}