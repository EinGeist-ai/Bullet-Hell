using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{

    [Header("References")]
    private Rigidbody2D rb;
    public GameObject ExplosionPrefab;
    public AudioSource audioData;
    public CapsuleCollider2D capsuleColider;
    private SpriteRenderer spriteRenderer;

    [Header("Default Damage")]
    public float damage = 1f;
    
    

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioData = GetComponent<AudioSource>();
        capsuleColider = GetComponent<CapsuleCollider2D>(); 
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    
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
                newObject.transform.localScale = new Vector3(5f, 5f, -1f); 


            }
            spriteRenderer.enabled = false; 
            capsuleColider.enabled = false; 
            Destroy(gameObject, audioData.clip.length);
        }
    }
}
