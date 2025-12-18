using UnityEngine;                  
using UnityEngine.UI;            
using System.Collections;          

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]      // SHOWS A HEADER IN THE INSPECTOR FOR HEALTH VARIABLES
    public int maxHealth = 25;       // MAX POSSIBLE HEALTH
    public int currentHealth;        // PLAYER'S CURRENT HEALTH VALUE

    [Header("UI Settings")]          // HEADER FOR UI SETTINGS IN INSPECTOR
    public Slider healthBar;         // REFERENCE TO HEALTH BAR SLIDER UI

    [Header("Invincibility Settings")] // HEADER FOR INVINCIBILITY VARIABLES
    public float invincibilityDuration = 2f; // HOW LONG PLAYER IS INVINCIBLE AFTER DAMAGE
    public float blinkInterval = 0.2f;       // HOW FAST PLAYER SPRITE BLINKS
    public bool isInvincible = false;       // TRACKS IF PLAYER CAN TAKE DAMAGE OR NOT
    public bool canDamage = false;

    private SpriteRenderer spriteRenderer;   // USED TO TURN PLAYER SPRITE ON/OFF FOR BLINKING


    


    void Start()
    {
        currentHealth = maxHealth;   // SET CURRENT HEALTH TO FULL HEALTH AT START, THE CURRENT HEALTH IS 10//

        if (healthBar != null)       // IF THERE IS A HEALTH BAR ASSIGNED…
        {
            healthBar.maxValue = maxHealth;  // SET SLIDER MAX VALUE TO MAX HEALTH
            healthBar.value = currentHealth; // SET SLIDER STARTING POSITION TO FULL HEALTH. THIS SETS THE BAR TO FULL HEALTH
        }

        spriteRenderer = GetComponent<SpriteRenderer>(); // GET SPRITE RENDERER ATTACHED TO PLAYER
        
        if (spriteRenderer == null)   // IF NO SPRITE RENDERER IS FOUND…
        {
            Debug.LogWarning("No SpriteRenderer found on Player! Add one to see blink effect."); // WARN DEVELOPER
        }
   
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;    // IF PLAYER IS INVINCIBLE, IGNORE DAMAGE

         
        
        currentHealth -= damage;     // SUBTRACT DAMAGE FROM CURRENT HEALTH
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // KEEP HEALTH BETWEEN 0 AND MAX, SO IT DOSEN'T GO BELOW 0//
        UpdateHealthBar();           // UPDATE UI SLIDER TO MATCH NEW HEALTH VALUE

        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}/{maxHealth}"); // PRINT DAMAGE INFO
        
        
        
        if (currentHealth <= 0)      // IF HEALTH REACHED ZERO…
        {
            Die();                   // CALL DIE FUNCTION
        }
        else
        {
            StartCoroutine(InvincibilityFrames()); // START INVINCIBILITY + BLINK EFFECT
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;     // ADD HEALING AMOUNT TO HEALTH
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // CLAMP HEALTH
        UpdateHealthBar();           // UPDATE HEALTH BAR UI
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)       // IF UI HEALTH BAR EXISTS…
        {
            healthBar.value = currentHealth; // SET SLIDER TO CURRENT HEALTH
        }
    }

    private void Die()
    {
        Debug.Log("Player had Died!"); // PRINT DEBUG MESSAGE
        // ADD DEATH LOGIC HERE LATER (RESPAWN, GAME OVER, ETC.)

    }

    //THIS COROUTINE TRIGGERS THE BLINKING EFFECT FOR INVINCIBILITY AND HANDLES INVINCIBILTY FRAMES//
    //BLINKS ON AND OFF//
    private IEnumerator InvincibilityFrames()
    {
        isInvincible = true;         // SET PLAYER TO INVINCIBLE


      


        //THIS VARIABLE '"ELASPED" SERVES AS A TIMER FOR THE INVINCIBILTY AND STARTS AT 0//
        float elapsed = 0f;          // TIME TRACKER FOR INVINCIBILITY


        //THIS LINE MEANS WHILE THE TIMER IS LESS THAN THE INVINCIBILITY DURATION, DO THE LINES BELOW//
        while (elapsed < invincibilityDuration) // LOOP UNTIL INVINCIBILITY TIME ENDS
        {
            if (spriteRenderer != null) // IF SPRITE RENDERER EXISTS…
            {


                //THIS LINE TOGGLES THE SPRITERNDER ON/OFF, GIVES THE BLINKING EFFECT//
                spriteRenderer.enabled = !spriteRenderer.enabled; // TOGGLE SPRITE ON/OFF TO BLINK
            
            }

            yield return new WaitForSeconds(blinkInterval); // WAIT A SHORT TIME BEFORE NEXT BLINK
            
            
            //THIS LINE ADDS THE BLINKINTERVAL (0.2) TO THE TIMER SO THE TIMER GOES UP BY 0.2//
            elapsed += blinkInterval;  // ADD TO ELAPSED TIME
        }

        if (spriteRenderer != null)
            spriteRenderer.enabled = true; // MAKE SURE SPRITE IS VISIBLE AT END

        isInvincible = false;        // TURN OFF INVINCIBILITY
    }
}