using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;

public class PlayerTele : MonoBehaviour
{

      private Camera cam; // REFERENCE TO THE MAIN CAMERA FOR GETTING MOUSE WORLD POSITION

    [Header("Teleport")]
    public float PlayerRadius = 5f; // MAXIMUM TELEPORT DISTANCE FROM PLAYER

    public float teleportCooldown = 3f; // TIME BETWEEN TELEPORTS (COOLDOWN)
    public float nextTeleportTime = 1f; // TIME WHEN PLAYER CAN NEXT TELEPORT
    public static event Action<float> OnTeleportUsed; // EVENT TO NOTIFY UI OF COOLDOWN USAGE

    [Header("Effects")]
    public GameObject beamPrefab; // PREFAB USED TO DISPLAY TELEPORT BEAM
    public float beamDuration = 0.2f; // DURATION BEFORE BEAM IS DESTROYED
    public SpriteRenderer playerSprite; // REFERENCE TO THE PLAYER’S SPRITE (FOR HIDING/REAPPEARING)
    public float disappearTime = 0.1f; // TIME THE PLAYER STAYS INVISIBLE WHEN TELEPORTING

    [Header("Teleport State")]
    public bool isTeleporting = false; // WHETHER THE PLAYER IS CURRENTLY TELEPORTING
    public float teleportDuration = 0.2f; // DURATION THE PLAYER REMAINS IN A TELEPORT STATE

    [Header("Teleport Chain Settings")]
    public int maxChains = 3; // MAXIMUM NUMBER OF CHAINED TELEPORTS
    public float chainRadius = 5f; // HOW FAR TO LOOK FOR THE NEXT ENEMY FOR CHAINING
    public float chainDelay = 0.15f; // DELAY BETWEEN CHAINED TELEPORTS

    public int EnemiesDestoryed = 0;

    public bool Detectwall = false, canTeleport;

    public PlayerHealth PH;

    void Start()
    {
        cam = Camera.main;
    }

  void Update()
{
    // CHECK EVERY FRAME IF THE RIGHT MOUSE BUTTON IS PRESSED, TELEPORT COOLDOWN HAS PASSED, AND IF THE PLAYER CAN TELEPORT
    if (Input.GetMouseButtonDown(1) && Time.time >= nextTeleportTime && canTeleport)
    {
        // CALCULATE THE TELEPORT TARGET POSITION BASED ON THE MOUSE POSITION AND PLAYER RANGE
        Vector3 targetPos = GetTeleportPosition();
       
        // START THE TELEPORTATION SEQUENCE AS A COROUTINE PASSING THE TARGET POSITION
        StartCoroutine(TeleportSequence(targetPos));

        // SET THE NEXT TIME THE PLAYER CAN TELEPORT
        // Time.time IS THE CURRENT GAME TIME IN SECONDS SINCE THE GAME STARTED
        // ADD teleportCooldown TO ENFORCE TIME BETWEEN TELEPORTS
        nextTeleportTime = Time.time + teleportCooldown;

        // FIRE AN EVENT TO NOTIFY OTHER SYSTEMS OR UI THAT TELEPORT HAS BEEN USED
        OnTeleportUsed?.Invoke(teleportCooldown);

        // TEMPORARILY SUSPEND CAMERA MOVEMENT TO MAKE TELEPORT VISUALLY SMOOTHER
        FindFirstObjectByType<SmoothCameraFollow>().SuspendCamera(0.4f);
    }
}

IEnumerator TeleportSequence(Vector3 targetPos)  
// THIS FUNCTION HANDLES THE PLAYER TELEPORTING SEQUENCE, ENEMY BEING DESTORYED FROM TELEPORTING, CHAINS TELEPORTS TO ENEMIES CLOSEST TO PLAYER WITHIN A RADIUS, WALL DETECTION, AND VISUAL EFFECTS//
{
    isTeleporting = true; 
    // SETS A FLAG TO PREVENT OTHER ACTIONS WHILE TELEPORTING

    //CHECKS IF PLAYERSPITE EXISTS//
    if (playerSprite != null)

        // HIDES THE PLAYER SPRITE TO CREATE A TELEPORT DISAPPEAR EFFECT
        playerSprite.enabled = false; 

        
        // STORES THE CURRENT POSITION OF THE PLAYER OR GETS THE CURRENT POSITION//
        Vector3 currentPos = transform.position; 
    
    //THIS LOOP ALLOWS CHAINING BETWEEN MULTIPLE ENEMIES AND HANDLES THE ACTUAL TELEPORTING LOGIC//
    for (int i = 0; i < maxChains; i++) 

    {   
        //CHECKS IF THE TARGETPOS EQUALS THE CURRENTPOSITION
        if (targetPos == currentPos)
            
             // SLIGHTLY OFFSETTING THE TARGET POSITION -> THIS IS DONE BY MULTIPLYING BY 0.01F
            targetPos += Vector3.right * 0.01f; 
       
            // THIS CALCULATES THE CENTER OR MIDDLE BETWEEN THE CURRENT POSITION AND TARGET POSITION -> THIS IS DONE BY DIVIDING BY 2F
            Vector3 CenterofThePath = (currentPos + targetPos) / 2f; 
       
            // CALCULATES THE RADIUS OF THE CIRCLE USED TO DETECT COLLIDERS ALONG THE TELEPORT PATH, ADDS BUFFER TO ENSURE ENEMIES ARE HIT  
            float pathRadius = Vector3.Distance(currentPos, targetPos) / 2f + 0.1f; 
        
            // GETS ALL COLLIDERS (ENEMIES, GROUND, WALLS) WITHIN THE PATH RADIUS
            Collider2D[] hits = Physics2D.OverlapCircleAll(CenterofThePath, pathRadius); 
      
            // LOOP THROUGH EACH COLLIDER FOUND IN THE PATH
        foreach (Collider2D hit in hits) 
        
        {   
            
            // CHECK IF THE COLLIDER IS AN ENEMY
            if (hit.CompareTag("Enemy"))
            { 
                // GETS THE GROUNDENEMY SCRIPT ON THE COLLIDER//
                GroundEnemyAi enemy = hit.GetComponent<GroundEnemyAi>(); 
                 
               
                // CHECKS IF ENEMY EXISTS AND IS ALIVE
                if (enemy != null && enemy.isAlive) 
             
                {   
                    // INCREMENT THE COUNT OF DESTROYED ENEMIES
                    EnemiesDestoryed++; 
                 
                    // MARK ENEMY AS DEAD TO PREVENT DOUBLE COUNTING
                    enemy.isAlive = false; 
                    
                    // DESTROYS THE ENEMY GAME OBJECT
                    Destroy(enemy.gameObject); 


                    //HEAL A SMALL AMOUNT FOR KILLING ENEMIES WITH TELEPORTING//
                    PH.Heal(2);
                   
                }
            
            }


         // CHECK IF COLLIDER IS GROUND OR WALL
            else if (hit.CompareTag("Ground"))
           
            {  
                // SETS A FLAG INDICATING A WALL/GROUND WAS DETECTED
                Detectwall = true; 
     
            }
        }
        
        // CHECK IF TELEPORT BEAM VISUAL EXISTS
        if (beamPrefab != null)

        {   
            
            // CREATE THE BEAM GAME OBJECT
            GameObject beam = Instantiate(beamPrefab); 
      
            // GET THE LINE RENDERER COMPONENT TO DRAW THE BEAM
            LineRenderer line = beam.GetComponent<LineRenderer>(); 
          

            if (line != null)
            {

                // SET START OF LINE TO PLAYER'S CURRENT POSITION
                line.SetPosition(0, currentPos); 
            
                // SET END OF LINE TO TARGET POSITION
                line.SetPosition(1, targetPos); 
                
            }
                // DESTROY THE BEAM AFTER A SHORT TIME TO CLEAN UP
                Destroy(beam, beamDuration); 

        }

        // WAIT BEFORE ACTUALLY MOVING THE PLAYER (SIMULATES DISAPPEAR EFFECT)
        yield return new WaitForSeconds(disappearTime); 
       
        // CALCULATE DIRECTION FROM CURRENT POSITION TO TARGET
        Vector3 direction = (targetPos - currentPos).normalized; 
        
        
        // CALCULATE DISTANCE BETWEEN CURRENT POSITION AND TARGET
        float distance = Vector3.Distance(currentPos, targetPos); 
      
        
        // CAST A RAY TO DETECT ANY WALLS ALONG THE TELEPORT PATH
        RaycastHit2D[] wallHits = Physics2D.RaycastAll(currentPos, direction, distance); 
      
        // LOOP THROUGH ALL HITS ALONG THE RAY
        foreach (RaycastHit2D hit in wallHits) 
        
        {   
            // CHECK IF THE HIT COLLIDER IS A WALL
            if (hit.collider != null && hit.collider.CompareTag("LevelWalls")) 
          
            { 
                // CONVERT 2D HIT POINT TO VECTOR3 FOR TELEPORT POSITION
                Vector3 wallPoint = new Vector3(hit.point.x, hit.point.y, 0f); 
               
               // MOVE THE TELEPORT TARGET SLIGHTLY BEFORE THE WALL TO PREVENT GOING THROUGH
                targetPos = wallPoint - direction * 0.1f; 
              

                Debug.Log("Teleport blocked by wall!"); 
                // DEBUG LOG TO SHOW WALL WAS HIT

                break; 
                // STOP CHECKING OTHER HITS SINCE WALL BLOCKS TELEPORT
            }
        }

        transform.position = targetPos; 
        // ACTUALLY MOVE THE PLAYER TO THE TARGET POSITION

        currentPos = targetPos; 
        // UPDATE CURRENT POSITION FOR NEXT LOOP/CHAIN

        Collider2D nextEnemy = FindClosestEnemy(currentPos, chainRadius); 
        // FIND THE NEXT ENEMY TO CHAIN TELEPORT TO

        if (nextEnemy != null) 
        // CHECK IF A NEXT ENEMY EXISTS
        {
            targetPos = nextEnemy.transform.position; 
            // UPDATE THE TELEPORT TARGET TO THE NEXT ENEMY

            yield return new WaitForSeconds(chainDelay); 
            // WAIT BEFORE TELEPORTING TO NEXT ENEMY
        }
        else
        {
            break; 
            // EXIT THE CHAIN LOOP IF NO MORE ENEMIES
        }
    }

    if (playerSprite != null)
        playerSprite.enabled = true; 
    // MAKE PLAYER VISIBLE AGAIN AFTER TELEPORT

    yield return new WaitForSeconds(teleportDuration); 
    // WAIT TO COMPLETE TELEPORT STATE BEFORE ALLOWING OTHER ACTIONS

    isTeleporting = false; 
    // RESET TELEPORTING FLAG TO ALLOW NORMAL PLAYER ACTIONS
}


//PUT THIS VECTOR3 IN TELEPORT SEQEUNCE BECAUSE IT CAN TAKE A VECTOR3 AS A PARAMETER/
Vector3 GetTeleportPosition()
{
    // CONVERT THE MOUSE SCREEN POSITION TO A WORLD POSITION IN THE GAME
    Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition); 
    mouseWorldPos.z = 0f; // SET Z TO 0 BECAUSE THIS GAME IS 2D

    // CALCULATE THE DIRECTION FROM THE PLAYER TO THE MOUSE POSITION AND NORMALIZE IT (MAKE IT HAVE A LENGTH OF 1)
    Vector3 direction = (mouseWorldPos - transform.position).normalized; 

    // CALCULATE THE DISTANCE BETWEEN THE PLAYER AND THE MOUSE POSITION
    float distance = Vector3.Distance(transform.position, mouseWorldPos); 

    // IF THE MOUSE IS WITHIN TELEPORT RANGE
    if (distance <= PlayerRadius)
        return mouseWorldPos; // RETURN THE MOUSE POSITION AS THE TELEPORT TARGET
    else
        // IF THE MOUSE IS TOO FAR, RETURN A POSITION AT MAX TELEPORT RANGE IN THE DIRECTION OF THE MOUSE
        return transform.position + direction * PlayerRadius; 
}



    //THIS FUNCTION FINDS THE CLOSEST ENEMY, THIS FUNCTIONS STOPS WHEN IT DETECTS ALL THE ENEMIES WITHIN A GIVEN RADIUS//


    Collider2D FindClosestEnemy(Vector3 currentPos, float radius)
{
    // GET ALL COLLIDERS WITHIN THE SPECIFIED RADIUS AROUND CURRENT POSITION
    // THIS IS AN ARRAY OF ALL THE COLLIDERS THAT ARE WITHIN THE CIRCLE//
    Collider2D[] enemies = Physics2D.OverlapCircleAll(currentPos, radius);

    // INITIALIZE VARIABLE TO STORE THE CLOSEST ENEMY
    // SET TO NULL BECAUSE THERE HASN'T BEEN ANYTHING FOUND YET
    Collider2D closest = null;

    // INITIALIZE MINIMUM DISTANCE AS INFINITY SO ANY ENEMY WILL BE CLOSER
    float minDist = Mathf.Infinity;

    // LOOP THROUGH ALL COLLIDERS DETECTED IN THE CIRCLE AND FOR EACH ENEMY, CALCULATE THE DISTANCE AND CHECK WHICH ENEMY IS THE CLOSEST//

    foreach (Collider2D enemy in enemies)
    {
        // CHECK IF THE COLLIDER HAS THE TAG "Enemy"
        if (enemy.CompareTag("Enemy"))
        {
            // CALCULATE DISTANCE FROM CURRENT POSITION TO THIS ENEMY
            //THIS LINE GETS THE DISTACNE FROM THE PLAYER'S CURRENT POSITION AND THE ENEMY POSITION TO SEE WHICH ENEMY IS THE CLOSEST//
            float dist = Vector2.Distance(currentPos, enemy.transform.position);

            // CHECK IF THIS ENEMY IS CLOSER THAN ANY PREVIOUSLY CHECKED ENEMY

            //MINDIST -> DOSEN'T STAY INFINITY FOREVER SINCE IT COMPARES ALL ENEMEIES IN THE FOREACH LOOP. FOREACH GOES THROUGH ALL ENEMIES IN THE LOOP//
            if (dist < minDist)
            {
                // UPDATE MINIMUM DISTANCE AND STORE THIS ENEMY AS THE CLOSEST ONE
                //THIS LINE MAKES MINDIST NO LONGER BE INFINITY, WITHOUT THIS LINE MINDIST WILL BE INFINITY FOREVER, BUT YOU CAN COMPARE DISTANCES THIS WAY//
                minDist = dist;

                //SAVE THE COLLIDER OF THE CLOSEST ENEMY//
                closest = enemy;
            }
        }
    }

    // RETURN THE CLOSEST ENEMY FOUND, OR NULL IF NO ENEMIES ARE IN RANGE
    return closest;
}

private void OnDrawGizmos()
{
    if (transform == null) return;

    // Player teleport radius
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(transform.position, PlayerRadius);

    // Chain teleport radius
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, chainRadius);
}

}
