using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuggyCafeEffect : MonoBehaviour
{
    public GameObject[] furniture;
    public Image[] uiElements;
    public Light[] lights;

    private void Start()
    {
        ApplyBuggyEffects();
    }

    private void ApplyBuggyEffects()
    {
        // Randomly reposition, resize, and recolor furniture
        foreach (GameObject item in furniture)
        {
            item.transform.position += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            item.transform.localScale = new Vector3(Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f), 1);
            item.GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);
        }

        // Overlap UI elements and make them flicker
        foreach (Image uiElement in uiElements)
        {
            uiElement.transform.position += new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), 0);
            StartCoroutine(FlickerUIElement(uiElement));
        }

        // Make some lights flicker
        foreach (Light light in lights)
        {
            StartCoroutine(FlickerLight(light));
        }
    }

    private IEnumerator FlickerUIElement(Image uiElement)
    {
        while (true)
        {
            uiElement.enabled = !uiElement.enabled;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        }
    }

    private IEnumerator FlickerLight(Light light)
    {
        while (true)
        {
            light.enabled = !light.enabled;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        }
    }
}
