using UnityEngine;

// ScriptableObject that holds a single quest's data
[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/Quest Asset")]
public class QuestAsset : ScriptableObject
{
    [TextArea] public string notes; // Optional notes for designers

    public QuestData questData; // The actual quest (title, objectives, etc.)
}
