using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 50f;
    public float lifetime = 2f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // You can add hit logic here
        Destroy(gameObject);
    }
}
