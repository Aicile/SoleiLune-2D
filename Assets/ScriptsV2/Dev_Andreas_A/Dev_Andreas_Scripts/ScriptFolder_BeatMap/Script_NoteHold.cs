using UnityEngine;

// Handles logic for hold notes
public class Script_NoteHold : MonoBehaviour
{
    public Script_BeatMapNote Data { get; private set; } // Beatmap data reference
    private float holdTime = 0f; // Tracks how long the key has been held
    private bool isHolding = false; // Whether the player is currently holding the key
    private float speed; // Movement speed

    // Initializes hold note with data and movement speed
    public void Setup(Script_BeatMapNote data, float noteSpeed)
    {
        Data = data;
        speed = noteSpeed;
    }

    void Update()
    {
        if (!isHolding)
        {
            // Move down if not currently being held
            transform.position += Vector3.down * speed * Time.deltaTime;
        }
        else
        {
            // Track hold duration
            holdTime += Time.deltaTime;

            if (holdTime >= Data.holdDuration)
            {
                Debug.Log("Hold complete!");
                Destroy(gameObject); // Successfully completed hold
            }
        }
    }

    // Called when player is holding the key in the hit zone
    public void StartHold()
    {
        isHolding = true;
    }

    // Called when key is released before holdDuration is reached
    public void CancelHold()
    {
        if (holdTime < Data.holdDuration)
        {
            Debug.Log("Hold failed early!");
        }

        Destroy(gameObject); // Remove note either way
    }
}
