using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public float volume;
    public int fontSize;
    public float brightness;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.SetInt("FontSize", fontSize);
        PlayerPrefs.SetFloat("Brightness", brightness);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        volume = PlayerPrefs.GetFloat("Volume", 0.1f);
        fontSize = PlayerPrefs.GetInt("FontSize", 14);
        brightness = PlayerPrefs.GetFloat("Brightness", 1f);
    }

    public void SetBrightness(float newBrightness)
    {
        brightness = newBrightness;
        BrightnessController.Instance.SetBrightness(newBrightness);
        SaveSettings();
    }

}
