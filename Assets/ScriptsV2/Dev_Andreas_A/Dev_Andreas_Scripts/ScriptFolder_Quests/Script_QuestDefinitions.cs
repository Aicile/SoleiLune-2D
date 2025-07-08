using System;
using System.Collections.Generic;
using UnityEngine;

// Enum for tracking the state of an entire quest
public enum QuestState
{
    Inactive,
    Active,
    Completed,
    Failed
}

// Enum for what kind of objective the player must complete
public enum ObjectiveType
{
    TalkToNPC,
    CollectItem,
    KillTarget,
    GoToLocation,
    Custom
}

// Enum for tracking each individual objective's completion state
public enum ObjectiveState
{
    Incomplete,
    Complete
}

// Represents one objective within a quest
[Serializable]
public class QuestObjective
{
    public string id; // Unique within the quest
    public string description;
    public ObjectiveType type;
    public ObjectiveState state = ObjectiveState.Incomplete;

    public string targetId; // The ID of the target (NPC, Item, Enemy, etc.)
    public int requiredAmount = 1; // How many are needed
    public int currentAmount = 0; // How many are completed so far
}

// Represents an entire quest, which may have multiple objectives
[Serializable]
public class QuestData
{
    public string questId;
    public string title;
    public string description;

    public QuestState state = QuestState.Inactive;
    public bool isMajorQuest = false;

    public List<QuestObjective> objectives = new List<QuestObjective>();

    // Optional: for quest prerequisites
    public List<string> prerequisiteQuestIds = new List<string>();

    // Optional: display-only reward text
    public string rewardDescription;
}

// A container that holds all quests (useful for save/load or editor lists) So my research says, not sure how to use it yet.
[Serializable]
public class QuestDatabase
{
    public List<QuestData> allQuests = new List<QuestData>();
}

public class Script_QuestDefinitions : MonoBehaviour
{
    // This class is intentionally left empty.
    // It serves as a holder for quest-related data structures and enums.
}
