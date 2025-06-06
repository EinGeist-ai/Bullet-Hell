using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 3f; // Maximum health of the enemy
    private float currentHealth; // Current health of the enemy
    public GameObject ExplosionPrefab;
    public AudioSource audioData; // Reference to the AudioSource component for sound effects
    public CapsuleCollider2D collider; // Reference to the CapsuleCollider2D component
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component 
    private BoxCollider2D boxCollider; // Reference to the BoxCollider2D component
    private EnemyBehaviorShooter movementShooter; // Reference to the movement script if needed
    private EnemyBehaviorMeele movementMeele; // Reference to the meele movement script if needed
    private PlayerShooting shooting; // Reference to the shooting script if needed
    private Rigidbody2D _rb;
    public GameObject explosion1;
    public GameObject explosion2;
    public GameObject explosion3;


    public float explosionDelay = 1f; // Delay between explosions
    public float explosionDelay2 = 0.5f; // Delay between explosions for the second explosion

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; // Initialize current health to max health at the start
        audioData = GetComponent<AudioSource>(); // Correct initialization of the class-level variable
        collider = GetComponent<CapsuleCollider2D>(); // Get the CapsuleCollider2D component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        boxCollider = GetComponent<BoxCollider2D>(); // Get the BoxCollider2D component
        movementShooter = GetComponent<EnemyBehaviorShooter>(); // Get the movement script if it exists
        movementMeele = GetComponent<EnemyBehaviorMeele>(); // Get the meele movement script if it exists
        shooting = GetComponent<PlayerShooting>(); // Get the shooting script if it exists
        _rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // Reduce current health by damage amount
        if (currentHealth <= 0 && CompareTag("Enemy"))
        {
            GameObject newObject = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            newObject.transform.localScale = new Vector3(5f, 5f, -3f); // Set the scale of the explosion
            Die(); // Call Die method if health is 0 or less
        }
        else if (currentHealth <= 0 && CompareTag("EnemyBoss"))
        {
            StartCoroutine(TriggerBossExplosions()); // Start the coroutine for Explosion sequence
            Die(); // Call Die method if health is 0 or less
        }
    }

    private IEnumerator TriggerBossExplosions()
    {
        // Instantiate explosions with delay
        GameObject newObject1 = Instantiate(ExplosionPrefab, explosion1.transform.position, Quaternion.identity);
        newObject1.transform.localScale = new Vector3(3f, 3f, -3f); // Set the scale of the explosion
        audioData.Play(); // Play the explosion sound
        yield return new WaitForSeconds(explosionDelay); // Wait for the delay

        GameObject newObject3 = Instantiate(ExplosionPrefab, explosion3.transform.position, Quaternion.identity);
        newObject3.transform.localScale = new Vector3(3f, 3f, -3f); // Set the scale of the explosion
        audioData.Play(); // Play the explosion sound again
        yield return new WaitForSeconds(explosionDelay2); // Wait for the delay
        

        GameObject newObject2 = Instantiate(ExplosionPrefab, explosion2.transform.position, Quaternion.identity);
        newObject2.transform.localScale = new Vector3(10f, 10f, -3f); // Set the scale of the explosion
        audioData.Play(); // Play the explosion sound again
        spriteRenderer.enabled = false; // Disable the sprite renderer to hide the enemy
    }

    private void Die()
    {
        Debug.Log("Enemy has died!"); // Log death message
        // Here you can add more logic for enemy death, like playing an animation or dropping loot
        // For now, we will just destroy the enemy object
        collider.enabled = false; // Disable the collider to prevent further collisions
        if (boxCollider != null)          // Check if boxCollider exists
        {                               
            boxCollider.enabled = false; // Disable the box collider to prevent further collisions
        }
        if (shooting != null) // Check if shooting script exists
        {
            shooting.enabled = false; // Disable the shooting script to prevent further actions
        }
        if (movementShooter != null) // Check if movementShooter script exists
        {
            movementShooter.enabled = false; // Disable the movement script to prevent further actions
        }
        if (movementMeele != null) // Check if movementMeele script exists
        {
            movementMeele.enabled = false; // Disable the meele movement script to prevent further actions
        }
        if (spriteRenderer.enabled) // Check if spriteRenderer is enabled
        {
            spriteRenderer.enabled = false; // Disable the sprite renderer to hide the enemy
        }

        _rb.velocity = Vector2.zero; // Stop the movement of the enemy

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
