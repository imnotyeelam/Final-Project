using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GrapplingHook : MonoBehaviour
{
    [Header("Grappling Settings")]
    public Transform cameraTransform;
    public Transform hookStartPoint;
    public float maxGrappleDistance = 30f;
    public float grappleSpeed = 10f;
    public LayerMask grappleMask;
    public GameObject ropeCylinderPrefab;

    [Header("Aiming Settings")]
    public float normalMaxDistance = 30f;
    public float aimMaxDistance = 50f; // Longer range when aiming
    public float normalSpeed = 10f;
    public float aimSpeed = 15f; // Faster when aiming

    [Header("Visuals")]
    public Animator handAnimator;
    public GameObject aimIndicator; // Visual cue for aim mode

    private CharacterController controller;
    private Vector3 grapplePoint;
    private bool isGrappling = false;
    private GameObject currentRopeCylinder;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (aimIndicator) aimIndicator.SetActive(false);
    }

    void Update()
    {
        // Only allow grappling in hook mode
        if (HandSwitcher.CurrentMode != 1) 
        {
            if (isGrappling) ReleaseGrapple();
            return;
        }

        // Update aiming visual
        if (aimIndicator)
        {
            aimIndicator.SetActive(HandSwitcher.IsAiming);
        }

        // Adjust values based on aiming state
        maxGrappleDistance = HandSwitcher.IsAiming ? aimMaxDistance : normalMaxDistance;
        grappleSpeed = HandSwitcher.IsAiming ? aimSpeed : normalSpeed;

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryStartGrapple();
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            ReleaseGrapple();
        }

        if (isGrappling)
        {
            PerformGrappleMovement();
            UpdateRopeVisual();
        }
    }

    void TryStartGrapple()
    {
        Ray ray = HandSwitcher.IsAiming 
            ? new Ray(cameraTransform.position, cameraTransform.forward) // Precise aim
            : new Ray(hookStartPoint.position, hookStartPoint.forward); // Hip fire

        if (Physics.Raycast(ray, out RaycastHit hit, maxGrappleDistance, grappleMask))
        {
            grapplePoint = hit.point;
            isGrappling = true;

            if (handAnimator != null)
            {
                handAnimator.SetBool("IsGrappling", true); // Changed from "IsFisting"
            }

            if (ropeCylinderPrefab != null && currentRopeCylinder == null)
            {
                currentRopeCylinder = Instantiate(ropeCylinderPrefab);
            }
        }
    }

    void ReleaseGrapple()
    {
        isGrappling = false;

        if (handAnimator != null)
        {
            handAnimator.SetBool("IsGrappling", false);
        }

        if (currentRopeCylinder)
        {
            Destroy(currentRopeCylinder);
        }
    }

    void PerformGrappleMovement()
    {
        Vector3 direction = (grapplePoint - transform.position).normalized;
        controller.Move(direction * grappleSpeed * Time.deltaTime);
        
        // Optional: Add slight upward force for better feel
        controller.Move(Vector3.up * grappleSpeed * 0.2f * Time.deltaTime);
    }

    void UpdateRopeVisual()
    {
        if (currentRopeCylinder == null) return;

        Vector3 start = hookStartPoint.position;
        Vector3 end = grapplePoint;
        Vector3 dir = end - start;

        currentRopeCylinder.transform.position = start + dir * 0.5f;
        currentRopeCylinder.transform.up = dir.normalized;
        currentRopeCylinder.transform.localScale = new Vector3(0.1f, dir.magnitude * 0.5f, 0.1f);
    }

    void OnDisable()
    {
        ReleaseGrapple();
    }
}