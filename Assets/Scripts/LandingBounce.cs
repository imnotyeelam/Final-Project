using UnityEngine;

public class LandingBounce : MonoBehaviour
{
    public SimpleFPSMovement movement;
    public float bounceAmount = 0.1f;
    public float bounceSpeed = 4f;

    private float verticalOffset = 0f;
    private bool wasGroundedLastFrame = true;

    void Update()
    {
        if (!movement) return;

        if (!wasGroundedLastFrame && movement.isGrounded)
        {
            verticalOffset = -bounceAmount;
        }

        verticalOffset = Mathf.Lerp(verticalOffset, 0f, Time.deltaTime * bounceSpeed);
        transform.localPosition = new Vector3(transform.localPosition.x, verticalOffset, transform.localPosition.z);

        wasGroundedLastFrame = movement.isGrounded;
    }
}
