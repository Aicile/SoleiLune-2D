using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Using TextMeshPro for the clock display
using UnityEngine.Rendering; // Used to access the volume component
using UnityEngine.SceneManagement; // Used for scene management

public class DayNightScript : MonoBehaviour
{
    public static DayNightScript Instance { get; private set; }

    private TextMeshProUGUI timeDisplay;
    private TextMeshProUGUI dayDisplay;
    public Volume ppv;

    public float tick;

    private static float seconds = 0;
    private static int mins = 0;
    private static int hours = 8; // Start at 8 AM
    private static int days = 1;

    public bool activateLights;
    public GameObject[] lights;
    public SpriteRenderer[] stars;

    public float nightBrightness = 0.5f; // Adjust this value to make night brighter

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ppv = FindObjectOfType<Volume>();
        timeDisplay = GameObject.FindWithTag("TimeDisplay")?.GetComponent<TextMeshProUGUI>();
        dayDisplay = GameObject.FindWithTag("DayDisplay")?.GetComponent<TextMeshProUGUI>();
        DisplayTime(); // Update the display after loading a new scene
    }

    void FixedUpdate()
    {
        CalcTime();
        DisplayTime();
    }

    void CalcTime()
    {
        seconds += Time.fixedDeltaTime * tick;
        if (seconds >= 60)
        {
            seconds = 0;
            mins += 1;
        }

        if (mins >= 60)
        {
            mins = 0;
            hours += 1;
        }

        if (hours >= 24)
        {
            hours = 0;
            days += 1;
        }

        ControlPPV();
    }

    void ControlPPV()
    {
        if (hours >= 21 || hours < 6) // Define clear night hours
        {
            ppv.weight = nightBrightness; // Maintain a constant night brightness
            UpdateNightEnvironment(true);
        }
        else if (hours == 20) // Handle dusk transition
        {
            // Smooth transition into night starting from 8 PM to 9 PM
            ppv.weight = Mathf.Lerp(0f, nightBrightness, (float)(mins / 60.0));
            UpdateNightEnvironment(true);
        }
        else if (hours == 6) // Handle dawn transition
        {
            // Smooth transition out of night from 6 AM to 7 AM
            ppv.weight = Mathf.Lerp(nightBrightness, 0f, (float)(mins / 60.0));
            UpdateNightEnvironment(ppv.weight > 0.5 * nightBrightness);
        }
        else // Daytime
        {
            ppv.weight = 0; // Ensure day has no post processing weight
            UpdateNightEnvironment(false);
        }
    }

    void UpdateNightEnvironment(bool isNight)
    {
        foreach (var star in stars)
        {
            star.color = new Color(star.color.r, star.color.g, star.color.b, isNight ? nightBrightness : 0);
        }
        foreach (var light in lights)
        {
            light.SetActive(isNight);
        }
        activateLights = isNight;
    }

    void DisplayTime()
    {
        if (timeDisplay != null)
            timeDisplay.text = string.Format("{0:00}:{1:00}", hours, mins);
        if (dayDisplay != null)
            dayDisplay.text = "Day: " + days;
    }
}
