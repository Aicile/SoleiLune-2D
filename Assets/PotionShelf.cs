using UnityEngine;
using TMPro;

public class PotionShelf : MonoBehaviour
{
    public TextMeshProUGUI healthPotionText;
    public TextMeshProUGUI manaPotionText;
    public TextMeshProUGUI energyPotionText;

    void Start()
    {
        UpdateShelf();
    }

    public void UpdateShelf()
    {
        healthPotionText.text = StockManager.instance.healthPotionCount.ToString();
        manaPotionText.text = StockManager.instance.manaPotionCount.ToString();
        energyPotionText.text = StockManager.instance.energyPotionCount.ToString();
    }
}
