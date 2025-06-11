using UnityEngine;

// Represents a single note in the beatmap
[System.Serializable]
public class Script_BeatMapNote
{
    public int lane;                 // Lane index (0 = top, 1 = mid, 2 = bottom)
    public string inputKeyRaw;       // Raw key name from JSON (e.g., "A")
    public float holdDuration = 0f;  // Duration the player must hold (0 = tap)
    public float spawnDelay = 0f;    // Time after previous note to spawn (only used in Controlled mode)

    [System.NonSerialized]
    public KeyCode inputKey;         // Parsed key from raw string
}

// Wrapper class to load all notes from JSON
[System.Serializable]
public class Script_BeatMap
{
    public Script_BeatMapNote[] notes;
}
