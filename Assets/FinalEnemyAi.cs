using UnityEngine;

public class FinalEnemyAi : MonoBehaviour
{
    public Transform player;                  // Player position
    public float moveSpeed = 3f;              // How fast the enemy moves
    public float stopDistance = 6f;           // Stop & shoot distance
    public float shootingCooldown = 1.5f;     // Time between shots
    public float pauseAfterShooting = 2f;     // Pause before moving after shooting

    private float nextShootTime = 0f;
    private float pauseTimer = 0f;

    // Bullet settings
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 10f;


    public PlayerHealth PH;

    public EnemySpawns ES;

    public FinalArea FA;
   





    void Awake()
    {
        
        FA = GameObject.Find("City Door").GetComponent<FinalArea>();

        PH = GameObject.Find("Player").GetComponent<PlayerHealth>();

        player = GameObject.Find("Player").GetComponent<Transform>();

        ES = GameObject.Find("Final Area Enemy Spawn Manager").GetComponent<EnemySpawns>();
    
    
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // If player is within shooting range and enemies can spawn
        if (distance <= stopDistance && FA.EnemiesCanSpawn)
        {
            ShootPlayer();

            // Reset pause timer every time enemy shoots
            pauseTimer = pauseAfterShooting;
        }
        // If player is out of range but pause timer > 0, stay in place
        else if (pauseTimer > 0f)
        {
            pauseTimer -= Time.deltaTime;
        }
        // If player is out of range and pause timer finished, move toward player
        else if (distance > stopDistance && FA.EnemiesCanSpawn)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    void ShootPlayer()
    {
        if (Time.time >= nextShootTime)
        {
            nextShootTime = Time.time + shootingCooldown;

            // Spawn the bullet
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

            // Calculate direction to player
            Vector2 direction = (player.position - bulletSpawn.position).normalized;

            // Rotate bullet to face the player
            bullet.transform.right = direction;

            // Move the bullet in the direction of the player
            bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletSpeed;
        }
    }


private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        PlayerTele tele = collision.GetComponent<PlayerTele>();
        if (tele != null)
        {
            if (tele.isTeleporting)
            {
                ES.CountKills();
               
                Destroy(gameObject);
                
                Debug.Log("Enemy killed via teleport");

                PH.Heal(1);

              

         
                
                // STOP processing, so player doesn't take damage
                return;
            }
            else
            {
                PH.TakeDamage(1);
                Debug.Log("Player took 1 damage from enemy collision");
            }
        }
    }
}





}