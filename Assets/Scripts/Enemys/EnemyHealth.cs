using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Explosion Prefabs")]
    public GameObject explosion1;
    public GameObject explosion2;
    public GameObject explosion3;

    [Header("Refrences")]
    public GameObject ExplosionPrefab;
    public AudioSource audioData;
    public CapsuleCollider2D capsuleColider;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private EnemyBehaviorShooter movementShooter;
    private EnemyBehaviorMeele movementMeele;
    private PlayerShooting shooting;
    private Rigidbody2D _rb;

    [Header("Default Health")]
    public float maxHealth = 3f;
    public float currentHealth;

    [Header("Explosion Delay")]
    public float explosionDelay = 1f; 
    public float explosionDelay2 = 0.5f;

    
    void Start()
    {
        currentHealth = maxHealth; 
        audioData = GetComponent<AudioSource>(); 
        capsuleColider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        movementShooter = GetComponent<EnemyBehaviorShooter>();
        movementMeele = GetComponent<EnemyBehaviorMeele>();
        shooting = GetComponent<PlayerShooting>();
        _rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 && CompareTag("Enemy"))
        {
            GameObject newObject = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            newObject.transform.localScale = new Vector3(5f, 5f, -3f);
            Die();
        }
        else if (currentHealth <= 0 && CompareTag("EnemyBoss"))
        {
            StartCoroutine(TriggerBossExplosions());
            Die();
        }
    }

    private IEnumerator TriggerBossExplosions()
    {
        // Instantiate explosions with delay
        GameObject newObject1 = Instantiate(ExplosionPrefab, explosion1.transform.position, Quaternion.identity);
        newObject1.transform.localScale = new Vector3(3f, 3f, -3f);
        audioData.Play();
        yield return new WaitForSeconds(explosionDelay);

        GameObject newObject3 = Instantiate(ExplosionPrefab, explosion3.transform.position, Quaternion.identity);
        newObject3.transform.localScale = new Vector3(3f, 3f, -3f);
        audioData.Play();
        yield return new WaitForSeconds(explosionDelay2);
        

        GameObject newObject2 = Instantiate(ExplosionPrefab, explosion2.transform.position, Quaternion.identity);
        newObject2.transform.localScale = new Vector3(10f, 10f, -3f);
        audioData.Play();
        spriteRenderer.enabled = false;
    }

    private void Die()
    {
        Debug.Log("Enemy has died!");
        capsuleColider.enabled = false;
        if (boxCollider != null)          
        {                               
            boxCollider.enabled = false; 
        }
        if (shooting != null) 
        {
            shooting.enabled = false;
        }
        if (movementShooter != null)
        {
            movementShooter.enabled = false;
        }
        if (movementMeele != null)
        {
            movementMeele.enabled = false;
        }
        if (spriteRenderer.enabled)
        {
            spriteRenderer.enabled = false;
        }

        _rb.velocity = Vector2.zero;

        if (CompareTag ("EnemyBoss"))
        {
            Destroy(gameObject, audioData.clip.length * 3);
        }
        else
        {
            Destroy(gameObject, audioData.clip.length);

        }

        
    }
}
