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

    private CharacterController controller;
    private Vector3 grapplePoint;
    private bool isGrappling = false;
    private GameObject currentRopeCylinder;
    private float lastGrappleTime;

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
            bool showIndicator = HandSwitcher.IsAiming;
            aimIndicator.SetActive(showIndicator);
            
            if (showIndicator)
            {
                UpdateAimIndicatorColor();
            }
        }

        // Adjust values based on aiming state
        maxGrappleDistance = HandSwitcher.IsAiming ? aimMaxDistance : normalMaxDistance;
        grappleSpeed = HandSwitcher.IsAiming ? aimSpeed : normalSpeed;

        // Start grappling on left-click down
        if (Input.GetMouseButtonDown(0) && !isGrappling && Time.time > lastGrappleTime + cooldown)
        {
            TryStartGrapple();
        }

        // Release grappling on left-click up
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
            {
                handAnimator.SetBool("IsGrappling", true);
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
        lastGrappleTime = Time.time;

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