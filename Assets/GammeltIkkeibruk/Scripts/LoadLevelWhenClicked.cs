using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelWhenClicked : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform != null)
        {
            PlayerPositionManager.SavePlayerPosition(playerTransform.position);
        }

        
        SceneManager.LoadScene(sceneName);
    }
}
