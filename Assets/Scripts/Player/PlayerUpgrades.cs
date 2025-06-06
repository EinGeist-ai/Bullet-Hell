using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    [Header("Possesion Of Upgrades")]
    public bool hasHealthUpgrade = false; // Flag to check if player has health upgrade
    public bool hasShieldUpgrade = false; // Flag to check if player has shield upgrade
    public bool hasSpeedUpgrade = false; // Flag to check if player has speed upgrade
    public bool hasDamageUpgrade = false; // Flag to check if player has damage upgrade
    public bool hasBulletSpeedUpgrade = false; // Flag to check if player has bullet speed upgrade
    public bool hasBulletSizeUpgrade = false; // Flag to check if player has bullet size upgrade
    public bool hasBulletPierceUpgrade = false; // Flag to check if player has bullet pierce upgrade
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
