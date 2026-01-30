using UnityEngine;
using UnityEngine.UI;
using System;

public class TimingMinigameTwo : MonoBehaviour
{
    public Slider timingSlider;
    public RectTransform successZone; // This is the UI element that visually represents the success zone
    public int requiredSuccesses = 3;
    private int currentSuccesses = 0;

    public float markerSpeed = 2.0f; // Speed at which the slider handle moves
    private bool isMarkerMovingRight = true; // Direction control for the slider handle movement

    private Action onMinigameSuccess; // Callback for successful minigame completion
    private Action onMinigameFail; // Callback for minigame failure

    public void StartMinigame(Action onSuccess, Action onFail)
    {
        gameObject.SetActive(true);
        timingSlider.gameObject.SetActive(true);
        timingSlider.value = 0;
        currentSuccesses = 0;
        onMinigameSuccess = onSuccess;
        onMinigameFail = onFail;
        MoveSuccessZone(); // Set the initial position for the success zone
    }

    void Update()
    {
        MoveMarker();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckForSuccess();
        }
    }

    private void MoveMarker()
    {
        float valueChange = Time.deltaTime * markerSpeed;
        if (isMarkerMovingRight)
        {
            timingSlider.value += valueChange;
            if (timingSlider.value >= timingSlider.maxValue)
                isMarkerMovingRight = false;
        }
        else
        {
            timingSlider.value -= valueChange;
            if (timingSlider.value <= timingSlider.minValue)
                isMarkerMovingRight = true;
        }
    }

    private void CheckForSuccess()
    {
        float handlePosition = timingSlider.value / timingSlider.maxValue;
        float successStart = successZone.anchorMin.x;
        float successEnd = successZone.anchorMax.x;

        if (handlePosition >= successStart && handlePosition <= successEnd)
        {
            currentSuccesses++;
            if (currentSuccesses < requiredSuccesses)
            {
                MoveSuccessZone();
            }
            else
            {
                onMinigameSuccess?.Invoke();
                gameObject.SetActive(false);
            }
        }
        else
        {
            onMinigameFail?.Invoke();
            gameObject.SetActive(false);
        }
    }

    private void MoveSuccessZone()
    {
        float newPosition = UnityEngine.Random.Range(0.1f, 0.9f); // New position within the slider
        float zoneWidth = 0.2f; // Width of the success zone as a percentage of the slider's width
        successZone.anchorMin = new Vector2(newPosition - zoneWidth / 2, 0);
        successZone.anchorMax = new Vector2(newPosition + zoneWidth / 2, 1);
    }
}
