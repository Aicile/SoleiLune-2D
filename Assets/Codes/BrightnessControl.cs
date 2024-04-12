using UnityEngine;
using UnityEngine.UI;

public class BrightnessController : MonoBehaviour
{
    public static BrightnessController Instance;
    private Image brightnessOverlay;

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

    public void SetBrightness(float brightness)
    {
        if (brightnessOverlay == null)
        {
            FindBrightnessOverlay();
        }

        if (brightnessOverlay != null)
        {
            Color c = brightnessOverlay.color;
            c.a = 1 - brightness;
            brightnessOverlay.color = c;
        }
    }

    
    public void FindBrightnessOverlay()
    {
        
        GameObject overlayObject = GameObject.FindWithTag("BrightnessOverlay");
        if (overlayObject != null)
        {
            brightnessOverlay = overlayObject.GetComponent<Image>();
        }
    }
}
