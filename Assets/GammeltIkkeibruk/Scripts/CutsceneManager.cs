using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    // The name of the scene to load
    public string sceneToLoad = "CafeScene"; // Replace with the actual name of your cafe scene
    // The time to wait before loading the scene
    public float delay = 46.0f;

    void Start()
    {
        // Start the coroutine to load the scene after a delay
        StartCoroutine(LoadSceneAfterDelay());
    }

    IEnumerator LoadSceneAfterDelay()
    {
        // Wait for the specified amount of time
        yield return new WaitForSeconds(delay);
        // Load the specified scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
