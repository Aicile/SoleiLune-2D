using UnityEngine;
using UnityEngine.UI;
using System; // For Action

public class TimingMinigameTwo : MonoBehaviour
{
    public Slider timingSlider; // Assign in the inspector, represents the moving marker
    public RectTransform successZone; // Assign in the inspector, represents the area for a successful hit
    public int requiredSuccesses = 3; // The number of successful hits required to complete the minigame
    private int currentSuccesses = 0; // Current count of successful hits

    public float markerSpeed = 2.0f; // Speed at which the marker moves
    private bool isMarkerMovingRight = true; // Direction of the marker movement

    private Action onMinigameSuccess; // Callback for when the minigame is successfully completed
    private Action onMinigameFail; // Callback for when the player fails the minigame

    // Start or reset the minigame
    public void StartMinigame(Action onSuccess, Action onFail)
    {
        gameObject.SetActive(true);
        timingSlider.gameObject.SetActive(true); // Enable the minigame UI
        timingSlider.value = 0; // Start at the beginning of the slider
        currentSuccesses = 0; // Reset the success count
        onMinigameSuccess = onSuccess;
        onMinigameFail = onFail;
    }

    void Update()
    {
        // Move the marker back and forth within the bounds of the slider
        if (isMarkerMovingRight)
        {
            timingSlider.value += Time.deltaTime * markerSpeed;
            if (timingSlider.value >= timingSlider.maxValue)
            {
                isMarkerMovingRight = false;
            }
        }
        else
        {
            timingSlider.value -= Time.deltaTime * markerSpeed;
            if (timingSlider.value <= timingSlider.minValue)
            {
                isMarkerMovingRight = true;
            }
        }

        // Check for player input
        if (Input.GetKeyDown(KeyCode.Space)) // Use space bar or any key you prefer
        {
            CheckForSuccess();
        }
    }

    // Check if the marker is within the success zone
    private void CheckForSuccess()
    {
        // Check the slider's fill area rather than the whole slider range
        var sliderFillArea = timingSlider.fillRect;
        var sliderValue = timingSlider.value / timingSlider.maxValue; // Normalize the value to be between 0 and 1

        if (sliderValue >= sliderFillArea.anchorMin.x && sliderValue <= sliderFillArea.anchorMax.x)
        {
            currentSuccesses++;
            if (currentSuccesses >= requiredSuccesses)
            {
                onMinigameSuccess?.Invoke();
                gameObject.SetActive(false); // Hide the minigame UI
            }
        }
        else
        {
            onMinigameFail?.Invoke();
            gameObject.SetActive(false); // Hide the minigame UI
        }
    }

}
