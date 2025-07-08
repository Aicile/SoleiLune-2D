using System.Collections.Generic;
using UnityEngine;

// A ScriptableObject that holds references to all QuestAssets in the game
[CreateAssetMenu(fileName = "New Quest Database", menuName = "Quests/Quest Database")]
public class QuestDatabaseAsset : ScriptableObject
{
    // A list of all quests (defined as separate ScriptableObjects)
    public List<QuestAsset> allQuests = new List<QuestAsset>();
}
