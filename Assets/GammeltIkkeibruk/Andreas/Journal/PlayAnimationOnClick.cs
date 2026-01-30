using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Required for coroutine

public class PlayAnimationOnClick : MonoBehaviour
{
    public Button[] yourButton;
    public Button[] backButton;
    public GameObject animatedObject; // Reference to the GameObject with the Animator
    public float animationDuration = 1.0f; // Set this to the length of your animation
    private Animator animator;
    void Start()
    {
        animator = animatedObject.GetComponent<Animator>();
        for (int i = 0; i < yourButton.Length; i++)
        {
        yourButton[i].onClick.AddListener(() => StartCoroutine(PlayAnimationAndProceed()));

        }
        for (int i = 0; i < backButton.Length; i++)
        {
            backButton[i].onClick.AddListener(() => StartCoroutine(PlayAnimationAndBackwards()));

        }
    }

    IEnumerator PlayAnimationAndProceed()
    {
        if (!animatedObject.activeSelf)
        {
            animatedObject.SetActive(true); // Activate the GameObject
        }

        
        animator.SetTrigger("FlipPage");

        yield return new WaitForSeconds(animationDuration); 
    }
    IEnumerator PlayAnimationAndBackwards()
    {
        if (!animatedObject.activeSelf)
        {
            animatedObject.SetActive(true); // Activate the GameObject
        }


        animator.SetTrigger("FlipBack");

        yield return new WaitForSeconds(animationDuration); 
    }
}