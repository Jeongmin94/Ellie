using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private Transform orientation;

        [Header("Ground Check")]
        public float playerHeight;
        public LayerMask groundLayer;
        private bool isGrounded;
        public float groundDrag;

        private float horizontalInput;
        private float verticalInput;

        public Vector3 MoveDirection { get; private set; }
        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
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
        }
        private void GetInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }

        private void CheckGround()
        {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
            if (isGrounded)
            {
                rb.drag = groundDrag;
            }
            else
                rb.drag = 0;
        }
        private void CalculateMoveDirection()
        {
            MoveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
            rb.AddForce(MoveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
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
    }
}