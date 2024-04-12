[System.Serializable]
public class QuestGoal
{
    public int requiredAmount;
    public int currentAmount;
    public string description;

    public bool IsReached()
    {
        return (currentAmount >= requiredAmount);
    }
}
