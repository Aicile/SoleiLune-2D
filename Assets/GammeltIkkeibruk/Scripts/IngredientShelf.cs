using UnityEngine;
using TMPro;

public class IngredientShelf : MonoBehaviour
{
    public TextMeshProUGUI coffeeBeansText;
    public TextMeshProUGUI rosesText;
    public TextMeshProUGUI lilacsText;
    public TextMeshProUGUI lavenderText;
    public TextMeshProUGUI crimsonLycorisText;
    public TextMeshProUGUI redBeansText;

    void Start()
    {
        UpdateShelf();
    }

    public void UpdateShelf()
    {
        coffeeBeansText.text = StockManager.instance.coffeeBeanCount.ToString();
        rosesText.text = StockManager.instance.roseCount.ToString();
        lilacsText.text = StockManager.instance.lilacCount.ToString();
        lavenderText.text = StockManager.instance.lavenderCount.ToString();
        crimsonLycorisText.text = StockManager.instance.crimsonLycorisCount.ToString();
        redBeansText.text = StockManager.instance.redBeanCount.ToString();
    }
}
