using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel; 
    private bool tutorialAlreadyShown = false; 

    void Start()
    {
        tutorialPanel.SetActive(false); 
    }

    void Update()
    {
        
        if (tutorialPanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            CloseTutorial();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !tutorialAlreadyShown)
        {
            ShowTutorial();
        }
    }

    void ShowTutorial()
    {
        tutorialPanel.SetActive(true);
        Time.timeScale = 0f; 
        tutorialAlreadyShown = true; 
    }

    void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        Time.timeScale = 1f; 
    }
}
