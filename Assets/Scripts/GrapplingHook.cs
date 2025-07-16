using UnityEngine;
using UnityEngine.UI;

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
    public float aimMaxDistance = 50f;
    public float normalSpeed = 10f;
    public float aimSpeed = 15f;

    [Header("Visuals")]
    public Animator handAnimator;
    public GameObject aimIndicator;
    public float cooldown = 0.5f;

    [Header("Grapple Gravity")]
    public float grappleGravity = -5f; // Lighter gravity while grappling

    private CharacterController controller;
    private Vector3 grapplePoint;
    private bool isGrappling = false;
    private GameObject currentRopeCylinder;
    private float lastGrappleTime;

    private SimpleFPSMovement movementScript;
    private float originalGravity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        movementScript = GetComponent<SimpleFPSMovement>();
        if (movementScript != null)
            originalGravity = movementScript.gravity;

        if (aimIndicator) aimIndicator.SetActive(false);
    }

    void Update()
    {
        // If not in hook mode
        if (HandSwitcher.CurrentMode != 1)
        {
            if (isGrappling)
                ReleaseGrapple();

            if (aimIndicator && aimIndicator.activeSelf)
                aimIndicator.SetActive(false);

            return;
        }

        // Aim indicator visibility
        if (aimIndicator)
        {
            bool showIndicator = HandSwitcher.IsAiming;
            aimIndicator.SetActive(showIndicator);
            if (showIndicator) UpdateAimIndicatorColor();
        }

        // Adjust values based on aim
        maxGrappleDistance = HandSwitcher.IsAiming ? aimMaxDistance : normalMaxDistance;
        grappleSpeed = HandSwitcher.IsAiming ? aimSpeed : normalSpeed;

        // Start grapple
        if (Input.GetMouseButtonDown(0) && !isGrappling && Time.time > lastGrappleTime + cooldown)
        {
            TryStartGrapple();
        }

        // Release grapple
        if (Input.GetMouseButtonUp(0) && isGrappling)
        {
            ReleaseGrapple();
        }

        if (isGrappling)
        {
            PerformGrappleMovement();
            UpdateRopeVisual();
        }
    }

    void UpdateAimIndicatorColor()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        bool canGrapple = Physics.Raycast(ray, maxGrappleDistance, grappleMask);

        if (aimIndicator.TryGetComponent<Image>(out var image))
        {
            image.color = canGrapple ? Color.green : Color.red;
        }
    }

    void TryStartGrapple()
    {
        Ray ray = HandSwitcher.IsAiming
            ? new Ray(cameraTransform.position, cameraTransform.forward)
            : new Ray(hookStartPoint.position, hookStartPoint.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxGrappleDistance, grappleMask))
        {
            grapplePoint = hit.point;
            isGrappling = true;
            lastGrappleTime = Time.time;

            if (handAnimator != null)
                handAnimator.SetBool("IsGrappling", true);

            if (ropeCylinderPrefab != null && currentRopeCylinder == null)
                currentRopeCylinder = Instantiate(ropeCylinderPrefab);

            // Reduce gravity
            if (movementScript != null)
                movementScript.gravity = grappleGravity;
        }
    }

    void ReleaseGrapple()
    {
        isGrappling = false;
        lastGrappleTime = Time.time;

        if (handAnimator != null)
            handAnimator.SetBool("IsGrappling", false);

        if (currentRopeCylinder)
            Destroy(currentRopeCylinder);

        // Restore gravity
        if (movementScript != null)
            movementScript.gravity = originalGravity;
    }

    void PerformGrappleMovement()
    {
        Vector3 direction = (grapplePoint - transform.position).normalized;
        controller.Move(direction * grappleSpeed * Time.deltaTime);
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
        if (aimIndicator) aimIndicator.SetActive(false);
    }
}
