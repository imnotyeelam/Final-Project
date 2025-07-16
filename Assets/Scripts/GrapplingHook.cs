using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class GrapplingHook : MonoBehaviour
{
    [Header("Grappling Settings")]
    public Transform cameraTransform;
    public Transform hookStartPoint;
    public float maxGrappleDistance = 30f;
    public float grappleSpeed = 20f;
    public LayerMask grappleMask;
    public GameObject ropeCylinderPrefab;

    [Header("Aiming Settings")]
    public float normalMaxDistance = 30f;
    public float aimMaxDistance = 50f;
    public float normalSpeed = 12f;
    public float aimSpeed = 18f;

    [Header("Visuals")]
    public Animator handAnimator;
    public GameObject aimIndicator;
    public float cooldown = 0.5f;

    private CharacterController controller;
    private Vector3 grapplePoint;
    private bool isGrappling = false;
    private GameObject currentRopeCylinder;
    private float lastGrappleTime;
    private bool initialBoostApplied = false;

    // Gravity override
    private SimpleFPSMovement movementScript;
    private float originalGravity;
    private bool gravityOverridden = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        movementScript = GetComponent<SimpleFPSMovement>();
        if (movementScript != null)
        {
            originalGravity = movementScript.gravity;
        }

        if (aimIndicator) aimIndicator.SetActive(false);
    }

    void Update()
    {
        if (HandSwitcher.CurrentMode != 1)
        {
            if (isGrappling) ReleaseGrapple();
            return;
        }

        if (aimIndicator)
        {
            bool showIndicator = HandSwitcher.IsAiming;
            aimIndicator.SetActive(showIndicator);

            if (showIndicator) UpdateAimIndicatorColor();
        }

        maxGrappleDistance = HandSwitcher.IsAiming ? aimMaxDistance : normalMaxDistance;
        grappleSpeed = HandSwitcher.IsAiming ? aimSpeed : normalSpeed;

        if (Input.GetMouseButtonDown(0) && !isGrappling && Time.time > lastGrappleTime + cooldown)
        {
            TryStartGrapple();
        }

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
            initialBoostApplied = false;

            if (handAnimator) handAnimator.SetBool("IsGrappling", true);

            if (ropeCylinderPrefab != null && currentRopeCylinder == null)
                currentRopeCylinder = Instantiate(ropeCylinderPrefab);

            // Override gravity
            if (movementScript && !gravityOverridden)
            {
                movementScript.gravity = -1f; // Weak gravity during grapple
                gravityOverridden = true;
            }
        }
    }

    void ReleaseGrapple()
    {
        isGrappling = false;
        lastGrappleTime = Time.time;

        if (handAnimator) handAnimator.SetBool("IsGrappling", false);

        if (currentRopeCylinder) Destroy(currentRopeCylinder);

        // Restore gravity
        if (movementScript && gravityOverridden)
        {
            movementScript.gravity = originalGravity;
            gravityOverridden = false;
        }
    }

    void PerformGrappleMovement()
    {
        Vector3 direction = (grapplePoint - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, grapplePoint);

        float pullSpeed = Mathf.Lerp(grappleSpeed * 1.8f, grappleSpeed * 0.6f, distance / maxGrappleDistance);

        if (!initialBoostApplied)
        {
            pullSpeed *= 2.5f;
            initialBoostApplied = true;
        }

        controller.Move(direction * pullSpeed * Time.deltaTime);

        if (distance < 1.2f)
        {
            ReleaseGrapple();
        }
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
