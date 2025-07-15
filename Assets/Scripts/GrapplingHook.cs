using System;
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

    [Header("Hand Animation")]
    public Animator handAnimator;

    private CharacterController controller;
    private Vector3 grapplePoint;
    private bool isGrappling = false;
    private GameObject currentRopeCylinder;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Only allow grappling in hooker mode
        if (HandSwitcher.CurrentMode != 1) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, maxGrappleDistance, grappleMask))
            {
                grapplePoint = hit.point;
                isGrappling = true;

                if (handAnimator != null)
                {
                    handAnimator.SetBool("IsFisting", true);
                }

                if (currentRopeCylinder == null && ropeCylinderPrefab != null)
                {
                    currentRopeCylinder = Instantiate(ropeCylinderPrefab);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            isGrappling = false;

            if (handAnimator != null)
            {
                handAnimator.SetBool("IsFisting", false);
            }

            if (currentRopeCylinder)
            {
                Destroy(currentRopeCylinder);
            }
        }

        if (isGrappling)
        {
            Vector3 direction = (grapplePoint - transform.position).normalized;
            controller.Move(direction * grappleSpeed * Time.deltaTime);
            UpdateRopeVisual();
        }
    }
    
    void UpdateRopeVisual()
    {
        if (currentRopeCylinder == null) return;

        Vector3 start = hookStartPoint.position;
        Vector3 end = grapplePoint;
        Vector3 dir = end - start;
        float length = dir.magnitude;

        // Position rope in the middle of the start and end points
        currentRopeCylinder.transform.position = start + dir / 2f;

        // Rotate rope to align with the direction
        currentRopeCylinder.transform.up = dir.normalized;

        // Scale rope based on distance (assuming Y-axis is up in cylinder)
        currentRopeCylinder.transform.localScale = new Vector3(0.1f, length / 2f, 0.1f);
    }
}
