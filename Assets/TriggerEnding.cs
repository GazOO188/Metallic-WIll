using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TriggerEnding : MonoBehaviour
{
    public PlayableDirector EndingTimeline;

    public EnemySpawns ES;
    public PlayerController PC;
    public PlayerTele PT;

    public Rigidbody2D PlayerRB;

    public float AscendForce = 10f;

    private bool triggered = false;

    public GameObject WhiteEffect;


    public Rigidbody2D Rb;

    void Start()
    {
        WhiteEffect.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !triggered && ES.FinalEnemiesDefeated == 60)
        {
            triggered = true;
            StartCoroutine(AscendToHeaven());

            Rb.linearVelocity = Vector2.zero;
            PC.CanMove = false;
        }
    }

    public IEnumerator AscendToHeaven()
    {
        yield return new WaitForSeconds(2f);

        WhiteEffect.SetActive(true);

        // Subscribe HERE (only when timeline is about to play)
        EndingTimeline.stopped += OnTimelineFinished;

        // Now start the timeline
        EndingTimeline.Play();

        // Disable movement and gravity
        PC.CanMove = false;
        PT.isTeleporting = false;
        PlayerRB.gravityScale = 0f;

        // Ascend while the timeline is playing
        while (EndingTimeline.state == PlayState.Playing)
        {
            PlayerRB.linearVelocity = new Vector2(0, AscendForce);
            yield return null;
        }
    }

    // Called only when the timeline finishes
    private void OnTimelineFinished(PlayableDirector director)
    {

        SceneManager.LoadScene("Ending Scene");  // your next scene
    }
}
