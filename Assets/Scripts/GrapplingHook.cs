using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public Camera playerCamera;
    public Transform grappleOrigin; // 空 GameObject，挂在摄像头前
    public float maxDistance = 30f;
    public LayerMask grappleLayer;

    public LineRenderer lineRenderer;
    public float pullSpeed = 10f;

    private Vector3 grapplePoint;
    private bool isGrappling = false;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartGrapple();
        }

        if (isGrappling)
        {
            GrappleMovement();
            DrawRope();
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    void StartGrapple()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, grappleLayer))
        {
            grapplePoint = hit.point;
            isGrappling = true;
            lineRenderer.positionCount = 2;
        }
    }

    void GrappleMovement()
    {
        Vector3 direction = (grapplePoint - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, grapplePoint);

        if (distance > 2f)
        {
            controller.Move(direction * pullSpeed * Time.deltaTime);
        }
        else
        {
            isGrappling = false;
        }
    }

    void DrawRope()
    {
        lineRenderer.SetPosition(0, grappleOrigin.position);
        lineRenderer.SetPosition(1, grapplePoint);
    }
}
