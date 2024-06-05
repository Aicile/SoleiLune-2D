using UnityEngine;

public class PersistentCanvas : MonoBehaviour
{
    private static PersistentCanvas instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy if another instance already exists
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the canvas across scenes
        }
    }
}
