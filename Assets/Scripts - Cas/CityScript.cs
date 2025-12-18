using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CityScript : MonoBehaviour
{
    public TextMeshProUGUI CityDialogue;


    public bool CanSummon = false;

    public Collider2D DialgoueCollider;

    public GameObject DialoguePanel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CityDialogue.enabled = false;

        DialoguePanel.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {


        if (CanSummon)
        {
            
            StartCoroutine(ShowDialogue()); 

            CanSummon = false;
       
       
        }

        
        
    }


    public IEnumerator ShowDialogue()
    {
        yield return new WaitForSeconds(0f);
        
        CityDialogue.enabled = true;

        CityDialogue.text = "All units, prepare to engage!";


              
        DialoguePanel.SetActive(true);


        yield return new WaitForSeconds(2f);
        
        CityDialogue.enabled = false;

        DialoguePanel.SetActive(false);

       











    }


      public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
          
           
           CanSummon = true;
        
        }
    }
}



