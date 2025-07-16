using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class GrapplingHook : MonoBehaviour
{
    [Header("Core Settings")]
    public Transform cameraTransform;
    public Transform hookStartPoint;
    public LayerMask grappleMask;
    public GameObject ropePrefab;

    [Header("Distance Settings")]
    public float normalMaxDistance = 30f;
    public float aimMaxDistance = 50f;

    [Header("Speed Settings")]
    public float normalSpeed = 10f;
    public float aimSpeed = 15f;
    public float cooldown = 0.5f;

    [Header("Visual Settings")]
    public GameObject aimIndicator;
    public Color readyColor = new Color(1, 0.5f, 0, 0.8f); // Orange
    public Color validColor = Color.green;
    public Color invalidColor = Color.red;
    public Animator handAnimator;

    // Private variables
    private CharacterController controller;
    private Vector3 grapplePoint;
    private bool isGrappling = false;
    private GameObject currentRope;
    private float lastGrappleTime;
    private float currentMaxDistance;
    private float currentGrappleSpeed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentMaxDistance = normalMaxDistance;
        currentGrappleSpeed = normalSpeed;
        
        if (aimIndicator)
        {
            aimIndicator.SetActive(false);
            UpdateIndicatorColor(readyColor);
        }
    }

    void Update()
    {
        // Only work in hook mode
        if (HandSwitcher.CurrentMode != 1)
        {
            if (isGrappling) ReleaseGrapple();
            return;
        }

        UpdateAimState();
        HandleGrappleInput();
        UpdateGrappleMovement();
    }

    void UpdateAimState()
    {
        // Update current values based on aiming state
        currentMaxDistance = HandSwitcher.IsAiming ? aimMaxDistance : normalMaxDistance;
        currentGrappleSpeed = HandSwitcher.IsAiming ? aimSpeed : normalSpeed;

        // Update indicator visibility
        if (aimIndicator)
        {
            bool shouldShow = HandSwitcher.IsAiming;
            aimIndicator.SetActive(shouldShow);

            if (shouldShow)
            {
                UpdateIndicatorColor(CheckValidTarget() ? validColor : invalidColor);
            }
            else
            {
                UpdateIndicatorColor(readyColor);
            }
        }
    }

    bool CheckValidTarget()
    {
        Ray ray = HandSwitcher.IsAiming
            ? new Ray(cameraTransform.position, cameraTransform.forward)
            : new Ray(hookStartPoint.position, hookStartPoint.forward);
        
        return Physics.Raycast(ray, currentMaxDistance, grappleMask);
    }

    void HandleGrappleInput()
    {
        if (Input.GetMouseButtonDown(0) )// Left click
        {
            if (!isGrappling && Time.time > lastGrappleTime + cooldown)
            {
                TryStartGrapple();
            }
            else if (isGrappling)
            {
                ReleaseGrapple();
            }
        }
    }

    void TryStartGrapple()
    {
        Ray ray = HandSwitcher.IsAiming
            ? new Ray(cameraTransform.position, cameraTransform.forward)
            : new Ray(hookStartPoint.position, hookStartPoint.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, currentMaxDistance, grappleMask))
        {
            StartGrapple(hit.point);
        }
    }

    void StartGrapple(Vector3 targetPoint)
    {
        grapplePoint = targetPoint;
        isGrappling = true;
        lastGrappleTime = Time.time;

        // Visual feedback
        if (handAnimator) handAnimator.SetBool("IsGrappling", true);
        if (ropePrefab) currentRope = Instantiate(ropePrefab);
    }

    void UpdateGrappleMovement()
    {
        if (!isGrappling) return;

        Vector3 direction = (grapplePoint - transform.position).normalized;
        controller.Move(direction * currentGrappleSpeed * Time.deltaTime);
        
        // Small upward force for better feel
        controller.Move(Vector3.up * currentGrappleSpeed * 0.1f * Time.deltaTime);

        UpdateRopeVisual();
    }

    void UpdateRopeVisual()
    {
        if (!currentRope) return;

        Vector3 start = hookStartPoint.position;
        Vector3 end = grapplePoint;
        Vector3 midpoint = (start + end) * 0.5f;

        currentRope.transform.position = midpoint;
        currentRope.transform.up = (end - start).normalized;
        currentRope.transform.localScale = new Vector3(
            0.1f, 
            Vector3.Distance(start, end) * 0.5f, 
            0.1f
        );
    }

    void ReleaseGrapple()
    {
        if (!isGrappling) return;

        isGrappling = false;
        lastGrappleTime = Time.time;

        // Visual feedback
        if (handAnimator) handAnimator.SetBool("IsGrappling", false);
        if (currentRope) Destroy(currentRope);
    }

    void UpdateIndicatorColor(Color color)
    {
        if (!aimIndicator) return;

        // Try UI Image first
        var image = aimIndicator.GetComponent<Image>();
        if (image != null)
        {
            image.color = color;
            return;
        }

        // Fallback to SpriteRenderer
        var sprite = aimIndicator.GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.color = color;
        }
    }

    void OnDisable()
    {
        ReleaseGrapple();
        if (aimIndicator) aimIndicator.SetActive(false);
    }
}