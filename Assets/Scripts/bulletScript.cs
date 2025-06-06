using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class bulletScript : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 1f;
    public GameObject ExplosionPrefab;
    public AudioSource audioSource;
    private Rigidbody2D _rb;
    private SpriteRenderer spriteRenderer; 
    public CapsuleCollider2D collider;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
        Destroy(gameObject, 10f);

        // Debugging: Überprüfen, ob AudioSource und Clip vorhanden sind
        if (audioSource == null)
        {
            Debug.LogError("AudioSource-Komponente fehlt!");
        }
        else if (audioSource.clip == null)
        {
            Debug.LogError("Audio-Clip fehlt in der AudioSource!");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Bullet collided with: {collision.gameObject.name}");
        HandleCollision(collision);
    }

    private void HandleCollision(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ApplyDamage<PlayerHealth>(collision.gameObject, damage);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            ApplyDamage<EnemyHealth>(collision.gameObject, damage);
        }
        else if (collision.gameObject.CompareTag("EnemyBoss"))
        {
            ApplyDamage<EnemyHealth>(collision.gameObject, damage);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Bullet hit a wall.");
            Destroy(gameObject);
        }
    }

    private void ApplyDamage<T>(GameObject target, float damage) where T : MonoBehaviour
    {
        if (target.TryGetComponent(out T healthComponent))
        {
            if (healthComponent is PlayerHealth playerHealth)
            {
                playerHealth.TakeDamage((int)damage);
                Debug.Log($"Dealt {damage} damage to player.");
            }
            else if (healthComponent is EnemyHealth enemyHealth)
            {
                enemyHealth.TakeDamage((int)damage);
                Debug.Log($"Dealt {damage} damage to enemy.");
            }

            if (ExplosionPrefab != null)
            {
                Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            }

            // Sound abspielen und sicherstellen, dass es vor der Zerstörung abgespielt wird
            if (audioSource != null && audioSource.clip != null)
            {
                Debug.Log("Sound wird abgespielt.");
                audioSource.Play(); // Abspielen des Sounds
                _rb.velocity = Vector2.zero; // Stoppe die Bewegung der Kugel
                spriteRenderer.enabled = false; // Deaktivieren des Sprites, um den Soundeffekt zu verdecken
                collider.enabled = false; // Deaktivieren des Colliders, um weitere Kollisionen zu verhindern
                Destroy(gameObject, audioSource.clip.length);
            }
            else
            {
                Debug.LogWarning("AudioSource oder Audio-Clip fehlt, Sound konnte nicht abgespielt werden.");
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.LogWarning($"No health component of type {typeof(T).Name} found on {target.name}.");
        }
    }
}