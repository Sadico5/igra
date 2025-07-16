using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float detectionRadius = 5f;
    public float attackRange = 5f;

    [Header("Shooting Settings")]
    public GameObject enemyBulletPrefab;
    public float fireInterval = 1f;
    private float fireTimer = 0f;

    [Header("Obstacle Avoidance")]
    public LayerMask wallLayerMask;

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        FindPlayer();
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
            if (player == null)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }
        }

        Vector2 toPlayer = player.position - transform.position;
        float distance = toPlayer.magnitude;
        Vector2 direction = toPlayer.normalized;

        RaycastHit2D wallHit = Physics2D.Raycast(transform.position, direction, distance, wallLayerMask);
        bool clearLineOfSight = (wallHit.collider == null);

        if (distance <= detectionRadius && distance > attackRange)
        {
            if (!clearLineOfSight)
            {
                Vector2 wallNormal = wallHit.normal;
                Vector2 perp = Vector2.Perpendicular(wallNormal);
                if (Vector2.Dot(perp, direction) < 0)
                    perp = -perp;
                Vector2 steerDirection = (direction + perp * 0.5f).normalized;
                rb.linearVelocity = steerDirection * moveSpeed;
            }
            else
            {
                rb.linearVelocity = direction * moveSpeed;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (distance <= attackRange && clearLineOfSight)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireInterval)
            {
                ShootPlayer();
                fireTimer = 0f;
            }
        }
    }

    void ShootPlayer()
    {
        if (enemyBulletPrefab == null)
            return;

        GameObject bullet = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
        EnemyBulletController bulletCtrl = bullet.GetComponent<EnemyBulletController>();
        if (bulletCtrl != null)
            bulletCtrl.SetTarget(player);
    }
}