using System.Collections;
using TMPro;
using UnityEngine;


public class EnemySpawns : MonoBehaviour
{
    
    //LOCATION OF WHERE TO SPAWN THE ENEMIES RIGHT & LEFT CORNERS OF THE SCREEN//

    public GameObject enemyPrefab;
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;
    public Transform UpMostPoint;
    public float waveDelay = 1.8f;

    public int enemiesPerSide = 5;   // Number of enemies per side
   

    public FinalEnemyAi FEI;
    

    public int FinalEnemiesDefeated = 0;

    public int totalWaves = 3;


    private bool particlesPlayed = false, dialogueOne = false, dialogueTwo = false;


    public ParticleSystem Ps;

   
   
    public TextMeshProUGUI EnemyDialogue;


    public GameObject DialguePanel;


    void Start()
    {
    
       
    }


    void Update()
    {
        
        if(FinalEnemiesDefeated >= 60 && !particlesPlayed)
        {

            Debug.Log("Killed all");

            Ps.Play();

            particlesPlayed = true;



        }


    // Dialogue at 30 kills
    if(FinalEnemiesDefeated == 30 && !dialogueOne)
    {
        StartCoroutine(ShowDialogue("All units request assistance!"));
        dialogueOne = true;
    }

    // Dialogue at 50 kills
    if(FinalEnemiesDefeated == 50 && !dialogueTwo)
    {
        StartCoroutine(ShowDialogue("ALL UNITS I WANT HIM DEAD NOW!"));
        dialogueTwo = true;
    }
       
   
   
    }


   public IEnumerator ShowDialogue(string text)
{
    DialguePanel.SetActive(true);
    EnemyDialogue.enabled = true;

    EnemyDialogue.text = text;

    yield return new WaitForSeconds(1.5f);

    DialguePanel.SetActive(false);
    EnemyDialogue.enabled = false;
}







public IEnumerator SpawnWaves()
{
   
    for (int wave = 0; wave < totalWaves; wave++)
    {
        Debug.Log("Starting wave: " + (wave + 1));

        // --- LEFT SIDE ---
        for (int i = 0; i < enemiesPerSide; i++)
        {
            Vector3 spawnPos = leftSpawnPoint.position + new Vector3(Random.Range(-14, 14), 0, 0);
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(0.7f);
        }

        yield return new WaitForSeconds(1f);


        // --- RIGHT SIDE ---
        for (int i = 0; i < enemiesPerSide; i++)
        {
            Vector3 spawnPos = rightSpawnPoint.position + new Vector3(Random.Range(0,17), 0, 0);
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(0.7f);
        }


        yield return new WaitForSeconds(1f);


        // -- UP SIDE  ---
        for (int i = 0; i < enemiesPerSide; i++)
        {
            Vector3 spawnPos = UpMostPoint.position + new Vector3(Random.Range(-20,20),Random.Range(0, 20) , 0);
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            
            
            yield return new WaitForSeconds(0.7f);

        }



        yield return new WaitForSeconds(1f);


        // -- UP SIDE  ---
        for (int i = 0; i < enemiesPerSide; i++)
        {
            Vector3 spawnPos = UpMostPoint.position + new Vector3(Random.Range(-20,20), Random.Range(0,30), 0);
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            
            
            yield return new WaitForSeconds(0.7f);

        }



        

        



        yield return new WaitForSeconds(0.6f);

        Debug.Log("Finished wave: " + (wave + 1));
    }

   
}



    public void CountKills()
    {
        
        FinalEnemiesDefeated++;
   
   
    }


}