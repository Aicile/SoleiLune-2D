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

    private void SpawnInitialIngredients()
    {
        Canvas canvas = FindObjectOfType<Canvas>(); // Find the Canvas in the scene

        if (canvas == null)
        {
            Debug.LogError("No Canvas found in the scene. Please ensure there is a Canvas in the scene.");
            return;
        }

        foreach (var prefab in ingredientPrefabs)
        {
            if (prefab != null)
            {
                var ingredient = Instantiate(prefab, canvas.transform); // Instantiate as a child of the Canvas
                if (ingredient != null)
                {
                    activeIngredients.Add(ingredient);
                    originalPositions[ingredient] = ingredient.transform.position;
                    ingredient.SetActive(CheckIngredientAvailability(ingredient.GetComponent<DraggableIngredient>().ingredientID));
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

    public void ResetIngredients()
    {
        foreach (var ingredient in activeIngredients)
        {
            if (originalPositions.ContainsKey(ingredient))
            {
                ingredient.transform.position = originalPositions[ingredient]; // Reset to the original position
                ingredient.SetActive(CheckIngredientAvailability(ingredient.GetComponent<DraggableIngredient>().ingredientID));  // Reactivate the ingredient based on availability
            }
        }
    }

    private bool CheckIngredientAvailability(int ingredientID)
    {
        switch (ingredientID)
        {
            case 1:
                return JournalManager.instance.CheckIngredientStock("Coffee Bean");
            case 2:
                return JournalManager.instance.CheckIngredientStock("Rose");
            case 3:
                return JournalManager.instance.CheckIngredientStock("Lilac");
            case 4:
                return JournalManager.instance.CheckIngredientStock("Lavender");
            case 5:
                return JournalManager.instance.CheckIngredientStock("Crimson Lycoris");
            case 6:
                return JournalManager.instance.CheckIngredientStock("Red Bean");
            default:
                return false;
        }
    }
}
