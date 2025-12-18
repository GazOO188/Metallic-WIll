using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class DetectPlayer : MonoBehaviour
{
    public PlayerTele PT;
    public PlayerController PC;

    public Transform Player;
    public Rigidbody2D Rb;
    public SpriteRenderer SR;

    public PlayableDirector PD;

    public float GravityInfluence = 0.2f;

    public bool Touchdoor = false;
    public bool CanTeleportDifferently = false;

    void Update()
    {
        if (Touchdoor)
        {
            // Play timeline when door is touched
            if (PD != null)
                PD.Play();

            // Teleport immediately (no delay)
            StartCoroutine(TeleportPlayer());

            PC.CanMove = false;
            Touchdoor = false;
        }



    }


    public IEnumerator TeleportPlayer()
    {
        Rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.9f);
        Vector2 TeleportOutside = new Vector2(23.9f, -28f);

        CanTeleportDifferently = true;

        PC.CanMove = false;

        // SHORT DELAY ONLY IF YOU WANT A SMALL VISUAL PAUSE
        yield return new WaitForSeconds(0.2f);

        Rb.gravityScale = 0;
 

        SR.flipY = true;

        // TELEPORT PLAYER HERE — happens immediately after touching door
        Player.transform.position = TeleportOutside;

        Rb.gravityScale = GravityInfluence;

        yield return new WaitForSeconds(3.3f);

         SR.flipY = false;

         PC.CanMove = true;

         Rb.gravityScale = 5f;

        
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && PT.EnemiesDestoryed >= 6)
        {
            Touchdoor = true;
            Debug.Log("Can Teleport");
        }
    }
}