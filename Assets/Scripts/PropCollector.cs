using UnityEngine;

public class PropCollector : MonoBehaviour
{
    public Camera playerCamera;
    public float collectRange = 3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryCollectProp();
        }
    }

    void TryCollectProp()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, collectRange))
        {
            Prop prop = hit.collider.GetComponent<Prop>();
            if (prop != null)
            {
                if (prop.isWeapon && prop.weaponPrefab != null)
                {
                    WeaponManager.Instance?.EquipWeapon(prop.weaponPrefab);
                    UIManager.Instance?.AddTask("Picked up weapon: " + prop.weaponName);
                }
                else
                {
                    UIManager.Instance?.AddTask("Collected prop: " + prop.propName);
                }

                Destroy(hit.collider.gameObject);
            }
        }
    }
}
