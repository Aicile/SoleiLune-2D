using UnityEngine;
using System.Collections.Generic;

public class IngredientManager : MonoBehaviour
{
    public static IngredientManager Instance; // Singleton instance for global access

    public GameObject[] ingredientPrefabs; // Prefabs of the ingredients
    public Transform spawnPoint;           // Point where ingredients should spawn or reset to
    private List<GameObject> activeIngredients = new List<GameObject>();

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
            var ingredient = Instantiate(prefab, spawnPoint.position, Quaternion.identity, transform);
            activeIngredients.Add(ingredient);
        }
    }

    // Method to reset ingredients back to original position
    public void ResetIngredients()
    {
        foreach (var ingredient in activeIngredients)
        {
            ingredient.transform.position = spawnPoint.position; // Resets the position
            ingredient.SetActive(true);  // Reactivates the ingredient in case it was deactivated
        }
    }
}
