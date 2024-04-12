using UnityEngine;
using UnityEngine.SceneManagement;

public class BedroomToGarden : MonoBehaviour
{
    private bool isPlayerNear = false;

    private void Update()
    {
        
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            SavePlayerPositionAndLoadGardenScene();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }

    private void SavePlayerPositionAndLoadGardenScene()
    {
        
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerPositionManager.SavePlayerPosition(playerTransform.position);

        print("Go to Garden");
        
        SceneManager.LoadScene("GardenScene");
    }
}
