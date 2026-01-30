using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToNextScene : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(NextScene());
    }

    
    void Update()
    {

    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(8f);
        SceneManager.LoadScene(0);
    }
}
