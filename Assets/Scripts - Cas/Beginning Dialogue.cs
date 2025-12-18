using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class BeginningDialogue : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textUI;     // The text for dialogue
    public GameObject Panel;           // The dialogue panel
    public GameObject AButton;         // Button to advance dialogue
    public float typeSpeed = 0.05f;    // Type speed

    [Header("Dialogue")]
    public string[] sentences;         // Sentences for the dialogue
    private int index = 0;             // Current sentence index
    private bool textFinished = false; // Is the sentence fully typed

    [Header("Cutscene")]
    public PlayableDirector Timeline1, Timeline2; // Timeline to play after dialogue
   

    private void Start()
    {
        // Hide UI//
        AButton.SetActive(false);
        Timeline2.enabled = false;

    
   
    }

    public IEnumerator StartDialogueSequence()
    {
        // Optional delay before showing dialogue
        yield return new WaitForSeconds(6.6f);

        Panel.SetActive(true);
        textUI.enabled = true;

        StartCoroutine(TypeSentence());
    }

    private void Update()
    {
        // Advance dialogue when A is pressed and sentence is fully typed
        if (textFinished && Input.GetKeyDown(KeyCode.A))
        {
            NextSentence();
        }
    }

    private IEnumerator TypeSentence()
    {
        textFinished = false;
        AButton.SetActive(false);

        textUI.text = "";

        foreach (char letter in sentences[index])
        {
            textUI.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }

        textFinished = true;
        AButton.SetActive(true);
    }

    private void NextSentence()
    {
        AButton.SetActive(false);

        if (index < sentences.Length - 1)
        {
            index++;
            StartCoroutine(TypeSentence());
        }
        else
        {
            // Dialogue finished, hide panel and play timeline
            Panel.SetActive(false);
            textUI.enabled = false;

            Timeline2.enabled = true;
            Timeline2.Play();
        }
    }

   
}
