using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Using TextMeshPro for the clock display
using UnityEngine.Rendering; // Used to access the volume component
using UnityEngine.SceneManagement; // Used for scene management

public class DayNightScript : MonoBehaviour
{
    public static DayNightScript Instance { get; private set; } // Singleton instance

    private TextMeshProUGUI timeDisplay; // Display Time
    private TextMeshProUGUI dayDisplay; // Display Day
    public Volume ppv; // This is the post processing volume

    public float tick; // Increasing the tick, increases second rate
    private float seconds;
    private int mins;
    private int hours;
    private int days = 1;

    public bool activateLights; // Checks if lights are on
    public GameObject[] lights; // All the lights we want on when its dark
    public SpriteRenderer[] stars; // Star sprites 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make this GameObject persistent
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe when destroyed
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update ppv reference
        if (ppv == null)
        {
            Volume newPPV = FindObjectOfType<Volume>();
            if (newPPV != null)
            {
                ppv = newPPV;
            }
        }

        // Find and update the timeDisplay and dayDisplay references
        timeDisplay = GameObject.FindWithTag("TimeDisplay")?.GetComponent<TextMeshProUGUI>();
        dayDisplay = GameObject.FindWithTag("DayDisplay")?.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void FixedUpdate() // we used fixed update, since update is frame dependant. 
    {
        CalcTime();
        DisplayTime();

    }

    public void CalcTime() // Used to calculate sec, min and hours
    {
        seconds += Time.fixedDeltaTime * tick; // multiply time between fixed update by tick

        if (seconds >= 60) // 60 sec = 1 min
        {
            seconds = 0;
            mins += 1;
        }

        if (mins >= 60) //60 min = 1 hr
        {
            mins = 0;
            hours += 1;
        }

        if (hours >= 24) //24 hr = 1 day
        {
            hours = 0;
            days += 1;
        }
        ControlPPV(); // changes post processing volume after calculation
    }

    public void ControlPPV() // used to adjust the post processing slider.
    {
        //ppv.weight = 0;
        if (hours >= 21 && hours < 22) // dusk at 21:00 / 9pm    -   until 22:00 / 10pm
        {
            ppv.weight = (float)mins / 60; // since dusk is 1 hr, we just divide the mins by 60 which will slowly increase from 0 - 1 
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].color = new Color(stars[i].color.r, stars[i].color.g, stars[i].color.b, (float)mins / 60); // change the alpha value of the stars so they become visible
            }

            if (activateLights == false) // if lights havent been turned on
            {
                if (mins > 45) // wait until pretty dark
                {
                    for (int i = 0; i < lights.Length; i++)
                    {
                        lights[i].SetActive(true); // turn them all on
                    }
                    activateLights = true;
                }
            }
        }


        if (hours >= 6 && hours < 7) // Dawn at 6:00 / 6am    -   until 7:00 / 7am
        {
            ppv.weight = 1 - (float)mins / 60; // we minus 1 because we want it to go from 1 - 0
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].color = new Color(stars[i].color.r, stars[i].color.g, stars[i].color.b, 1 - (float)mins / 60); // make stars invisible
            }
            if (activateLights == true) // if lights are on
            {
                if (mins > 45) // wait until pretty bright
                {
                    for (int i = 0; i < lights.Length; i++)
                    {
                        lights[i].SetActive(false); // shut them off
                    }
                    activateLights = false;
                }
            }
        }
    }

    public void DisplayTime()
    {
        // Ensure timeDisplay and dayDisplay are not null before trying to update them
        if (timeDisplay != null)
        {
            timeDisplay.text = string.Format("{0:00}:{1:00}", hours, mins);
        }
        if (dayDisplay != null)
        {
            dayDisplay.text = "Day: " + days;
        }
    }
}