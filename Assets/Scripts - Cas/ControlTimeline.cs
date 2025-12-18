using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class ControlTimeline : MonoBehaviour
{
    
    public GameObject FakeSprite;


    public PlayableDirector PD;


    public PlayerController PC;


    public PlayerTele PT;


    public GameObject TimelineCam;


    public bool CanStart = false;

    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PD.extrapolationMode = DirectorWrapMode.Hold;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanStart)
        {
            
        
        StartCoroutine(DestroyFakePlayer());

        CanStart = true;
       
       
        }
    }


    public IEnumerator DestroyFakePlayer()
    {
        
        yield return new WaitForSeconds(4.5f);


        Destroy(FakeSprite);

        TimelineCam.SetActive(false);


        yield return new WaitForSeconds(1f);


        PC.CanMove = true;

        PT.canTeleport = true;


    
    
    
    }


    public void DisableTimeline()
    {
        
        PD.enabled = false;
        
   
   
   
    }



}
