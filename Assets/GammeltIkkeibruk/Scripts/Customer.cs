using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour
{
    public Dialogue requestDialogue;
    public Dialogue thankYouDialogue;
    public Dialogue outOfStockDialogue;
    public string potionNeeded;
    public Transform targetPosition; // This will be the chair position
    public GameObject thoughtBubble; // Attach this in the prefab directly

    public GameObject healthPotionObject;  // Assign in the editor
    public GameObject manaPotionObject;    // Assign in the editor
    public GameObject energyPotionObject;  // Assign in the editor

    private bool hasInteracted = false;
    private bool playerInRange = false;
    private Animator animator; // Reference to the Animator component

    void Awake()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
        AssignRandomPotionNeed();
        thoughtBubble.SetActive(false); // Ensure it's hidden on start
        UpdatePotionDisplay();
    }

    void Update()
    {
        MoveToTarget();
        CheckForInteraction();
    }

    private void MoveToTarget()
    {
        if (targetPosition != null && !hasInteracted)
        {
            animator.SetBool("isWalking", true); // Set walking animation
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, Time.deltaTime * 1);
            if (Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
            {
                animator.SetBool("isWalking", false); // Set idle animation
                thoughtBubble.SetActive(true);  // Show thought bubble when at the chair
                UpdatePotionDisplay();  // Update potion display when they sit down
            }
        }
    }

    private void UpdatePotionDisplay()
    {
        // Disable all potions first
        healthPotionObject.SetActive(false);
        manaPotionObject.SetActive(false);
        energyPotionObject.SetActive(false);

        // Enable the correct potion based on the need
        switch (potionNeeded)
        {
            case "Health":
                healthPotionObject.SetActive(true);
                break;
            case "Mana":
                manaPotionObject.SetActive(true);
                break;
            case "Energy":
                energyPotionObject.SetActive(true);
                break;
        }
    }

    private void CheckForInteraction()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !hasInteracted)
        {
            hasInteracted = true;
            animator.SetTrigger("Interact"); // Trigger interaction animation
            InitiateInteraction();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            DialogueManager.instance.EndDialogue();
            hasInteracted = false;
            thoughtBubble.SetActive(false);
        }
    }

    void InitiateInteraction()
    {
        requestDialogue.lines[0].text = $"I want a {potionNeeded} potion.";
        DialogueManager.instance.StartDialogue(requestDialogue, potionNeeded);
        StartCoroutine(HandlePotionRequestAfterDialogue());
    }

    IEnumerator HandlePotionRequestAfterDialogue()
    {
        yield return new WaitForSeconds(2);
        if (InventoryManager.instance.CheckPotionInInventory(potionNeeded))
        {
            CompleteTransaction(3); // +3 points for having the potion in inventory
        }
        else if (StockManager.instance.CheckPotionStock(potionNeeded))
        {
            CompleteTransaction(1); // +1 point for having the potion in stock
        }
        else
        {
            outOfStockDialogue.lines[0].text = $"Seems like you are out of {potionNeeded} potion.";
            DialogueManager.instance.StartDialogue(outOfStockDialogue, potionNeeded);
        }
    }

    public void CompleteTransaction(int points)
    {
        SatisfactionManager.instance.AddSatisfactionPoints(points);
        StockManager.instance.UpdatePotionStock(potionNeeded, -1);
        thankYouDialogue.lines[0].text = $"Thank you for the {potionNeeded} potion.";
        DialogueManager.instance.StartDialogue(thankYouDialogue, potionNeeded);
        StartCoroutine(LeaveCafe());
    }

    IEnumerator LeaveCafe()
    {
        yield return new WaitForSeconds(2);
        FindObjectOfType<CustomerManager>().CustomerServed(this);
        gameObject.SetActive(false);
    }

    private void AssignRandomPotionNeed()
    {
        string[] potions = { "Health", "Mana", "Energy" };
        potionNeeded = potions[Random.Range(0, potions.Length)];
    }
}
