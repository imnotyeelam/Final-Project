using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        [Header("Movement")]
        public float walkSpeed = 5f;
        public float runSpeed = 9f;
        public float jumpSpeed = 5f;
        public float gravityMultiplier = 2f;
        public float stickToGroundForce = 10f;
        public float stepInterval = 5f;
        public float runStepLengthen = 0.7f;
        private Vector2 input;
        private Vector3 moveDir = Vector3.zero;
        private float stepCycle;
        private float nextStep;
        private bool isWalking = true;
        private bool previouslyGrounded;
        private bool jumping;

        [Header("Camera & Mouse Look")]
        public Camera playerCamera;
        public MouseLook mouseLook = new MouseLook();
        private Vector3 originalCameraPos;

        [Header("Audio")]
        public AudioClip[] footstepSounds;
        public AudioClip jumpSound;
        public AudioClip landSound;
        private AudioSource audioSource;

        private CharacterController controller;
        private CollisionFlags collisionFlags;

        private void Start()
        {
            controller = GetComponent<CharacterController>();

            if (playerCamera == null)
                playerCamera = Camera.main;

            originalCameraPos = playerCamera.transform.localPosition;

            audioSource = GetComponent<AudioSource>();

            mouseLook.Init(transform, playerCamera.transform);
        }

        private void Update()
        {
            RotateView();

            if (!previouslyGrounded && controller.isGrounded)
            {
                PlayLandingSound();
                moveDir.y = 0f;
                jumping = false;
            }

            if (!controller.isGrounded && !jumping && previouslyGrounded)
            {
                moveDir.y = 0f;
            }

            previouslyGrounded = controller.isGrounded;

            // Check for jump input
            if (!jumping && Input.GetButtonDown("Jump") && controller.isGrounded)
            {
                moveDir.y = jumpSpeed;
                PlayJumpSound();
                jumping = true;
            }
        }

        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);

            Vector3 desiredMove = transform.forward * input.y + transform.right * input.x;

            // Adjust for surface normal
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, controller.radius, Vector3.down, out hitInfo,
                controller.height / 2f);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            moveDir.x = desiredMove.x * speed;
            moveDir.z = desiredMove.z * speed;

            if (controller.isGrounded)
            {
                moveDir.y = -stickToGroundForce;
            }
            else
            {
                moveDir += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
            }

            collisionFlags = controller.Move(moveDir * Time.fixedDeltaTime);

            ProgressStepCycle(speed);
        }

        private void PlayFootstepAudio()
        {
            if (!controller.isGrounded || footstepSounds == null || footstepSounds.Length == 0)
                return;

            int n = Random.Range(1, footstepSounds.Length);
            audioSource.PlayOneShot(footstepSounds[n]);
            footstepSounds[n] = footstepSounds[0];
            footstepSounds[0] = audioSource.clip;
        }

        private void PlayJumpSound()
        {
            if (jumpSound != null)
                audioSource.PlayOneShot(jumpSound);
        }

        private void PlayLandingSound()
        {
            if (landSound != null)
                audioSource.PlayOneShot(landSound);
            nextStep = stepCycle + .5f;
        }

        private void ProgressStepCycle(float speed)
        {
            if (controller.velocity.sqrMagnitude > 0 && (input.x != 0 || input.y != 0))
            {
                stepCycle += (controller.velocity.magnitude + (speed * (isWalking ? 1f : runStepLengthen))) * Time.fixedDeltaTime;
            }

            if (stepCycle > nextStep)
            {
                nextStep = stepCycle + stepInterval;
                PlayFootstepAudio();
            }
        }

        private void GetInput(out float speed)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            isWalking = !Input.GetKey(KeyCode.LeftShift);

            speed = isWalking ? walkSpeed : runSpeed;
            input = new Vector2(h, v);

            if (input.sqrMagnitude > 1)
                input.Normalize();
        }

        private void RotateView()
        {
            mouseLook.LookRotation(transform, playerCamera.transform);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;

            if (collisionFlags == CollisionFlags.Below || body == null || body.isKinematic)
                return;

            body.AddForceAtPosition(controller.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }
    }

    public class MouseLook
    {
        internal void Init(Transform transform1, Transform transform2)
        {
            throw new System.NotImplementedException();
        }

        internal void LookRotation(Transform transform1, Transform transform2)
        {
            throw new System.NotImplementedException();
        }
    }
}
