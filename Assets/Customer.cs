using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour
{
    public Dialogue requestDialogue;
    public Dialogue thankYouDialogue;
    public Dialogue outOfStockDialogue;
    public string potionNeeded;
    public Transform targetPosition;

    private bool hasInteracted = false;  // Tracks if the player has already interacted
    private bool playerInRange = false;  // Tracks if the player is within interaction range

    void Awake()
    {
        AssignRandomPotionNeed();
    }

    void Update()
    {
        if (targetPosition != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, Time.deltaTime * 1);
        }

        // Check for player input when in range and not interacted
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !hasInteracted)
        {
            hasInteracted = true;  // Prevent further interaction until reset
            InitiateInteraction();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;  // Player enters the trigger zone
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            DialogueManager.instance.EndDialogue();
            hasInteracted = false;  // Reset interaction flag when player leaves
        }
    }

    void InitiateInteraction()
    {
        requestDialogue.lines[0].text = $"I want a {potionNeeded} potion.";  // Dynamic request based on potion type
        DialogueManager.instance.StartDialogue(requestDialogue, potionNeeded);

        StartCoroutine(HandlePotionRequestAfterDialogue());
    }

    IEnumerator HandlePotionRequestAfterDialogue()
    {
        yield return new WaitForSeconds(2);  // Wait for request dialogue to be read

        if (JournalManager.instance.CheckPotionStock(potionNeeded))
        {
            CompleteTransaction();
        }
        else
        {
            outOfStockDialogue.lines[0].text = $"Seems like you are out of {potionNeeded} potion.";
            DialogueManager.instance.StartDialogue(outOfStockDialogue, potionNeeded);
        }
    }

    public void CompleteTransaction()
    {
        JournalManager.instance.UpdatePotionStock(potionNeeded, -1);
        thankYouDialogue.lines[0].text = $"Thank you for the {potionNeeded} potion.";
        DialogueManager.instance.StartDialogue(thankYouDialogue, potionNeeded);
        StartCoroutine(LeaveCafe());  // Customer leaves after transaction
    }

    IEnumerator LeaveCafe()
    {
        yield return new WaitForSeconds(2);  // Wait for 2 seconds after dialogue
        FindObjectOfType<CustomerManager>().CustomerServed(this);  // Notify the manager that this customer is served
        gameObject.SetActive(false);  // Deactivate or you could add an animation for leaving
    }


    private void AssignRandomPotionNeed()
    {
        string[] potions = { "Health", "Mana", "Energy" };
        potionNeeded = potions[Random.Range(0, potions.Length)];
    }
}
