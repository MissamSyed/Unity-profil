using UnityEngine;

public class AIALLIES : MonoBehaviour
{
    public enum State
    {
        FollowPlayer,
        AttackEnemy,
        HealPlayer
    }

    public State currentState;
    public Transform player;
    public float attackRange = 2f;
    public float followRange = 10f;
    public float healThreshold = 30f;
    public float attackCooldown = 1f;
    private float attackTimer;

    private Rigidbody2D rb;
    private Animator animator;
    private Enemy currentEnemy;
    private PlayerHealth playerHealth;
    public float moveSpeed = 3f;

    public Transform firePoint;
    public float bulletSpeed = 10f;
    public LineRenderer laserLine; // F�r visuell feedback

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

        attackTimer -= Time.deltaTime; // Countdown f�r attack
        DetectEnemies(); // Uppt�ck fiender

        switch (currentState)
        {
            case State.FollowPlayer:
                FollowPlayer();
                break;
            case State.AttackEnemy:
                AttackEnemy();
                break;
            case State.HealPlayer:
                HealPlayer();
                break;
        }
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
        if (currentEnemy == null)
        {
            currentState = State.FollowPlayer; // Om ingen fiende finns, f�lj spelaren
            return;
        }

        float distanceToEnemy = Vector2.Distance(transform.position, currentEnemy.transform.position);

        if (distanceToEnemy < attackRange && attackTimer <= 0)
        {
            Shoot(); // Skjut om vi �r n�ra och cooldown �r redo
            attackTimer = attackCooldown;
        }
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

            Debug.Log("AI heals player.");
        }
    }

    void DetectEnemies()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, followRange, LayerMask.GetMask("Enemy"));
        float closestDistance = Mathf.Infinity; // Starta med o�ndligt stort avst�nd
        Enemy nearestEnemy = null;

        foreach (var enemyCollider in enemiesInRange)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    nearestEnemy = enemy; // Uppdatera n�rmaste fiende
                }
            }
        }

        if (nearestEnemy != null)
        {
            currentEnemy = nearestEnemy; // S�tt den n�rmaste fienden som aktuell
            currentState = State.AttackEnemy; // Byt till AttackEnemy-state
            Debug.Log("Enemy detected. Switching to AttackEnemy state.");
        }
        else
        {
            // Om inga fiender hittades, g� tillbaka till FollowPlayer
            currentEnemy = null;
            currentState = State.FollowPlayer;
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

        RaycastHit2D hit = Physics2D.Raycast(firePosition, direction, maxShootDistance, LayerMask.GetMask("Enemy"));

        if (hit.collider != null)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(50);
            }
        }

        Debug.DrawRay(firePosition, direction * maxShootDistance, Color.red, 0.1f);
    }
}
