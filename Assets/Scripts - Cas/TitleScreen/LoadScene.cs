using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class LoadScene : MonoBehaviour
{


    //REFERENCE TO THE BEGINNING CUTSCENE//
    public PlayableDirector Timeline;


    public BeginningDialogue BD;


    //REFERENCE TO ROBOT//

    public GameObject Robot, FadeEffect, ControlPanel, QuitButton, ControlButton, DialoguePanel;



    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        FadeEffect.SetActive(false);
       
        ControlPanel.SetActive(false);

        DialoguePanel.SetActive(false);
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void StartCutScene()
    {
        
        Timeline.Play();

        FadeEffect.SetActive(true);

        Robot.SetActive(false);

        QuitButton.SetActive(false);

        ControlButton.SetActive(false);


        StartCoroutine(WaitForDialogue());

        StartCoroutine(BD.StartDialogueSequence());

        
    
    
    }


    public void ShowControlPanel()
    {
        
        ControlPanel.SetActive(true);

        Robot.SetActive(false);
    
    
    
    }


    public void QuitGame()
    {   
   
    Application.Quit();
   
    }



    public void HideControlPanel()
    {
        
        ControlPanel.SetActive(false);
   

        Robot.SetActive(true);
    
   
   
    }



    public IEnumerator WaitForDialogue()
    {
        
        yield return new WaitForSeconds(7.5f);


        DialoguePanel.SetActive(true);
  
  
    }

    



    public void LoadBeginningArea()
    {
    
    
        SceneManager.LoadScene("Beginning Scene");
    
    
    
    }




}
