using UnityEngine;
using UnityEngine.SceneManagement;

public class CatlasSceneLoader : MonoBehaviour
{
    private bool playerInTriggerZone = false;

    void Update()
    {
        // Check if the player is in the trigger zone and the 'E' key is pressed
        if (playerInTriggerZone && Input.GetKeyDown(KeyCode.E))
        {
            LoadStockScene();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player has entered the trigger zone
        if (other.CompareTag("Player"))
        {
            playerInTriggerZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player has exited the trigger zone
        if (other.CompareTag("Player"))
        {
            playerInTriggerZone = false;
        }
    }

    private void LoadStockScene()
    {
        // Load the stock scene
        SceneManager.LoadScene("CatlasScene"); // Make sure "StockScene" is the name of your stock scene
    }
}
