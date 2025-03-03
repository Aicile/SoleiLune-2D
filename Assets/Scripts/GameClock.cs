using UnityEngine;
using TMPro; // Ensure you're using TextMeshPro for the UI

public class GameClock : MonoBehaviour
{
    public TextMeshProUGUI clockText; // Assign your TextMeshProUGUI component in the inspector
    public Transform bedPosition; // Assign the bed's Transform in the inspector
    public GameObject player; // Assign the player GameObject in the inspector

    private float timeInSeconds; // Tracks the current time in seconds within the game
    private float totalCycleInSeconds = 900f; // 15 minutes * 60 seconds = 900 seconds for a full day cycle

    // Start and end times in hours (6 AM to 3 AM next day)
    private int startHour = 6;
    private int endHour = 27; // 3 AM the next day is represented as 27 hours

    void Update()
    {
        UpdateTime();
        UpdateClockDisplay();
        CheckForBedtime();
    }

    void UpdateTime()
    {
        // Increment time with real-world seconds multiplied by the desired speedup factor
        timeInSeconds += Time.deltaTime;

        // Loop the time if it exceeds the total cycle duration
        if (timeInSeconds > totalCycleInSeconds)
        {
            timeInSeconds = 0;
        }
    }

    void UpdateClockDisplay()
    {
        // Calculate the current hour in the game based on the time progression
        float hoursPassed = ((timeInSeconds / totalCycleInSeconds) * (endHour - startHour)) + startHour;
        int currentHour = (int)hoursPassed;
        int currentMinute = (int)((hoursPassed - currentHour) * 60);

        // Format and update the clock display
        clockText.text = string.Format("{0:00}:{1:00}", currentHour % 24, currentMinute);
    }

    void CheckForBedtime()
    {
        // Get the current hour in the game
        float hoursPassed = ((timeInSeconds / totalCycleInSeconds) * (endHour - startHour)) + startHour;
        int currentHour = (int)hoursPassed;

        // If it's past 3 AM, teleport the player to bed and reset the time to 6 AM
        if (currentHour >= 27) // 3 AM next day
        {
            TeleportToBed();
            ResetTimeToMorning();
        }
    }

    void TeleportToBed()
    {
        // Teleport the player to the bed's position
        player.transform.position = bedPosition.position;
    }
    void ResetTimeToMorning()
    {
        // Calculate the time in seconds corresponding to 6 AM
        float morningHourFraction = (6f - startHour) / (endHour - startHour);
        timeInSeconds = morningHourFraction * totalCycleInSeconds;
    }
}
