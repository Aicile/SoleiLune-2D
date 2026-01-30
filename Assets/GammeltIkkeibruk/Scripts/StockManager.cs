using UnityEngine;

public class StockManager : MonoBehaviour
{
    public static StockManager instance; // Singleton instance

    // Ingredient counts
    public int coffeeBeanCount;
    public int roseCount;
    public int lilacCount;
    public int lavenderCount;
    public int crimsonLycorisCount;
    public int redBeanCount;

    // Potion counts
    public int healthPotionCount;
    public int manaPotionCount;
    public int energyPotionCount;

    // Star points
    private int starPoints;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy if another instance already exists
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the stock manager across scenes
        }
    }

    void Start()
    {
        UpdateShelves(); // Ensure stock text is updated on start
    }

    public void UpdatePotionStock(string potionType, int change)
    {
        switch (potionType)
        {
            case "Health":
                healthPotionCount += change;
                break;
            case "Mana":
                manaPotionCount += change;
                break;
            case "Energy":
                energyPotionCount += change;
                break;
        }
        UpdateShelves(); // Update shelves after changing stock
    }

    public void UpdateIngredientStock(string ingredientType, int change)
    {
        switch (ingredientType)
        {
            case "Coffee Bean":
                coffeeBeanCount += change;
                break;
            case "Rose":
                roseCount += change;
                break;
            case "Lilac":
                lilacCount += change;
                break;
            case "Lavender":
                lavenderCount += change;
                break;
            case "Crimson Lycoris":
                crimsonLycorisCount += change;
                break;
            case "Red Bean":
                redBeanCount += change;
                break;
        }
        UpdateShelves(); // Update shelves after changing stock
    }

    public bool CheckPotionStock(string potionType)
    {
        switch (potionType)
        {
            case "Health":
                return healthPotionCount > 0;
            case "Mana":
                return manaPotionCount > 0;
            case "Energy":
                return energyPotionCount > 0;
            default:
                return false;
        }
    }

    public bool CheckIngredientStock(string ingredientType)
    {
        switch (ingredientType)
        {
            case "Coffee Bean":
                return coffeeBeanCount > 0;
            case "Rose":
                return roseCount > 0;
            case "Lilac":
                return lilacCount > 0;
            case "Lavender":
                return lavenderCount > 0;
            case "Crimson Lycoris":
                return crimsonLycorisCount > 0;
            case "Red Bean":
                return redBeanCount > 0;
            default:
                return false;
        }
    }

    public void UpdateStarPoints(int points)
    {
        starPoints += points;
        Debug.Log($"Star Points: {starPoints}");
        // Add logic to handle star level changes if needed
    }

    private void UpdateShelves()
    {
        IngredientShelf[] ingredientShelves = FindObjectsOfType<IngredientShelf>();
        PotionShelf[] potionShelves = FindObjectsOfType<PotionShelf>();

        foreach (IngredientShelf shelf in ingredientShelves)
        {
            shelf.UpdateShelf();
        }

        foreach (PotionShelf shelf in potionShelves)
        {
            shelf.UpdateShelf();
        }
    }
}
