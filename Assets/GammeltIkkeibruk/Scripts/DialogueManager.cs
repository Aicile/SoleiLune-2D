using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;

    private Dialogue currentDialogue;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private bool acceptInput = false;
    public float typewriterSpeed = 0.05f;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E) && acceptInput)
        {
            if (IsTypewriting())
            {
                StopAllCoroutines();
                CompleteTypewriterEffect();
            }
            else
            {
                // Assume you have a way to determine the potion type needed here
                string potionType = GetCurrentPotionType();
                AdvanceDialogue(potionType);
            }
        }
    }

    // Method to determine the potion type dynamically
    private string GetCurrentPotionType()
    {
        // Implementation depends on how potion types are tracked in your game
        return "Health";  // Example placeholder
    }

    public void StartDialogue(Dialogue dialogue, string potionType = "")
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        ShowDialogueLine(potionType);  // Ensure the potion type is passed here.
    }

    public void AdvanceDialogue(string potionType = "")
    {
        currentLineIndex++;
        if (currentLineIndex < currentDialogue.lines.Length)
        {
            ShowDialogueLine(potionType);
        }
        else
        {
            EndDialogue();
        }
    }



    void ShowDialogueLine(string potionType)
    {
        DialogueLine line = currentDialogue.lines[currentLineIndex];
        speakerNameText.text = line.speaker;
        string formattedText = line.text.Replace("{potion}", potionType); // Replace placeholder with potion type
        StartCoroutine(TypeSentence(formattedText));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typewriterSpeed);
        }
        acceptInput = true;
    }

    void CompleteTypewriterEffect()
    {
        DialogueLine line = currentDialogue.lines[currentLineIndex];
        dialogueText.text = line.text;
        acceptInput = true;
    }

    bool IsTypewriting()
    {
        return dialogueText.text != currentDialogue.lines[currentLineIndex].text;
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialogueActive = false;
        acceptInput = false;
    }
}
