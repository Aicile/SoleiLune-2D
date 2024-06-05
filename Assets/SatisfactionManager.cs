using UnityEngine;
using UnityEngine.UI;

public class SatisfactionManager : MonoBehaviour
{
    public static SatisfactionManager instance; // Singleton instance

    public Slider satisfactionSlider;
    private int satisfactionPoints;
    private int maxSatisfactionPoints = 100;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy if another instance already exists
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the satisfaction manager across scenes
        }
    }

    private void Start()
    {
        UpdateSatisfactionSlider();
    }

    public void AddSatisfactionPoints(int points)
    {
        satisfactionPoints += points;
        satisfactionPoints = Mathf.Clamp(satisfactionPoints, 0, maxSatisfactionPoints);
        UpdateSatisfactionSlider();
    }

    private void UpdateSatisfactionSlider()
    {
        if (satisfactionSlider != null)
        {
            satisfactionSlider.value = (float)satisfactionPoints / maxSatisfactionPoints;
        }
    }
}




