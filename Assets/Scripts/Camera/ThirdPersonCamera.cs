using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Camera
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform orientation;
        [SerializeField] private Transform player;
        [SerializeField] private Transform playerObj;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float rotationSpeed;
        public float RotationSpeed { get { return rotationSpeed; } set { rotationSpeed = value; } }

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
        }

        private void LateUpdate()
        {
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 inputDir = orientation.forward * vertical + orientation.right * horizontal;

            if(inputDir !=Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * RotationSpeed);
            }
        }
    }
}