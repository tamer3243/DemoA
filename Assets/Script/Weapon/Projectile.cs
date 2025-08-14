using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 10;

    private float spawnTime;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        spawnTime = Time.time;
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void Update()
    {
        if (Time.time - spawnTime >= lifeTime)
            ReturnToPool();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Có thể gọi TakeDamage ở đây
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
