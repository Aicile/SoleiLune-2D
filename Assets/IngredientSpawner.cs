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

        // Choose a random ingredient to spawn
        GameObject randomIngredient = ingredientPrefabs[Random.Range(0, ingredientPrefabs.Length)];

        // Instantiate the ingredient as a child of the Canvas
        Instantiate(randomIngredient, spawnPosition, Quaternion.identity, canvas.transform);
    }
}
