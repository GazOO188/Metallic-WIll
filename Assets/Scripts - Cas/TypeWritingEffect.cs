using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TypeWritingEffect : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public float typeSpeed = 0.05f;

    public string[] sentences;
    private int index = 0;
    private bool textFinished = false;

    public GameObject Panel;
    public GameObject AButton;

    public PlayableDirector PD;

    private void Start()
    {
        AButton.SetActive(false);
        textUI.enabled = false;
        PD.enabled = false;

        // Listen for timeline finishing
        PD.stopped += OnTimelineFinished;

        StartCoroutine(StartDialogueSequence());
    }

    public IEnumerator StartDialogueSequence()
    {
        Panel.SetActive(false);

        yield return new WaitForSeconds(3.2f);

        Panel.SetActive(true);
        textUI.enabled = true;

        StartCoroutine(TypeSentence());
    }

    private void Update()
    {
        if (textFinished && Input.GetKeyDown(KeyCode.A))
        {
            NextSentence();
        }
    }

    public IEnumerator TypeSentence()
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

    void NextSentence()
    {
        AButton.SetActive(false);

        if (index < sentences.Length - 1)
        {
            index++;
            StartCoroutine(TypeSentence());
        }
        else
        {
            Debug.Log("Dialogue Ended");

            Panel.SetActive(false);
            AButton.SetActive(false);
            textUI.enabled = false;

            // Play timeline
            PD.enabled = true;
            PD.Play();
        }
    }

    // GO BACK TO MAIN MENU//
    private void OnTimelineFinished(PlayableDirector director)
    {
       

        // Replace "MainMenu" with your scene name
        SceneManager.LoadScene("Ttile Screen");
    }
}
