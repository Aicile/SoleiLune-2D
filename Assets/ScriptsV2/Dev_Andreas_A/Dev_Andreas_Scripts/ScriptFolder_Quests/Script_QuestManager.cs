using System.Collections.Generic;
using UnityEngine;

// Manages all quest progress during gameplay
public class Script_QuestManager : MonoBehaviour
{
    // Singleton instance for easy global access
    public static Script_QuestManager Instance;

    // List of all currently active quests
    public List<QuestData> activeQuests = new List<QuestData>();

    // List of all completed quests
    public List<QuestData> completedQuests = new List<QuestData>();

    // Quests defined as ScriptableObjects, loaded at runtime
    [Header("Starting Quests (ScriptableObjects)")]
    public List<QuestAsset> startingQuests;

    [Header("Quest Database Reference")]
    public QuestDatabaseAsset questDatabase;


    private void Awake()
    {
        // Set up Singleton instance
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Optional: keep this object across scene changes
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Load all ScriptableObject-defined quests into the manager
        LoadAllQuestsFromDatabase(); // Or LoadStartingQuests(), based on your preference, Needs testing to figure out which is better, but im just making it now, ill change later based on how it works in practice, a build might deciede what to do next when we get that far.
    }

    // Loads quests from ScriptableObjects and starts them
    public void LoadStartingQuests()
    {
        foreach (var questAsset in startingQuests)
        {
            // Clone the quest data so we don't modify the ScriptableObject directly
            QuestData copy = CloneQuestData(questAsset.questData);
            StartQuest(copy);
        }
    }

    // Creates a deep copy of the quest data and its objectives
    private QuestData CloneQuestData(QuestData original)
    {
        QuestData copy = new QuestData
        {
            questId = original.questId,
            title = original.title,
            description = original.description,
            state = QuestState.Inactive,
            isMajorQuest = original.isMajorQuest,
            rewardDescription = original.rewardDescription,
            prerequisiteQuestIds = new List<string>(original.prerequisiteQuestIds),
            objectives = new List<QuestObjective>()
        };

        // Clone each objective individually
        foreach (var obj in original.objectives)
        {
            QuestObjective clone = new QuestObjective
            {
                id = obj.id,
                description = obj.description,
                type = obj.type,
                state = ObjectiveState.Incomplete,
                targetId = obj.targetId,
                requiredAmount = obj.requiredAmount,
                currentAmount = 0
            };
            copy.objectives.Add(clone);
        }

        return copy;
    }

    // Starts a new quest (if not already active or completed)
    public void StartQuest(QuestData newQuest)
    {
        if (newQuest.state != QuestState.Inactive) return;

        newQuest.state = QuestState.Active;
        activeQuests.Add(newQuest);
        Debug.Log($"Started Quest: {newQuest.title}");
    }

    // Call this when the player does something relevant to quest objectives
    public void NotifyEvent(string objectiveTypeString, string targetId, int amount = 1)
    {
        ObjectiveType type;

        // Try to convert the string into the ObjectiveType enum
        if (!System.Enum.TryParse(objectiveTypeString, out type)) return;

        // Check all active quests for matching objectives
        foreach (var quest in activeQuests)
        {
            foreach (var obj in quest.objectives)
            {
                if (obj.state == ObjectiveState.Complete) continue;

                // If the event matches this objective
                if (obj.type == type && obj.targetId == targetId)
                {
                    obj.currentAmount += amount;

                    // Cap at required amount and mark complete if done
                    if (obj.currentAmount >= obj.requiredAmount)
                    {
                        obj.currentAmount = obj.requiredAmount;
                        obj.state = ObjectiveState.Complete;
                        Debug.Log($"Objective Complete: {obj.description}");
                    }
                }
            }

            // Check if the quest should now be marked complete
            if (IsQuestComplete(quest))
            {
                CompleteQuest(quest);
            }
        }
    }

    // Checks if all objectives in a quest are complete
    private bool IsQuestComplete(QuestData quest)
    {
        foreach (var obj in quest.objectives)
        {
            if (obj.state != ObjectiveState.Complete)
                return false;
        }
        return true;
    }

    // Marks a quest as completed and moves it from active to completed list
    private void CompleteQuest(QuestData quest)
    {
        quest.state = QuestState.Completed;
        activeQuests.Remove(quest);
        completedQuests.Add(quest);
        Debug.Log($"Quest Completed: {quest.title} — {quest.rewardDescription}");
    }

    // Public method to check if a quest with a given ID is completed
    public bool IsQuestCompleted(string questId)
    {
        return completedQuests.Exists(q => q.questId == questId);
    }

    // Gets an active quest by ID (or null if not found)
    public QuestData GetActiveQuest(string questId)
    {
        return activeQuests.Find(q => q.questId == questId);
    }


    // Loads all quests from the QuestDatabase ScriptableObject
    public void LoadAllQuestsFromDatabase()
    {
        if (questDatabase == null)
        {
            Debug.LogWarning("Quest Database not assigned in QuestManager.");
            return;
        }

        foreach (var questAsset in questDatabase.allQuests)
        {
            // Optional: skip DLC quests, faction quests, or locked content here
            QuestData copy = CloneQuestData(questAsset.questData);
            StartQuest(copy);
        }

        Debug.Log($"Loaded {questDatabase.allQuests.Count} quests from QuestDatabase.");
    }





}
