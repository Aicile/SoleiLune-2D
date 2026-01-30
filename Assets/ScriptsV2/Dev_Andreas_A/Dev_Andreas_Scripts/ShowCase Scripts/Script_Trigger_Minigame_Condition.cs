using UnityEngine;

public class Script_Trigger_Minigame_Condition : MonoBehaviour
{
    // UI prefab to spawn (must be a UI object with RectTransform)
    public GameObject uiPrefab;

    // Canvas or UI parent
    public Transform canvasParent;

    // World-space interaction prompt (not a Canvas UI element)
    public GameObject interactionText;

    // Interaction key
    public KeyCode interactionKey = KeyCode.F;

    private bool playerInRange = false;

    void Start()
    {
        if (interactionText != null)
            interactionText.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            Instantiate(uiPrefab, canvasParent);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactionText != null)
                interactionText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactionText != null)
                interactionText.SetActive(false);
        }
    }
}
