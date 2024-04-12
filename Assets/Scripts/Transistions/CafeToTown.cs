using UnityEngine;
using UnityEngine.SceneManagement;

public class CafeToTown : MonoBehaviour
{
    private bool isPlayerNear = false;

    private void Update()
    {
        
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            SavePlayerPositionAndLoadTownArea();
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

    private void SavePlayerPositionAndLoadTownArea()
    {
       
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerPositionManager.SavePlayerPosition(playerTransform.position);

        Debug.Log("Go to Town Area");
       
        SceneManager.LoadScene("TownArea");
    }
}
