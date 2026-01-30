using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questID;
    public string title; 
    public string description;
    public bool isActive;

    public QuestGoal goal;

    public void CheckGoals()
    {
        if (goal.IsReached())
        {
            Complete();
        }
    }

    public void Complete()
    {
        isActive = false;
        Debug.Log($"{title} was completed!"); 
    }
}
