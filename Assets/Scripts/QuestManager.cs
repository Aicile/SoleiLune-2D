using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Linq;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public Quest[] quests;
    public bool[] questCompleted;
    public Quest currentQuest; 
    private UIManager uiManager; 
/*
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    } */

    void OnDestroy()
    {
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        uiManager = FindObjectOfType<UIManager>();

        
        if (currentQuest != null && currentQuest.isActive)
        {
            uiManager?.UpdateQuestUI(currentQuest);
        }
    }

    public void StartQuest(string questID)
    {
        Quest questToStart = quests.FirstOrDefault(q => q.questID == questID);
        if (questToStart != null && !questToStart.isActive)
        {
            questToStart.isActive = true;
            currentQuest = questToStart; 
            Debug.Log($"Quest started: {questToStart.title}. Objective: {questToStart.description}");

            
            uiManager?.UpdateQuestUI(questToStart);
        }
        else
        {
            Debug.LogError("Quest with ID " + questID + " not found or already active.");
        }
    }

    public void CompleteQuest(string questID)
    {
        Quest questToComplete = quests.FirstOrDefault(q => q.questID == questID);
        if (questToComplete != null && questToComplete.isActive)
        {
            questToComplete.isActive = false; 
            Debug.Log($"Quest completed: {questToComplete.title}");

            
            uiManager?.UpdateQuestUI(null); 

           
        }
        else
        {
            Debug.LogError("Quest with ID " + questID + " not found or not active.");
        }
    }
}
