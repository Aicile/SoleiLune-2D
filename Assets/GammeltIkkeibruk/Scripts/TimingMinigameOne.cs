using UnityEngine;
using UnityEngine.UI;
using System; // For Action

public class TimingMinigameOne : MonoBehaviour
{
    public Slider chargeSlider; // Assign the slider in the inspector
    public Image targetZone; // An image representing the zone where the player should release the button
    public float chargeSpeed = 1f; // How fast the slider charges
    public float releaseWindow = 0.2f; // The size of the window for successful release, centered on the targetZone

    private bool isCharging = false; // Is the player currently charging?
    private bool isMinigameActive = false; // Is the minigame currently active?

    private Action onMinigameSuccess; // Callback for when the minigame is successfully completed
    private Action onMinigameFail; // Callback for when the player fails the minigame

    public void StartMinigame(Action onSuccess, Action onFail)
    {
        gameObject.SetActive(true);
        chargeSlider.gameObject.SetActive(true);
        chargeSlider.value = 0;
        onMinigameSuccess = onSuccess;
        onMinigameFail = onFail;
        isMinigameActive = true;
    }

    void Update()
    {
        if (isMinigameActive)
        {
            if (Input.GetMouseButtonDown(0)) // Assuming left click to start charging
            {
                isCharging = true;
            }

            if (isCharging)
            {
                chargeSlider.value += Time.deltaTime * chargeSpeed;
                if (chargeSlider.value >= chargeSlider.maxValue)
                {
                    // Failed by overcharge
                    FailMinigame();
                }
            }

            if (Input.GetMouseButtonUp(0) && isCharging)
            {
                isCharging = false;
                // Normalize the slider's value to a 0-1 range based on its actual maxValue
                float normalizedValue = chargeSlider.value / chargeSlider.maxValue;

                // Calculate the target start and end values based on the slider's width and targetZone's relative position
                float sliderWidth = chargeSlider.GetComponent<RectTransform>().rect.width;
                float targetZoneWidth = targetZone.rectTransform.rect.width;
                float targetZoneCenter = targetZone.rectTransform.anchoredPosition.x / sliderWidth;

                // Convert to slider value range
                float targetStartValue = (targetZoneCenter - (releaseWindow / 2)) * chargeSlider.maxValue;
                float targetEndValue = (targetZoneCenter + (releaseWindow / 2)) * chargeSlider.maxValue;

                Debug.Log($"Normalized Slider Value: {normalizedValue}, Target Start: {targetStartValue}, Target End: {targetEndValue}");

                if (chargeSlider.value > targetStartValue && chargeSlider.value < targetEndValue)
                {
                    onMinigameSuccess?.Invoke();
                }
                else
                {
                    FailMinigame();
                }

                EndMinigame();
            }

        }
    }

    private void FailMinigame()
    {
        Debug.Log("Minigame failed!");
        onMinigameFail?.Invoke();
    }

    private void EndMinigame()
    {
        Debug.Log("Ending Minigame");
        isMinigameActive = false;
        chargeSlider.gameObject.SetActive(false);
    }

}

