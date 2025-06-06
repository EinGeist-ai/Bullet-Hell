using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBehaviorShooter : MonoBehaviour
{
    public GameObject player;
    public float maxSpeed = 2f;
    public float stopDistance = 2f;
    public float smoothTime = 0.2f; // The smaller, the faster it accelerates/decelerates

    private Rigidbody2D rb;
    private Vector2 velocity = Vector2.zero; // For SmoothDamp

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            Debug.LogWarning("Player GameObject not assigned!");
        }
    }

    void Update()
    {
        if (player == null) return;

        RotateTowardsPlayer();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        MoveTowardsPlayerSmooth();
    }

    private void RotateTowardsPlayer()
    {
        Vector2 direction = (Vector2)player.transform.position - rb.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg +90;
        rb.rotation = angle;
    }

    private void MoveTowardsPlayerSmooth()
    {
        float distance = Vector2.Distance(rb.position, player.transform.position);
        Vector2 direction = ((Vector2)player.transform.position - rb.position).normalized;

        Vector2 targetVelocity = (distance > stopDistance) ? direction * maxSpeed : Vector2.zero;

        // SmoothDamp from current velocity to target velocity
        Vector2 smoothedVelocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smoothTime);

        rb.velocity = smoothedVelocity;
    }
}
