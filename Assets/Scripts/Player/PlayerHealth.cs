using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public PlayerUpgrades playerUpgrades; // Reference to the upgrade script if needed, can be set in the inspector
    public float maxHealth = 3; // Player's health
    private float health; // Current health of the player
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth; // Initialize health to maxHealth at the start
        playerUpgrades = GetComponent<PlayerUpgrades>(); // Get the upgrade script component if needed
    }

    // Update is called once per frame
    void Update()
    {
        if(playerUpgrades != null && playerUpgrades.hasHealthUpgrade) // Check if the player has health upgrade
        {
            maxHealth = playerUpgrades.upgradedMaxHealth; // Add flat health upgrade
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage; // Reduce health by damage amount
        if (health <= 0)
        {
            Die(); // Call Die method if health is 0 or less
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!"); // Log death message
        // Here you can add more logic for player death, like playing an animation or restarting the game
        // For now, we will just destroy the player object
        Destroy(gameObject);
    }

    public void Heal(int amount)
    {
        health += amount; // Increase health by amount
        Debug.Log("Player healed! Current health: " + health); // Log healing message
    }

    public void HealthUpgrade(int upgradeAmount)
    {
        maxHealth += upgradeAmount; // Increase health by upgrade amount
        Debug.Log("Player health upgraded! Current health: " + health); // Log upgrade message
    }
}
