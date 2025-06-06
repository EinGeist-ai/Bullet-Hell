using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    private Rigidbody2D rb;
    public float damage = 1f; // Damage amount
    public GameObject ExplosionPrefab;
    public AudioSource audioData;
    public CapsuleCollider2D collider; // Reference to the CapsuleCollider2D component
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioData = GetComponent<AudioSource>();
        collider = GetComponent<CapsuleCollider2D>(); // Get the CapsuleCollider2D component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
    }

    // OnCollisionEnter2D is called when this collider/rigidbody has begun touching another rigidbody/collider
    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage((int)damage);
            Debug.Log("Dealt " + damage + " damage to player.");
            if (ExplosionPrefab != null)
            {
                audioData.Play();
                GameObject newObject = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
                newObject.transform.localScale = new Vector3(5f, 5f, -1f); // Set the scale of the explosion


            }
            spriteRenderer.enabled = false; // Disable the sprite renderer to hide the enemy
            collider.enabled = false; // Disable the collider to prevent further collisions
            Destroy(gameObject, audioData.clip.length); // Destroy the enemy after dealing damage
        }
    }
}
