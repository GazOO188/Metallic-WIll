using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class FinalArea : MonoBehaviour
{
    public PlayerController PC;

    public EnemySpawns ES;

    public PlayableDirector PD;

    public Transform PlayerPos;

    public Rigidbody2D PlayerRb;


    public bool TeleportToFinale = false, EnemiesCanSpawn;
   
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        if (TeleportToFinale)
        {
            
            StartCoroutine(TeleportToFinalArea());

            StartCoroutine(DelaySpawn());

            PD.Play();

            TeleportToFinale = false;
       
       
       
        }
        
    }


    //IF THE DOOR DETECTS THE PLAYER, DO THE FOLLOWING CODE//
    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            
            TeleportToFinale = true;

            EnemiesCanSpawn = true;
       
       
       
        }
   
   
   
    }


    public IEnumerator TeleportToFinalArea()
    {
        
        PlayerRb.linearVelocity = Vector2.zero;

        PC.CanMove = false;

        yield return new WaitForSeconds(1f);


        PlayerPos.transform.position = new Vector2(108.22f, -143.67f);

        PC.CanMove = true;
    
    
    
    }


    public IEnumerator DelaySpawn()
    {
        
        yield return new WaitForSeconds(6.5f);

        StartCoroutine(ES.SpawnWaves());

   
   
    }






}
