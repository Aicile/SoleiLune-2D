using UnityEngine;

public class RandomChairSpawner : MonoBehaviour
{
    public GameObject chairPrefab; // Assign the chair prefab in the inspector
    public int numberOfChairs = 20; // Number of chairs to spawn
    public Vector2 spawnAreaMin; // Minimum (x, y) of the spawn area
    public Vector2 spawnAreaMax; // Maximum (x, y) of the spawn area

    void Start()
    {
        SpawnRandomChairs();
    }

    void SpawnRandomChairs()
    {
        for (int i = 0; i < numberOfChairs; i++)
        {
            // Randomize position within the specified area
            float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector2 randomPosition = new Vector2(randomX, randomY);

            GameObject chair = Instantiate(chairPrefab, randomPosition, Quaternion.identity);

            // Randomize size
            float randomScale = Random.Range(0.5f, 1.5f);
            chair.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            // Randomize rotation
            float randomRotation = Random.Range(0, 360);
            chair.transform.rotation = Quaternion.Euler(0, 0, randomRotation);

            // Randomize color
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            chair.GetComponent<SpriteRenderer>().color = randomColor;
        }
    }
}
