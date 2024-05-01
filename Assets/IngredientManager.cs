using UnityEngine;
using System.Collections.Generic;

public class IngredientManager : MonoBehaviour
{
    public static IngredientManager Instance; // Singleton instance for global access

    public GameObject[] ingredientPrefabs; // Prefabs of the ingredients
    private List<GameObject> activeIngredients = new List<GameObject>(); // Active ingredients in the scene
    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>(); // Original positions of ingredients

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); // Optional: Keep alive across scenes
        }
    }

    void Start()
    {
        SpawnInitialIngredients();
    }

    // Spawns initial set of ingredients
    private void SpawnInitialIngredients()
    {
        foreach (var prefab in ingredientPrefabs)
        {
            if (prefab != null)
            {
                var ingredient = Instantiate(prefab, transform);
                if (ingredient != null)
                {
                    activeIngredients.Add(ingredient);
                    originalPositions[ingredient] = ingredient.transform.position;
                }
                else
                {
                    Debug.LogError("Failed to instantiate ingredient prefab.");
                }
            }
            else
            {
                Debug.LogError("Ingredient prefab is null.");
            }
        }
    }


    // Method to reset ingredients back to their original position
    public void ResetIngredients()
    {
        foreach (var ingredient in activeIngredients)
        {
            if (originalPositions.ContainsKey(ingredient))
            {
                ingredient.transform.position = originalPositions[ingredient]; // Reset to the original position
                ingredient.SetActive(true);  // Reactivate the ingredient in case it was deactivated
            }
        }
    }
}
