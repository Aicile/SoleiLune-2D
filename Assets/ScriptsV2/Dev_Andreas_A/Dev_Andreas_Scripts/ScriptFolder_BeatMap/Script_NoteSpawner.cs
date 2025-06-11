using UnityEngine;
using System.Collections.Generic;

public class Script_NoteSpawner : MonoBehaviour
{
    public enum SpawnMode { Controlled, Random }
    public SpawnMode spawnMode = SpawnMode.Controlled; // Choose spawn logic

    public GameObject notePrefab;       // Note prefab with movement & hold script
    public Transform[] lanes;           // Spawn positions for each lane
    public string beatmapFileName = "BeatMaps/beatmap"; // File path inside Resources
    public float noteSpeed = 2f;        // Speed of falling notes

    [Header("Controlled Mode")]
    public float startDelay = 1f;       // Wait time before first note

    [Header("Random Mode")]
    public float randomMinDelay = 1f;
    public float randomMaxDelay = 3f;

    private List<Script_BeatMapNote> noteQueue = new List<Script_BeatMapNote>();
    private int currentNoteIndex = 0;
    private float spawnTimer = 0f;
    private bool hasStarted = false;

    void Start()
    {
        LoadBeatmap();
        spawnTimer = startDelay;
        hasStarted = true;
    }

    void Update()
    {
        if (!hasStarted || currentNoteIndex >= noteQueue.Count) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnNote(noteQueue[currentNoteIndex]);
            currentNoteIndex++;

            // Reset timer based on mode
            if (spawnMode == SpawnMode.Controlled && currentNoteIndex < noteQueue.Count)
                spawnTimer = noteQueue[currentNoteIndex].spawnDelay;
            else if (spawnMode == SpawnMode.Random)
                spawnTimer = Random.Range(randomMinDelay, randomMaxDelay);
        }
    }

    void LoadBeatmap()
    {
        TextAsset text = Resources.Load<TextAsset>("BeatMaps/beatmap");


        if (text == null)
        {
            Debug.LogError("Beatmap not found! Expected Resources/" + beatmapFileName + ".json");
            return;
        }

        Script_BeatMap beatmap = JsonUtility.FromJson<Script_BeatMap>(text.text);
        if (beatmap != null && beatmap.notes != null)
        {
            foreach (var note in beatmap.notes)
            {
                try { note.inputKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), note.inputKeyRaw, true); }
                catch { Debug.LogWarning("Invalid key name: " + note.inputKeyRaw); note.inputKey = KeyCode.None; }

                noteQueue.Add(note);
            }
        }
    }

    void SpawnNote(Script_BeatMapNote data)
    {
        GameObject note = Instantiate(notePrefab, lanes[data.lane].position, Quaternion.identity);

        // Assign data to movement
        Script_NoteMovement move = note.GetComponent<Script_NoteMovement>();
        if (move != null) move.Setup(data, noteSpeed);

        // Assign hold data
        Script_NoteHold hold = note.GetComponent<Script_NoteHold>();
        if (hold != null && data.holdDuration > 0f) hold.Setup(data, noteSpeed);
    }
}
