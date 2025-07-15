using UnityEngine;

public class LightningEffect : MonoBehaviour
{
    public LineRenderer line;
    public float duration = 0.1f;

    public void ShowLightning(Vector3 start, Vector3 end)
    {
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        gameObject.SetActive(true);
        Invoke(nameof(Disable), duration);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}
