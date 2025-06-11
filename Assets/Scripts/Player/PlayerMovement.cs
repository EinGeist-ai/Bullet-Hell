using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Player Movement Settings")]
    public float speed = 5f;
    public float accelerationTime = 0.1f;
    public bool AutoFire = false;

    [Header("References")]
    public GameObject crossHair;
    public PlayerUpgrades playerUpgrades;
    private Rigidbody2D body;
    private Vector2 movementInput;
    private Vector2 currentVelocity;
    private Quaternion playerRotation;
    public GameObject console;

    void Start()
    {
        
        playerUpgrades = GetComponent<PlayerUpgrades>();
        body = GetComponent<Rigidbody2D>();
        crossHair = GameObject.FindGameObjectWithTag("Crosshair");
        if (body == null)
        {
            Debug.LogError("Rigidbody2D component not found on the player object.");
        }

        if (crossHair == null)
        {
            Debug.LogWarning("Crosshair GameObject is not assigned.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !console.activeSelf)
        {
            AutoFire = !AutoFire;
        }
        if (playerUpgrades != null && playerUpgrades.hasSpeedUpgrade)
        {
            speed = playerUpgrades.speedIncrease;
        }
        if(!console.activeSelf)
        {
            MoveCrosshair(); 
        }
        

        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.A) && !console.activeSelf) moveX = -1f;
        if (Input.GetKey(KeyCode.D) && !console.activeSelf) moveX = 1f;
        if (Input.GetKey(KeyCode.W) && !console.activeSelf) moveY = 1f;
        if (Input.GetKey(KeyCode.S) && !console.activeSelf) moveY = -1f;

        movementInput = new Vector2(moveX, moveY).normalized;

        if (AutoFire == false)
        {
            RotateTowardsMouseOnly();
        }
        else
        {
            RotateTowardsEnemy();
        }
    }

    void FixedUpdate()
    {
        Vector2 targetVelocity = movementInput * speed;
        body.velocity = Vector2.SmoothDamp(body.velocity, targetVelocity, ref currentVelocity, accelerationTime);
    }

    private void MoveCrosshair()
    {
        if (crossHair != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            crossHair.transform.position = mousePosition;
        }
    }

    private void RotateTowardsEnemy()
    {
        GameObject closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            Vector2 enemyPosition = closestEnemy.transform.position;
            Vector2 direction = enemyPosition - body.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
            playerRotation = Quaternion.Euler(0f, 0f, angle);

            transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, Time.deltaTime * 20f);
        }
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] enemyBosses = GameObject.FindGameObjectsWithTag("EnemyBoss");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(body.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        foreach (GameObject enemyBoss in enemyBosses)
        {
            float distance = Vector2.Distance(body.position, enemyBoss.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemyBoss;
            }
        }

        return closestEnemy;
    }

    private void RotateTowardsMouseOnly()
    {
        if (!AutoFire)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - body.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
            playerRotation = Quaternion.Euler(0f, 0f, angle);

            transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, Time.deltaTime * 20f);
        }
    }
}
