using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Default Settings")]
    public float bulletSpeed = 2f;
    public float fireRate = 1f;
    public float damage = 1f;
    public float bulletCount = 1f;
    public float bulletSize = 1f;
    public float maxTotalAngle = 90f;
    private float currentFireRate = 1;


    [Header("Refrences")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    private GameObject player;
    public GameObject console;
    public PlayerUpgrades playerUpgrades;
   


    private bool AutoFire = false;
    private bool canShoot = true;
    
    

    
    void Start()
    {
        if (CompareTag("Player"))
        {
            playerUpgrades = GetComponent<PlayerUpgrades>();
            ApplyUpgrades();
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void Update()
    {
        

        if (CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.R) && !console.activeSelf)
            {
                AutoFire = !AutoFire;
            }
            ApplyUpgrades();

            
            if (Input.GetButton("Fire1") && canShoot && !AutoFire && !console.activeSelf)
            {
                StartCoroutine(ShootCoroutine());
            }
            else if (AutoFire && canShoot && !console.activeSelf)
            {
                
                StartCoroutine(ShootCoroutine());
            }
        }
        else if (CompareTag("EnemyBoss"))
        {
            
            if (canShoot && player != null)
            {
                StartCoroutine(ShootCoroutine());
            }
        }
        else if (CompareTag("Enemy"))
        {
            
            if (canShoot && player != null)
            {
                StartCoroutine(ShootCoroutine());
            }
        }
    }

    private void ApplyUpgrades()
    {
        if (playerUpgrades == null) return;

        if (playerUpgrades.hasBulletSpeedUpgrade)
        {
            bulletSpeed = Mathf.Min(199f, playerUpgrades.bulletSpeedIncrease);
        }

        if (playerUpgrades.hasDamageUpgrade)
        {
            damage = playerUpgrades.damageIncrease;
        }

        if (playerUpgrades.hasBulletCountUpgrade)
        {
            bulletCount = Mathf.Max(1, playerUpgrades.bulletCountIncrease);
        }

        if (playerUpgrades.hasBulletCooldownUpgrade)
        {
            currentFireRate = Mathf.Max(0.01f, playerUpgrades.bulletCooldownReduction);
        }
        else
        {
            currentFireRate = fireRate;
        }

        if (playerUpgrades.hasBulletSizeUpgrade)
        {
            bulletSize = Mathf.Max(0.1f, playerUpgrades.bulletSizeIncrease);
        }
        else
        {
            bulletSize = 1f; // Setze auf Standardwert, wenn Upgrade nicht vorhanden ist
        }
    }

    private IEnumerator ShootCoroutine()
    {
        canShoot = false;
        if(CompareTag("Player"))
        { FireBullets(); }
        else if (CompareTag("EnemyBoss") || CompareTag("Enemy"))
        {
            
            Vector2 directionToPlayer = (player.transform.position - firePoint.position).normalized;
            FireBullets(directionToPlayer);
        }
        yield return new WaitForSeconds(currentFireRate);
        canShoot = true;
    }

    void FireBullets(Vector2 directionOverride = default)
    {
        int count = Mathf.Max(1, Mathf.RoundToInt(bulletCount));

        // Berechne den Winkel für die Verteilung basierend auf einem festen Winkel von 10° und maxTotalAngle
        float angleStep = Mathf.Min(10f, maxTotalAngle / Mathf.Max(1, count - 1)); // Abstand zwischen den Kugeln in Grad
        float totalAngle = angleStep * (count - 1); // Gesamter Winkel basierend auf der Anzahl der Kugeln
        float startAngle = -totalAngle / 2; // Startwinkel für die erste Kugel
        if (count == 1)
        {
            startAngle = 0f; // Wenn nur eine Kugel, dann kein Winkelversatz
        }

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + i * angleStep;
            Quaternion fireRotation = firePoint.rotation * Quaternion.Euler(0, 0, angle);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, fireRotation);
            bullet.transform.localScale *= bulletSize;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                if (directionOverride != default)
                {
                    // Setze die Geschwindigkeit und Rotation der Kugel in Richtung des Ziels
                    rb.velocity = directionOverride * bulletSpeed;
                    float angleToPlayer = Mathf.Atan2(directionOverride.y, directionOverride.x) * Mathf.Rad2Deg;
                    bullet.transform.rotation = Quaternion.AngleAxis(angleToPlayer, Vector3.forward);
                }
                else
                {
                    rb.velocity = fireRotation * Vector2.up * bulletSpeed;
                    bullet.transform.rotation = fireRotation;
                }

                bulletScript bulletScript = bullet.GetComponent<bulletScript>();
                if (bulletScript != null)
                {
                    bulletScript.damage = damage;
                    bulletScript.speed = bulletSpeed;
                }
            }
        }
    }
}