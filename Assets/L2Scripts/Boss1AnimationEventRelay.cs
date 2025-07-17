using UnityEngine;

public class Boss1AnimationEventRelay : MonoBehaviour
{
    public Boss1Controller controller;

    public void ThrowGrenade()
    {
        if (controller != null)
        {
            controller.ThrowGrenade();
        }
    }
}
