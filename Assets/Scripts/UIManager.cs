using UnityEngine;
using TMPro; // Make sure to include this namespace to work with TextMeshPro

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questDescriptionText;

    /* void Awake()
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
    }*/

    // This method updates the quest UI with the current active quest's details
    public void UpdateQuestUI(Quest quest)
    {
        if (quest != null && quest.isActive)
        {
            questTitleText.text = quest.title;
            questDescriptionText.text = quest.description;
        }
        else
        {
            // Clear the text if there is no active quest or quest is completed
            questTitleText.text = "No active quest";
            questDescriptionText.text = "Explore and find new quests!";
        }
    }
}
