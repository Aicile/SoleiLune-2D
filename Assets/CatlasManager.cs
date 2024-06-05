using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatlasManager : MonoBehaviour
{
    public static CatlasManager instance; // Singleton instance

    public Text soleilStatusText;
    public Text luneStatusText;
    public Button sendSoleilToTownButton;
    public Button sendSoleilToWoodWoodsButton;
    public Button sendLuneToTownButton;
    public Button sendLuneToWoodWoodsButton;
    public Text timeOfDayText; // Text to display the current time of day

    private bool soleilOnMission = false;
    private bool luneOnMission = false;
    private float soleilMissionTime;
    private float luneMissionTime;
    private float soleilMissionDuration = 300f; // 5 minutes for example
    private float luneMissionDuration = 300f;   // 5 minutes for example

    private string soleilSelectedArea;
    private string luneSelectedArea;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Ensure a single instance
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        sendSoleilToTownButton.onClick.AddListener(() => SendCatOnMission("Soleil", "Town"));
        sendSoleilToWoodWoodsButton.onClick.AddListener(() => SendCatOnMission("Soleil", "WoodWoods"));
        sendLuneToTownButton.onClick.AddListener(() => SendCatOnMission("Lune", "Town"));
        sendLuneToWoodWoodsButton.onClick.AddListener(() => SendCatOnMission("Lune", "WoodWoods"));
        UpdateTimeOfDay(); // Initialize the time of day display
    }

    private void Update()
    {
        UpdateMissionStatus();
    }

    public void SendCatOnMission(string catName, string area)
    {
        if (catName == "Soleil" && !soleilOnMission)
        {
            soleilOnMission = true;
            soleilMissionTime = Time.time + soleilMissionDuration;
            soleilStatusText.text = $"Soleil is on a mission to {area}!";
            soleilSelectedArea = area;
        }
        else if (catName == "Lune" && !luneOnMission)
        {
            luneOnMission = true;
            luneMissionTime = Time.time + luneMissionDuration;
            luneStatusText.text = $"Lune is on a mission to {area}!";
            luneSelectedArea = area;
        }
    }

    private void UpdateMissionStatus()
    {
        if (soleilOnMission && Time.time >= soleilMissionTime)
        {
            soleilOnMission = false;
            ReceiveMissionRewards("Soleil", soleilSelectedArea);
            soleilStatusText.text = "Soleil has returned!";
        }

        if (luneOnMission && Time.time >= luneMissionTime)
        {
            luneOnMission = false;
            ReceiveMissionRewards("Lune", luneSelectedArea);
            luneStatusText.text = "Lune has returned!";
        }
    }

    private void ReceiveMissionRewards(string catName, string selectedArea)
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
