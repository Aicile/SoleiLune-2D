using UnityEngine;

// Handles movement of individual tap or hold notes
public class Script_NoteMovement : MonoBehaviour
{
    public Script_BeatMapNote Data { get; private set; } // Reference to this note's beatmap data
    private float speed = 2f; // Speed at which the note moves downward

    // Called when the note is spawned to set its movement speed and data
    public void Setup(Script_BeatMapNote data, float noteSpeed)
    {
        this.Data = data;       // Store beatmap data for input checks
        this.speed = noteSpeed; // Store note speed for Update movement
    }

    void Update()
    {
        // Move the note down over time
        transform.position += Vector3.down * speed * Time.deltaTime;
    }
}

/*
using UnityEngine;

// Moves a note from its spawn lane to the hit zone over a set time
public class Script_NoteMovement : MonoBehaviour
{
    private Vector3 startPosition;      // Where the note starts (spawned)
    private Vector3 targetPosition;     // Where it should end (the hit zone)
    private float travelTime;           // How long it should take to reach the hit zone
    private float spawnTime;            // When it was spawned
    private Script_BeatMapNote data;    // The note’s beatmap data

    // Called when note is spawned
    public void Setup(Script_BeatMapNote noteData, float timeToReach)
    {
        data = noteData;
        travelTime = timeToReach;
        spawnTime = Time.time;

        startPosition = transform.position;

        Transform hitZone = GameObject.FindGameObjectWithTag("HitZone")?.transform;
        if (hitZone != null)
        {
            targetPosition = new Vector3(startPosition.x, hitZone.position.y, startPosition.z);
        }
        else
        {
            Debug.LogError("HitZone not found! Make sure it exists and is tagged 'HitZone'");
        }
    }

    void Update()
    {
        float t = (Time.time - spawnTime) / travelTime;

        // Clamp between 0 and 1 to prevent overshooting
        t = Mathf.Clamp01(t);

        // Always move from original spawn position to hit zone
        transform.position = Vector3.Lerp(startPosition, targetPosition, t);
    }

    public Script_BeatMapNote GetData() => data;
}*/
