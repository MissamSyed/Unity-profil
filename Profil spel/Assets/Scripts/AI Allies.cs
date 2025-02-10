using UnityEngine;

public class AIAllies : MonoBehaviour
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
    public float moveSpeed = 3f; // Hastighet för AI Allies

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
        if (playerHealth == null) return; // Early exit if playerHealth is not assigned.

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

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < attackRange)
        {
            currentState = State.AttackEnemy;
        }
        else if (distance > followRange)
        {
            currentState = State.FollowPlayer;
        }
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
            currentEnemy.TakeDamage(10);
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

        if (playerHealth.currentHealth < healThreshold)
        {
            playerHealth.Heal(5); // Heals the player by 5 units

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

        if (playerHealth.currentHealth < healThreshold)
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
}
