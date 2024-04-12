using UnityEngine;
using UnityEngine.SceneManagement;

public class TownToCafe : MonoBehaviour
{
    private bool isPlayerNear = false;

    private void Update()
    {
        
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            SavePlayerPositionAndLoadCafeScene();
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

    private void SavePlayerPositionAndLoadCafeScene()
    {
        
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerPositionManager.SavePlayerPosition(playerTransform.position);

        Debug.Log("Go to cafe");
        
        SceneManager.LoadScene("CafeScene");
    }
}
