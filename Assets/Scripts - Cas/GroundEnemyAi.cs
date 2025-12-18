using UnityEngine;
using System.Collections;


public class GroundEnemyAi : MonoBehaviour
{
 [Header("Patrol Settings")]
public float moveSpeed = 2f;         
// HOW FAST THE ENEMY WALKS DURING PATROL

public float patrolDistance = 3f;    
// HOW FAR THE ENEMY WALKS BEFORE TURNING AROUND

public float waitTime = 1f;          
// HOW LONG THE ENEMY WAITS WHEN IT REACHES THE END OF PATROL PATH


[Header("Shooting Settings")]
public GameObject bulletPrefab;      
// THE BULLET OBJECT THAT THE ENEMY SHOOTS

public Transform firePoint;          
// THE POSITION WHERE BULLETS SPAWN (TYPICALLY IN FRONT OF ENEMY)

public float fireRate = 1.5f;          
// HOW MANY BULLETS THE ENEMY FIRES PER SECOND

public float visionRange = 5f;       
// HOW FAR THE ENEMY CAN SEE THE PLAYER TO START SHOOTING



[Header("References")]
public Transform player;             
// THE PLAYER OBJECT (SET IN THE INSPECTOR)

private Vector3 startPos;
// THE POSITION WHERE THE ENEMY STARTS PATROLLING FROM




private bool movingRight = false;
// TRACKS WHETHER THE ENEMY IS MOVING RIGHT OR LEFT

private bool isPatrolling = true;
// TRACKS IF THE ENEMY IS CURRENTLY PATROLLING

private bool isShooting = false;
// TRACKS IF THE ENEMY IS CURRENTLY SHOOTING

private SpriteRenderer SR;
// REFERENCE TO THE ENEMY’S SPRITE TO FLIP IT LEFT/RIGHT


//ADD A REFERENCE TO THE TARGETPOSITION//


public Vector2 PatrolEnding;


public bool isAlive = true;





void Start()
{
    //THIS LINE SAVES THE INITIAL POSITION OF THE PATROL//
    startPos = transform.position;
    // SAVE THE INITIAL POSITION AS THE START OF PATROL

    StartCoroutine(PatrolRoutine());
    // START THE PATROL BEHAVIOR, ENEMY STARTS PATROLLING//

    SR = GetComponent<SpriteRenderer>();
    // GET THE SPRITE RENDERER COMPONENT TO FLIP THE SPRITE
}


void Update()
{
    if (player != null)
    // IF THE PLAYER EXISTS IN THE SCENE
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        // CALCULATE HOW FAR THE PLAYER IS FROM THE ENEMY

        if (distanceToPlayer <= visionRange)
        // IF THE PLAYER IS WITHIN VISION RANGE
        {
            //MAKE THE ENEMY STOP PATROLLING, STOP IN THEIR PLACE AND PREPARE TO SHOOT AT THE PLAYER//
            if (isPatrolling)
            {

                isPatrolling = false;
                StopAllCoroutines();
                // STOP PATROLLING IF CURRENTLY PATROLLING
            }

            //SET LOGIC FOR SHOOTING//
            if (!isShooting)
            {
                isShooting = true;
                StartCoroutine(ShootRoutine());
                // START SHOOTING IF NOT ALREADY SHOOTING
            }
        }
        else
        {
            // IF PLAYER IS OUT OF VISION RANGE, GO BACK PATROLLING//
            if (!isPatrolling)
            {
                isPatrolling = true;
                isShooting = false;
                StopAllCoroutines();
                StartCoroutine(PatrolRoutine());
                // STOP SHOOTING AND RESUME PATROL
            }
        }
    }
}


IEnumerator PatrolRoutine()
{
    while (isPatrolling)
    // LOOP THIS AS LONG AS PATROLLING IS TRUE
    {

        //STARTPOS = THE STARTING POSITION OR ORIGINAL POSITION OF THE PATROL POSITION//
        //(movingRight ? Vector3.right : Vector3.left) MEANS:

        //IF MOVING RIGHT IS TRUE -> USE VECTOR3.RIGHT (1,0,0)

        //IF MOVING LEFT IS TRUE -> USE VECTOR3.LEFT(-1,0,0)

        //THIS LINE CALCUATES WHERE THE ENEMY SHOULD MOVE NEXT//

        Vector3 targetPos = startPos + (movingRight ? Vector3.right : Vector3.left) * patrolDistance;
        // CALCULATE THE NEXT TARGET POSITION (LEFT OR RIGHT FROM START)


        PatrolEnding = targetPos;


        //THIS WHILE LOOP MOVES THE ENEMY CLOSE TO THE TARGETPOS
       
        //MOVES ENEMY TOWARD THE TARGETPOSITION EACH FRAME//
        
        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        // MOVE TOWARD THE TARGET POSITION UNTIL CLOSE ENOUGH
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            // MOVE THE ENEMY TOWARDS TARGET POSITION AT moveSpeed

            yield return null;
            // WAIT FOR NEXT FRAME BEFORE CONTINUING LOOP
        }

        yield return new WaitForSeconds(waitTime);
        // ONCE TARGET REACHED, WAIT FOR A MOMENT

        movingRight = !movingRight;
        // CHANGE DIRECTION (IF WAS MOVING RIGHT, NOW MOVE LEFT, AND VICE VERSA)

        if (SR != null)
        {
            SR.flipX = movingRight;
            // FLIP THE SPRITE HORIZONTALLY BASED ON DIRECTION
        }
    }
}


IEnumerator ShootRoutine()
{
    while (isShooting)
    // LOOP THIS AS LONG AS SHOOTING IS TRUE
    {
        if (bulletPrefab != null && firePoint != null && player != null)
        // MAKE SURE ALL NECESSARY OBJECTS EXIST
        {
            Vector3 dir = (player.position - firePoint.position).normalized;
            // CALCULATE DIRECTION VECTOR FROM FIRE POINT TO PLAYER

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            // CALCULATE THE ANGLE TO ROTATE THE BULLET TO FACE THE PLAYER

            Quaternion rot = Quaternion.Euler(0f, 0f, angle);
            // CREATE ROTATION BASED ON THAT ANGLE

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rot);
            // CREATE A BULLET INSTANCE AT FIREPOINT FACING PLAYER

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            // GET THE BULLET’S RIGIDBODY TO APPLY VELOCITY

            if (rb != null)
            {
                rb.linearVelocity = dir * rb.linearVelocity.magnitude;
                // SET BULLET VELOCITY TO MOVE TOWARD PLAYER WITH BULLET’S SPEED
            }
        }
        yield return new WaitForSeconds(fireRate);
        // WAIT BEFORE FIRING THE NEXT BULLET BASED ON FIRE RATE
    }
}


private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    // IF THE PLAYER COLLIDES WITH THE ENEMY
    {
        PlayerTele tele = collision.GetComponent<PlayerTele>();
        // GET THE PLAYER'S TELEPORT SCRIPT TO CHECK IF TELEPORTING

        if (tele != null)
        {
            if (tele.isTeleporting)
            // IF PLAYER IS TELEPORTING THROUGH THE ENEMY
            {
                Destroy(gameObject);

                Debug.Log("Enemy killed via teleport");
                // DESTROY THE ENEMY AND PRINT DEBUG MESSAGE

                


            }
            else 
            {
                PlayerHealth health = tele.GetComponent<PlayerHealth>();
                if (health != null)
                    health.TakeDamage(1);
                Debug.Log("Player took 1 damage because they ran into the enemy");
                // IF PLAYER IS NOT TELEPORTING, DAMAGE THE PLAYER INSTEAD
            }
        }
    }
}
}