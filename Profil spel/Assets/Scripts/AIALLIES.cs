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

        DetectEnemies();  // Ensure enemies are detected

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

        Debug.Log("AI is following the player.");
    }

    void AttackEnemy()
    {
        if (currentEnemy == null)
        {
            Debug.Log("No current enemy to attack.");
            return;
        }

        Vector2 direction = (currentEnemy.transform.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        if (animator != null)
            animator.SetTrigger("attack");

        // Check if the AI is within attack range and cooldown has passed
        if (Vector2.Distance(transform.position, currentEnemy.transform.position) < attackRange && attackTimer <= 0)
        {
            Debug.Log("In attack range and ready to shoot!");
            Shoot();  // AI now shoots!
            attackTimer = attackCooldown;  // Reset cooldown
        }

        attackTimer -= Time.deltaTime;  // Countdown the attack timer
    }

    void DefendPlayer()
    {
        if (player == null || playerHealth == null) return;

        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        rb.velocity = -directionToPlayer * moveSpeed;

        if (animator != null)
            animator.SetBool("isWalking", true);

        Debug.Log("AI is defending the player.");
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

            Debug.Log("AI is healing the player.");
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
                currentState = State.AttackEnemy;  // Switch to attack state
                Debug.Log("Enemy detected. Switching to AttackEnemy state.");
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
            Debug.Log("Player health is low. Switching to HealPlayer state.");
        }
        else if (Vector2.Distance(transform.position, player.position) < defendRange)
        {
            currentState = State.DefendPlayer;
            Debug.Log("AI is in defend range. Switching to DefendPlayer state.");
        }
        else
        {
            currentState = State.FollowPlayer;
            Debug.Log("AI is in follow mode. Switching to FollowPlayer state.");
        }
    }

    void Shoot()
    {
        if (currentEnemy == null || firePoint == null)
        {
            Debug.Log("No enemy or firePoint not assigned.");
            return;
        }

        Vector2 firePosition = firePoint.position;
        Vector2 enemyPosition = currentEnemy.transform.position;
        Vector2 direction = (enemyPosition - firePosition).normalized;
        float maxShootDistance = 10f;

        Debug.Log("Shooting at enemy. FirePoint: " + firePosition + ", Enemy Position: " + enemyPosition);

        RaycastHit2D hit = Physics2D.Raycast(firePosition, direction, maxShootDistance, LayerMask.GetMask("Enemy"));

        if (hit.collider != null)
        {
            Debug.Log("AI hit " + hit.collider.name);

            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(50); // Damage to enemy
            }
        }
        else
        {
            Debug.Log("No hit. Raycast didn't detect an enemy.");
        }

        Debug.DrawRay(firePosition, direction * maxShootDistance, Color.red, 0.1f); // Debug line
    }
}
