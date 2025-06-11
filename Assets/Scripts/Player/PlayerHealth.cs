using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health Settings")]
    public float maxHealth = 3;
    private float health;

    [Header("Refrences")]
    public PlayerUpgrades playerUpgrades;
    
    
    void Start()
    {
        health = maxHealth;
        playerUpgrades = GetComponent<PlayerUpgrades>();
    }

    
    void Update()
    {
        if(playerUpgrades != null && playerUpgrades.hasHealthUpgrade)
        {
            maxHealth = playerUpgrades.upgradedMaxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        Destroy(gameObject);
    }

    public void Heal(int amount)
    {
        health += amount;
        Debug.Log("Player healed! Current health: " + health);
    }

    public void HealthUpgrade(int upgradeAmount)
    {
        maxHealth += upgradeAmount;
        Debug.Log("Player health upgraded! Current health: " + health);
    }
}
