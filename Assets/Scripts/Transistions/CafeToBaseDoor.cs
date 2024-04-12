using UnityEngine;

public class CafeToBaseDoor : MonoBehaviour
{
    private bool isPlayerNear = false;
    
    public SceneFader sceneFader;

    private void Update()
    {
       
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            LoadBasementScene();
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

    private void LoadBasementScene()
    {
        
        if (isPlayerNear)
        {
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            PlayerPositionManager.SavePlayerPosition(playerTransform.position);
        }

        Debug.Log("Go to basement");
  
        sceneFader.FadeToScene("BasementScene");
    }
}
