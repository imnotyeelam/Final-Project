using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bullet;

    public float rangeToTargetPlayer, timeBetweenShots = 0.5f;
    private float shotCounter;

    public Transform gun, firePoint;

    public float rotateSpeed = 45f;

    public LayerMask obstacleMask; // �����ϰ��㣨���� Bookcase��
    bool CanSeePlayer()
    {
        Vector3 direction = (PlayerController1.instance.transform.position - firePoint.position).normalized;
        float distance = Vector3.Distance(firePoint.position, PlayerController1.instance.transform.position);

        // ���߼��
        if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, distance, ~0))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true; // �м�û���ϰ�����Թ���
            }
        }
        return false; // ���ϰ���ס
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shotCounter = timeBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, PlayerController1.instance.transform.position) < rangeToTargetPlayer && CanSeePlayer())
        {
            gun.LookAt(PlayerController1.instance.transform.position + new Vector3(0f, 0.2f, 0f));

            shotCounter -= Time.deltaTime;

            if (shotCounter <= 0)
            {
                Instantiate(bullet, firePoint.position, firePoint.rotation);
                shotCounter = timeBetweenShots;
            }
        }

        else
        {
            shotCounter = timeBetweenShots;
            gun.rotation = Quaternion.Lerp(gun.rotation, Quaternion.Euler(0f, gun.rotation.eulerAngles.y + 10f, 0f), rotateSpeed * Time.deltaTime);
        }
    }
}
