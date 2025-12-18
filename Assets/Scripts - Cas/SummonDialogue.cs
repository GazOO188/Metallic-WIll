using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummonDialogue : MonoBehaviour
{
    public TextMeshProUGUI LevelOneDialogue;


    public bool CanSummon = false;

    public Collider2D DialgoueCollider;

    public GameObject DialoguePanel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LevelOneDialogue.enabled = false;

        DialoguePanel.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {


        
        
    }


    public IEnumerator ShowDialogue()
    {
        yield return new WaitForSeconds(0f);
        
        LevelOneDialogue.enabled = true;
              
        DialoguePanel.SetActive(true);


        yield return new WaitForSeconds(3f);
        
        LevelOneDialogue.enabled = false;

        DialoguePanel.SetActive(false);

       











    }


      public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !CanSummon)
        {
           StartCoroutine(ShowDialogue());
           
           CanSummon = true;
        
        }
    }
}



