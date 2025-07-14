using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GrapplingHook : MonoBehaviour
{
    [Header("Grappling Settings")]
    public Transform cameraTransform;
    public Transform hookStartPoint;
    public float maxGrappleDistance = 30f;
    public float grappleSpeed = 20f;
    public float stopDistance = 2f; // Distance to stop near grapple point
    public LayerMask grappleMask;
    public GameObject ropeCylinderPrefab;

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
        // Shoot grapple
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, maxGrappleDistance, grappleMask))
            {
                grapplePoint = hit.point;
                isGrappling = true;

                if (currentRopeCylinder == null && ropeCylinderPrefab != null)
                {
                    currentRopeCylinder = Instantiate(ropeCylinderPrefab);
                }
            }
        }

        // Cancel grapple
        if (Input.GetKeyUp(KeyCode.E))
        {
            StopGrapple();
        }

        if (isGrappling)
        {
            Vector3 toTarget = grapplePoint - transform.position;
            float distance = toTarget.magnitude;

            if (distance > stopDistance)
            {
                // Pull the player smoothly
                Vector3 move = toTarget.normalized * grappleSpeed * Time.deltaTime;
                controller.Move(move);
            }
            else
            {
                // Stop when close enough
                StopGrapple();
            }

            UpdateRopeVisual();
        }
    }

    void StopGrapple()
    {
        isGrappling = false;
        if (currentRopeCylinder)
        {
            Destroy(currentRopeCylinder);
        }
    }

    void UpdateRopeVisual()
    {
        if (currentRopeCylinder == null) return;

        Vector3 start = hookStartPoint.position;
        Vector3 end = grapplePoint;
        Vector3 dir = end - start;
        float length = dir.magnitude;

        currentRopeCylinder.transform.position = start + dir / 2f;
        currentRopeCylinder.transform.up = dir.normalized;
        currentRopeCylinder.transform.localScale = new Vector3(0.05f, length / 2f, 0.05f);
    }
}
