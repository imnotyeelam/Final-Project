using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float swayAmount = 0.05f;
    public float swaySmooth = 6f;

    private Vector3 initialPos;

    void Start()
    {
        initialPos = transform.localPosition;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 targetPos = initialPos + new Vector3(-mouseX, -mouseY, 0f) * swayAmount;
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * swaySmooth);
    }
}
