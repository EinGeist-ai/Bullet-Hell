using System.Collections;
using System.Collections.Generic;
using System.Linq; // Für LINQ-Operationen
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 2f;
    public float fireRate = 1f; // Base fire rate
    public float damage = 1f;
    public PlayerUpgrades playerUpgrades;
    public float bulletCount = 1f;
    public float bulletSize = 1f;
    public float maxTotalAngle = 90f; // Maximum total spread angle for all bullets
    public bool AutoFire = false; // For player shooting behavior

    private bool canShoot = true;
    private float currentFireRate = 1; // Adjusted fire rate based on upgrades
    private GameObject player; // Reference to the player for enemy targeting

    void Start()
    {
        if (CompareTag("Player"))
        {
            playerUpgrades = GetComponent<PlayerUpgrades>();
            ApplyUpgrades();
        }
        else
        {
            // Find the player GameObject in the scene for enemy targeting
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            AutoFire = !AutoFire; // Toggle AutoFire mode
        }

        if (CompareTag("Player"))
        {
            ApplyUpgrades();

            // Player-controlled shooting
            if (Input.GetButton("Fire1") && canShoot && !AutoFire)
            {
                StartCoroutine(ShootCoroutine());
            }
            else if (AutoFire && canShoot)
            {
                // Auto fire when AutoFire is enabled
                StartCoroutine(ShootCoroutine());
            }
        }
        else if (CompareTag("EnemyBoss"))
        {
            // Boss-controlled automatic shooting
            if (canShoot && player != null)
            {
                StartCoroutine(ShootCoroutine());
            }
        }
        else if (CompareTag("Enemy"))
        {
            // Enemy-controlled automatic shooting
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
            currentFireRate = Mathf.Max(0f, playerUpgrades.bulletCooldownReduction);
        }
        else
        {
            currentFireRate = fireRate;
        }
    }

    private IEnumerator ShootCoroutine()
    {
        canShoot = false;
        if(CompareTag("Player"))
        { FireBullets(); }
        else if (CompareTag("EnemyBoss") || CompareTag("Enemy"))
        {
            // If it's an enemy or boss, target the player
            Vector2 directionToPlayer = (player.transform.position - firePoint.position).normalized;
            FireBullets(directionToPlayer);
        }
        yield return new WaitForSeconds(currentFireRate);
        canShoot = true;
    }

    void FireBullets(Vector2 directionOverride = default)
    {
        int count = Mathf.Max(1, Mathf.RoundToInt(bulletCount));

        // Berechne den Winkel für die Verteilung basierend auf maxTotalAngle
        float angleStep = count > 1 ? maxTotalAngle / (count - 1) : 0f; // Abstand zwischen den Kugeln in Grad
        float startAngle = -maxTotalAngle / 2; // Startwinkel für die erste Kugel
        if (count == 1)
        {
            startAngle = 0f; // Wenn nur eine Kugel, dann kein Winkelversatz
        }

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + i * angleStep;
            float bulletDirection = startAngle + i * angleStep + 90; // Berechne den Winkel für die Kugel
            Quaternion fireRotation = firePoint.rotation * Quaternion.Euler(0, 0, angle);
            

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, fireRotation);
            bullet.transform.localScale *= bulletSize; // Skalierung der Kugel basierend auf bulletSize

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