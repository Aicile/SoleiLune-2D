using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public Dialogue dialogue;

    private bool isPlayerInRange = false;
    private bool dialogueStarted = false; 
    private DialogueManager dialogueManager; 

    void Start()
    {
        
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager not found in the scene.");
        }
    }

    void Update()
    {
        
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !dialogueStarted)
        {
            TriggerDialogue();
            dialogueStarted = true; 
        }

       
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialogueStarted = false; 
        }
    }

    public void TriggerDialogue()
    {
        if (dialogue == null)
        {
            Debug.LogError("Dialogue reference not set in NPCInteraction script.");
            return;
        }

        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager reference is null. Ensure DialogueManager is correctly set up in the scene.");
            return;
        }

        dialogueManager.StartDialogue(dialogue);
    }
}
