using DigitalRuby.LightningBolt;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 50f;
    public float lifetime = 2f;

    private LightningBoltScript lightning;

    void Start()
    {
        Destroy(gameObject, lifetime);

        // Find the LightningBoltScript component in the child
        lightning = GetComponentInChildren<LightningBoltScript>();

        if (lightning != null)
        {
            lightning.gameObject.SetActive(true); // Enable the lightning effect
            lightning.StartPosition = transform.position;
            lightning.EndPosition = transform.position; // Optional, update in Update()
        }
    }

    void Update()
    {
        // Move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Update lightning positions
        if (lightning != null)
        {
            lightning.StartPosition = transform.position + Vector3.left * 0.2f;  // Offset for style
            lightning.EndPosition = transform.position + Vector3.right * 0.2f;  // Offset for style
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // TODO: Add hit logic here
        Destroy(gameObject);
    }
}
