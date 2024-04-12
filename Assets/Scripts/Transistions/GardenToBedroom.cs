using UnityEngine;
using UnityEngine.SceneManagement;

public class GardenToBedroom : MonoBehaviour
{
    private bool isPlayerNear = false;

    private void Update()
    {
        
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            SavePlayerPositionAndLoadBedroomScene();
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

    private void SavePlayerPositionAndLoadBedroomScene()
    {
       
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerPositionManager.SavePlayerPosition(playerTransform.position);

        print("Go to Bedroom");
        
        SceneManager.LoadScene("BedroomScene");
    }
}
