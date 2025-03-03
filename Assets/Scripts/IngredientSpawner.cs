using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    public GameObject[] ingredientPrefabs; // Array of ingredient prefabs
    public float spawnInterval = 5f; // Time interval between spawns

    // Edge GameObjects
    public Transform top;
    public Transform bottom;
    public Transform left;
    public Transform right;

    private Canvas canvas; // Reference to the Canvas

    void Start()
    {
        canvas = FindObjectOfType<Canvas>(); // Find the Canvas in the scene

        if (canvas == null)
        {
            Debug.LogError("No Canvas found in the scene. Please ensure there is a Canvas in the scene.");
            return;
        }

        if (ingredientPrefabs.Length == 0)
        {
            Debug.LogError("No ingredient prefabs assigned. Please assign ingredient prefabs in the inspector.");
            return;
        }

        StartCoroutine(SpawnIngredients());
    }

    IEnumerator SpawnIngredients()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnRandomIngredient();
        }
    }

    void SpawnRandomIngredient()
    {
        // Generate a random position within the map bounds
        float minX = left.position.x;
        float maxX = right.position.x;
        float minY = bottom.position.y;
        float maxY = top.position.y;

        Vector2 spawnPosition = new Vector2(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY)
        );

        // Log the spawn position
        Debug.Log($"Spawning ingredient at position: {spawnPosition}");

        // Choose a random ingredient to spawn
        GameObject randomIngredient = ingredientPrefabs[Random.Range(0, ingredientPrefabs.Length)];

        // Log the ingredient being spawned
        Debug.Log($"Spawning ingredient: {randomIngredient.name}");

        // Instantiate the ingredient as a child of the Canvas
        GameObject spawnedIngredient = Instantiate(randomIngredient, spawnPosition, Quaternion.identity, canvas.transform);

        // Log the spawned ingredient
        if (spawnedIngredient != null)
        {
            Debug.Log($"Ingredient {spawnedIngredient.name} spawned successfully.");
        }
        else
        {
            Debug.LogError("Failed to spawn ingredient.");
        }
    }
}
