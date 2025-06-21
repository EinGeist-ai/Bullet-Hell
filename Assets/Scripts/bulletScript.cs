using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class bulletScript : MonoBehaviour
{

    [Header("Bullet Settings")]
    public float speed = 20f;
    public float damage = 1f;

    [Header("Refrences")]
    public GameObject ExplosionPrefab;
    public AudioSource audioSource;
    public SpriteRenderer spriteRenderer; 
    public CapsuleCollider2D capsuleColider;
    private Rigidbody2D _rb;
    private GameObject player;
    private PlayerUpgrades playerUpgrades;
    private float localPiercing = 0;

    void Start()
    {
        
        
            player = GameObject.FindGameObjectWithTag("Player");
            playerUpgrades = player.GetComponent<PlayerUpgrades>();
            if (playerUpgrades != null)
            {
                if (playerUpgrades.hasBulletPierceUpgrade)
                {
                    localPiercing = playerUpgrades.bulletPierceIncrease;
                }
            }
            else
            {
                Debug.LogWarning("PlayerUpgrades-Komponente nicht gefunden!");
            }

        _rb = GetComponent<Rigidbody2D>();
        capsuleColider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
        Destroy(gameObject, 10f);

        if (audioSource == null)
        {
            Debug.LogError("AudioSource-Komponente fehlt!");
        }
        else if (audioSource.clip == null)
        {
            Debug.LogError("Audio-Clip fehlt in der AudioSource!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        HandleTrigger(other);
    }

    private void HandleTrigger(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ApplyDamage<PlayerHealth>(other.gameObject, damage);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            ApplyDamage<EnemyHealth>(other.gameObject, damage);
            localPiercing -= 1;
        }
        else if (other.gameObject.CompareTag("EnemyBoss"))
        {
            ApplyDamage<EnemyHealth>(other.gameObject, damage);
            localPiercing -= 1;
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
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

            if (localPiercing <= 0 && !target.CompareTag("Player") && !target.CompareTag("Wall"))
            {
                if (audioSource != null && audioSource.clip != null)
                {
                    Debug.Log("Sound wird abgespielt.");
                    audioSource.Play();
                    _rb.velocity = Vector2.zero;
                    spriteRenderer.enabled = false;
                    capsuleColider.enabled = false;
                    Destroy(gameObject, audioSource.clip.length);
                }
                else
                {
                    Debug.LogWarning("AudioSource oder Audio-Clip fehlt, Sound konnte nicht abgespielt werden.");
                    Destroy(gameObject);
                }
            }
            else if (target.CompareTag("Player") || target.CompareTag("Wall"))
            {
                if (audioSource != null && audioSource.clip != null)
                {
                    Debug.Log("Sound wird abgespielt.");
                    audioSource.Play();
                    _rb.velocity = Vector2.zero;
                    spriteRenderer.enabled = false;
                    capsuleColider.enabled = false;
                    Destroy(gameObject, audioSource.clip.length);
                }
                else
                {
                    Debug.LogWarning("AudioSource oder Audio-Clip fehlt, Sound konnte nicht abgespielt werden.");
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            Debug.LogWarning($"No health component of type {typeof(T).Name} found on {target.name}.");
        }
    }
}