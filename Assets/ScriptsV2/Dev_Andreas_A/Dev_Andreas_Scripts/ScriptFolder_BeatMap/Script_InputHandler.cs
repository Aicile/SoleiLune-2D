using UnityEngine;
using System.Collections.Generic;

// Handles player input for tapping and holding notes
public class Script_InputHandler : MonoBehaviour
{
    public float hitRange = 0.5f; // Vertical distance threshold to register a hit
    public string noteTag = "Note"; // Tag for all notes in the scene
    public Transform hitZone; // Assign this in Inspector

    void Update()
    {
        // Handle tap input
        if (Input.GetKeyDown(KeyCode.A)) TryTap(KeyCode.A);
        if (Input.GetKeyDown(KeyCode.S)) TryTap(KeyCode.S);
        if (Input.GetKeyDown(KeyCode.D)) TryTap(KeyCode.D);

        // Handle hold input while key is held down
        if (Input.GetKey(KeyCode.A)) UpdateHold(KeyCode.A);
        if (Input.GetKey(KeyCode.S)) UpdateHold(KeyCode.S);
        if (Input.GetKey(KeyCode.D)) UpdateHold(KeyCode.D);

        // Handle release of hold keys
        if (Input.GetKeyUp(KeyCode.A)) CancelHold(KeyCode.A);
        if (Input.GetKeyUp(KeyCode.S)) CancelHold(KeyCode.S);
        if (Input.GetKeyUp(KeyCode.D)) CancelHold(KeyCode.D);
    }

    // Attempts to tap a note when key is pressed
    void TryTap(KeyCode key)
    {
        Script_NoteMovement[] notes = Object.FindObjectsByType<Script_NoteMovement>(FindObjectsSortMode.None);


        foreach (var note in notes)
        {
            if (note.Data.inputKey != key) continue;

            float distance = Mathf.Abs(note.transform.position.y - hitZone.position.y);
            if (distance <= hitRange && note.Data.holdDuration <= 0)
            {
                Debug.Log($"Tap Hit! Key {key}");
                Destroy(note.gameObject); //Remove the note tapped note
                return;
            }
        }

        Debug.Log("Tap Miss!"); // No matching note found within range
    }

    // Updates hold notes while key is held down
    void UpdateHold(KeyCode key)
    {
        Script_NoteHold[] holdNotes = Object.FindObjectsByType<Script_NoteHold>(FindObjectsSortMode.None);

        foreach (var note in holdNotes)
        {
            if (note == null || note.Data == null || note.Data.inputKey != key) continue;


            float distance = Mathf.Abs(note.transform.position.y);
            if (distance <= hitRange)
            {
                note.StartHold(); // Start accumulating hold time
            }
        }
    }

    // Cancels hold if key is released too early
    void CancelHold(KeyCode key)
    {
        // Get all active hold notes in the scene
        Script_NoteHold[] holdNotes = Object.FindObjectsByType<Script_NoteHold>(FindObjectsSortMode.None);

        foreach (var note in holdNotes)
        {
            // Skip if note or its data is not set (null safety)
            if (note == null || note.Data == null) continue;

            // Only act on the note that matches the released key
            if (note.Data.inputKey != key) continue;

            // Cancel the hold (may destroy the note)
            note.CancelHold();
        }
    }
}
