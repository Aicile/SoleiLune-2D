using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CatPurr : MonoBehaviour
{
    public AudioClip purrSound; 
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
           
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

           
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                audioSource.PlayOneShot(purrSound);
            }
        }
    }
}
