using UnityEngine;

public class Customer : MonoBehaviour
{
    public Dialogue requestDialogue;  // Dialogue when requesting potion
    public Dialogue thankYouDialogue; // Dialogue for successful transaction
    public Dialogue outOfStockDialogue; // Dialogue when out of stock
    public string potionNeeded; // The type of potion this customer wants
    public Transform targetPosition;


    void Update()
    {
        if (targetPosition != null)
        {
            // Implement movement logic here, could be simple linear interpolation or more complex pathfinding
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, Time.deltaTime * 1); // Adjust speed as necessary
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ShowRequest();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DialogueManager.instance.EndDialogue();
        }
    }

    void ShowRequest()
    {
        // Check if the potion is in stock
        bool isInStock = JournalManager.instance.CheckPotionStock(potionNeeded);

        if (isInStock)
        {
            // Start dialogue with the request dialogue
            DialogueManager.instance.StartDialogue(requestDialogue);
        }
        else
        {
            // Start dialogue with the out of stock dialogue
            DialogueManager.instance.StartDialogue(outOfStockDialogue);
        }
    }

    public void CompleteTransaction()
    {
        if (JournalManager.instance.CheckPotionStock(potionNeeded))
        {
            // Reduce stock
            JournalManager.instance.UpdatePotionStock(potionNeeded, -1);
            // Thank customer
            DialogueManager.instance.StartDialogue(thankYouDialogue);
        }
        else
        {
            DialogueManager.instance.StartDialogue(outOfStockDialogue);
        }
    }


}
