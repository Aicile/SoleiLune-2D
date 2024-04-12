using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI speakerNameText; 
    public TextMeshProUGUI dialogueText; 
    public GameObject dialoguePanel; 

    private Dialogue currentDialogue;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private bool acceptInput = false; 

    
    public float typewriterSpeed = 0.05f;

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
                
                AdvanceDialogue();
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        ShowDialogueLine();
    }

    void AdvanceDialogue()
    {
        currentLineIndex++;
        if (currentLineIndex < currentDialogue.lines.Length)
        {
            ShowDialogueLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void ShowDialogueLine()
    {
        DialogueLine line = currentDialogue.lines[currentLineIndex];
        speakerNameText.text = line.speaker; 
        StartCoroutine(TypeSentence(line.text)); 
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

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialogueActive = false;
        acceptInput = false; 
    }
}
