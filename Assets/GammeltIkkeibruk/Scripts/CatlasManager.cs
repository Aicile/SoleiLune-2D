using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CatlasManager : MonoBehaviour
{
    public static CatlasManager instance; // Singleton instance

    public TextMeshProUGUI soleilStatusText;
    public TextMeshProUGUI luneStatusText;
    public Image townImage;
    public Image woodWoodsImage;
    public Button sendSoleilButton;
    public Button sendLuneButton;
    public TextMeshProUGUI timeOfDayText; // Text to display the current time of day

    private bool soleilOnMission = false;
    private bool luneOnMission = false;
    private float soleilMissionTime;
    private float luneMissionTime;
    private float soleilMissionDuration = 300f; // 5 minutes for example
    private float luneMissionDuration = 300f;   // 5 minutes for example

    private string selectedArea; // The selected area
    private string selectedCat;  // The selected cat (Soleil or Lune)

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Ensure a single instance
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
    }

    private void Start()
    {
        if (townImage == null)
        {
            Debug.LogError("Town Image is not assigned.");
        }
        else
        {
            townImage.GetComponent<Button>().onClick.AddListener(() => SelectArea("Town"));
        }

        if (woodWoodsImage == null)
        {
            Debug.LogError("WoodWoods Image is not assigned.");
        }
        else
        {
            woodWoodsImage.GetComponent<Button>().onClick.AddListener(() => SelectArea("WoodWoods"));
        }

        if (sendSoleilButton == null)
        {
            Debug.LogError("Send Soleil Button is not assigned.");
        }
        else
        {
            sendSoleilButton.onClick.AddListener(() => SendCatOnMission("Soleil"));
        }

        if (sendLuneButton == null)
        {
            Debug.LogError("Send Lune Button is not assigned.");
        }
        else
        {
            sendLuneButton.onClick.AddListener(() => SendCatOnMission("Lune"));
        }

        if (timeOfDayText == null)
        {
            Debug.LogError("Time of Day Text is not assigned.");
        }

        if (soleilStatusText == null)
        {
            Debug.LogError("Soleil Status Text is not assigned.");
        }

        if (luneStatusText == null)
        {
            Debug.LogError("Lune Status Text is not assigned.");
        }

        UpdateTimeOfDay(); // Initialize the time of day display
    }

    private void Update()
    {
        UpdateMissionStatus();
    }

    public void SelectArea(string area)
    {
        selectedArea = area;
        Debug.Log($"Selected Area: {selectedArea}");
        // Update UI to show selected area ingredients
    }

    public void SendCatOnMission(string catName)
    {
        if (catName == "Soleil" && !soleilOnMission)
        {
            soleilOnMission = true;
            soleilMissionTime = Time.time + soleilMissionDuration;
            soleilStatusText.text = $"Soleil is on a mission to {selectedArea}!";
        }
        else if (catName == "Lune" && !luneOnMission)
        {
            luneOnMission = true;
            luneMissionTime = Time.time + luneMissionDuration;
            luneStatusText.text = $"Lune is on a mission to {selectedArea}!";
        }
    }

    private void UpdateMissionStatus()
    {
        if (soleilOnMission && Time.time >= soleilMissionTime)
        {
            soleilOnMission = false;
            ReceiveMissionRewards("Soleil");
            soleilStatusText.text = "Soleil has returned!";
        }

        if (luneOnMission && Time.time >= luneMissionTime)
        {
            luneOnMission = false;
            ReceiveMissionRewards("Lune");
            luneStatusText.text = "Lune has returned!";
        }
    }

    private void ReceiveMissionRewards(string catName)
    {
        List<string> ingredients = new List<string>();
        string timeOfDay = timeOfDayText.text;

        if (catName == "Soleil")
        {
            // Add ingredients Soleil can gather based on the area and time of day
            if (timeOfDay == "Day")
            {
                ingredients.Add("Light Berry");
                ingredients.Add("Sun Drop");
                if (selectedArea == "Town")
                {
                    ingredients.Add("Coffee Beans");
                }
                else if (selectedArea == "WoodWoods")
                {
                    ingredients.Add("Healing Ging Tree Leaves");
                }
            }
        }
        else if (catName == "Lune")
        {
            // Add ingredients Lune can gather based on the area and time of day
            if (timeOfDay == "Night")
            {
                ingredients.Add("Dark Berry");
                ingredients.Add("Moon Drop");
                if (selectedArea == "Town")
                {
                    ingredients.Add("Lavender");
                }
                else if (selectedArea == "WoodWoods")
                {
                    ingredients.Add("Mint leaves");
                }
            }
        }

        foreach (string ingredient in ingredients)
        {
            JournalManager.instance.UpdateIngredientStock(ingredient, 1);
        }

        Debug.Log($"{catName} has returned with ingredients from {selectedArea} during the {timeOfDay}!");
    }

    private void UpdateTimeOfDay()
    {
        // Implement your logic to update the time of day here
        // For example:
        // timeOfDayText.text = "Day"; // or "Night"
    }
}
