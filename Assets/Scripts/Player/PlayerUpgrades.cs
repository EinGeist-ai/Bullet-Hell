using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    [Header("Possesion Of Upgrades")]
    public bool hasHealthUpgrade = false; // Flag to check if player has health upgrade | Done
    public bool hasShieldUpgrade = false; // Flag to check if player has shield upgrade | Done
    public bool hasSpeedUpgrade = false; // Flag to check if player has speed upgrade | Done
    public bool hasDamageUpgrade = false; // Flag to check if player has damage upgrade | Done
    public bool hasBulletSpeedUpgrade = false; // Flag to check if player has bullet speed upgrade | Done
    public bool hasBulletSizeUpgrade = false; // Flag to check if player has bullet size upgrade | Done
    public bool hasBulletPierceUpgrade = false; // Flag to check if player has bullet pierce upgrade | Done
    public bool hasBulletExplosiveUpgrade = false; // Flag to check if player has bullet explosive upgrade
    public bool hasBulletHomingUpgrade = false; // Flag to check if player has bullet homing upgrade
    public bool hasBulletCountUpgrade = false; // Flag to check if player has bullet count upgrade
    public bool hasBulletCooldownUpgrade = false; // Flag to check if player has bullet cooldown upgrade

    [Header("Upgrade Values")]
    public float upgradedMaxHealth = 3f; // Maximum health of the player
    public float shieldRegeneration = 5f; // Regeneration time of shield in seconds
    public float speedIncrease = 0f; // Flat addition for speed upgrade
    public float damageIncrease = 0f; // Flat addition for damage upgrade
    public float bulletSpeedIncrease = 0f; // Flat addition for bullet speed upgrade
    public float bulletSizeIncrease = 0f; // Flat addition for bullet size upgrade
    public float bulletPierceIncrease = 0f; // Flat addition for bullet pierce upgrade
    public float bulletExplosiveRadius = 2f; // Radius for explosive bullet upgrade
    public float bulletHomingStrength = 1f; // Strength for homing bullet upgrade
    public int bulletCountIncrease = 1; // Number of bullets for bullet count upgrade
    public float bulletCooldownReduction = 0.5f; // Cooldown reduction for bullet cooldown upgrade

    
    public void ApplyUpgrade(string upgradeName)
    {
        switch (upgradeName)
        {
            case "HealthUpgrade":
                if (!hasHealthUpgrade)
                {
                    hasHealthUpgrade = true;
                }
                else
                {
                    upgradedMaxHealth++;
                }
                break;

            case "ShieldUpgrade":
                if (!hasShieldUpgrade)
                {
                    hasShieldUpgrade = true;
                }
                else
                {
                    shieldRegeneration -= 0.5f;
                }
                break;

            case "SpeedUpgrade":
                if (!hasSpeedUpgrade)
                {
                    hasSpeedUpgrade = true;
                }
                else
                {
                    speedIncrease += 0.5f;
                }
                break;

            case "DamageUpgrade":
                if (!hasDamageUpgrade)
                {
                    hasDamageUpgrade = true;
                }
                else
                {
                    damageIncrease += 1f;
                }
                break;

            case "BulletSpeedUpgrade":
                if (!hasBulletSpeedUpgrade)
                {
                    hasBulletSpeedUpgrade = true;
                }
                else
                {
                    bulletSpeedIncrease += 5f;
                }
                break;

             case "BulletSizeUpgrade":
                if (!hasBulletSizeUpgrade)
                {
                    hasBulletSizeUpgrade = true;
                }
                else
                {
                    bulletSizeIncrease += 0.1f;
                }
                break;
             case "BulletPierceUpgrade":
                if (!hasBulletPierceUpgrade)
                {
                    hasBulletPierceUpgrade = true;
                }
                else
                {
                    bulletPierceIncrease += 1f;
                }
                break;

            default:
                Debug.LogWarning($"Upgrade {upgradeName} not recognized.");
                break;
        }
    }
}

