using UnityEngine;

public class AIALLIES : MonoBehaviour
{
    public enum State
    {
        FollowPlayer,
        AttackEnemy,
        DefendPlayer,
        HealPlayer
    }

    public State currentState;
    public Transform player;
    public float attackRange = 2f;
    public float followRange = 10f;
    public float defendRange = 5f;
    public float healThreshold = 30f;
    public float attackCooldown = 1f;
    private float attackTimer;

    private Rigidbody2D rb;
    private Animator animator;
    private Enemy currentEnemy;
    private PlayerHealth playerHealth;
    public float moveSpeed = 3f;

    // For shooting (hitscan)
    public GameObject bulletPrefab; // Assign in Inspector
    public Transform firePoint; // Create an empty GameObject in front of the AI and assign it here
    public float bulletSpeed = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentState = State.FollowPlayer;

        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (playerHealth == null) return;

        switch (currentState)
        {
            case State.FollowPlayer:
                FollowPlayer();
                break;
            case State.AttackEnemy:
                AttackEnemy();
                break;
            case State.DefendPlayer:
                DefendPlayer();
                break;
            case State.HealPlayer:
                HealPlayer();
                break;
        }

        EvaluateState();
    }

    void FollowPlayer()
    {
        if (player == null || playerHealth == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        if (animator != null)
            animator.SetBool("isWalking", true);
    }

    void AttackEnemy()
    {
        if (currentEnemy == null) return;

        Vector2 direction = (currentEnemy.transform.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        if (animator != null)
            animator.SetTrigger("attack");

        if (Vector2.Distance(transform.position, currentEnemy.transform.position) < attackRange && attackTimer <= 0)
        {
            Shoot();  // AI now shoots!
            attackTimer = attackCooldown;
        }

        attackTimer -= Time.deltaTime;
    }

    void DefendPlayer()
    {
        if (player == null || playerHealth == null) return;

        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        rb.velocity = -directionToPlayer * moveSpeed;

        if (animator != null)
            animator.SetBool("isWalking", true);
    }

    void HealPlayer()
    {
        if (player == null || playerHealth == null) return;

        if (playerHealth.currentPlayerHealth < healThreshold)
        {
            playerHealth.currentPlayerHealth += 5;
            if (playerHealth.currentPlayerHealth > playerHealth.maxHealth)
                playerHealth.currentPlayerHealth = playerHealth.maxHealth;

            if (animator != null)
                animator.SetTrigger("heal");
        }
    }

    void DetectEnemies()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, followRange);
        foreach (var enemyCollider in enemiesInRange)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                currentEnemy = enemy;
                currentState = State.AttackEnemy;
                break;
            }
        }
    }

    void EvaluateState()
    {
        if (playerHealth == null) return;

        if (playerHealth.currentPlayerHealth < healThreshold)
        {
            currentState = State.HealPlayer;
        }
        else if (Vector2.Distance(transform.position, player.position) < defendRange)
        {
            currentState = State.DefendPlayer;
        }
        else
        {
            currentState = State.FollowPlayer;
        }
    }

    void Shoot()
    {
        if (currentEnemy == null || firePoint == null) return;

        Vector2 firePosition = firePoint.position;
        Vector2 enemyPosition = currentEnemy.transform.position;
        Vector2 direction = (enemyPosition - firePosition).normalized;
        float maxShootDistance = 10f;

        RaycastHit2D hit = Physics2D.Raycast(firePosition, direction, maxShootDistance, LayerMask.GetMask("Enemy"));

        if (hit.collider != null)
        {
            Debug.Log("AI hit " + hit.collider.name);

            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(10); // Damage to enemy
            }
        }

        Debug.DrawRay(firePosition, direction * maxShootDistance, Color.red, 0.1f); // Debug line
    }
}
