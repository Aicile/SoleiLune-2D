using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPositionManager : MonoBehaviour
{
    private static string PositionKeyPrefix = "PlayerPosition_";

    public static void SavePlayerPosition(Vector3 position)
    {
        // Create a unique key for the scene
        string sceneKey = PositionKeyPrefix + SceneManager.GetActiveScene().name;
        PlayerPrefs.SetFloat(sceneKey + "_x", position.x);
        PlayerPrefs.SetFloat(sceneKey + "_y", position.y);
        PlayerPrefs.SetFloat(sceneKey + "_z", position.z);
        PlayerPrefs.Save();
    }

    public static Vector3? LoadPlayerPosition()
    {
        string sceneKey = PositionKeyPrefix + SceneManager.GetActiveScene().name;
        if (PlayerPrefs.HasKey(sceneKey + "_x") && PlayerPrefs.HasKey(sceneKey + "_y") && PlayerPrefs.HasKey(sceneKey + "_z"))
        {
            float x = PlayerPrefs.GetFloat(sceneKey + "_x");
            float y = PlayerPrefs.GetFloat(sceneKey + "_y");
            float z = PlayerPrefs.GetFloat(sceneKey + "_z");
            return new Vector3(x, y, z);
        }
        else
        {
            // No saved position found
            return null;
        }
     }
}
