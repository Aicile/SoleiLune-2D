using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class OptionsManager : MonoBehaviour
{
    public Slider volumeSlider;
    public TMP_Dropdown fontSizeDropdown;
    public Slider brightnessSlider;
    public AudioSource gameAudio;
    public Text gameText;
    public AudioSource MusicAudioSource;
    public List<TextMeshProUGUI> allGameTexts = new List<TextMeshProUGUI>();
    public Image brightnessOverlay;


    private void Start()
    {
        SettingsManager.Instance.LoadSettings();

        volumeSlider.value = SettingsManager.Instance.volume;
        fontSizeDropdown.value = fontSizeDropdown.options.FindIndex(option => option.text == SettingsManager.Instance.fontSize.ToString());
        brightnessSlider.value = SettingsManager.Instance.brightness;

        ApplySettings();
    }

    public void OnVolumeChange()
    {
        SettingsManager.Instance.volume = volumeSlider.value;
        gameAudio.volume = volumeSlider.value;
    }

    public void OnFontSizeChange()
    {
        SettingsManager.Instance.fontSize = int.Parse(fontSizeDropdown.options[fontSizeDropdown.value].text);
        gameText.fontSize = SettingsManager.Instance.fontSize;
    }

    public void OnBrightnessChange()
    {
        // Implementer lysforandringer her hvis det trengs evt
    }

    public void ApplySettings()
    {
        gameAudio.volume = SettingsManager.Instance.volume;
        gameText.fontSize = SettingsManager.Instance.fontSize;
       
    }

    public void SaveSettingsButton()
    {
        SettingsManager.Instance.SaveSettings();
    }

    public void AdjustVolume(float volume)
    {
        MusicAudioSource.volume = volume;
    }

    public void AdjustFontSize(int dropdownValue)
    {
        
        float[] sizes = { 24f, 32f, 48f };

        foreach (TextMeshProUGUI text in allGameTexts)
        {
            text.fontSize = sizes[dropdownValue];
        }
    }

    public void AdjustBrightness(float value)
    {
       
        float alpha = 1f - value;
        Color currentColor = brightnessOverlay.color;
        brightnessOverlay.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
    }
}
