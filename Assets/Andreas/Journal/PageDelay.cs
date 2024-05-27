using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PageDelay : MonoBehaviour
{
    public void DisablePage()
    {
        Invoke("ActualDisable", 0.9f);
    }

    private void ActualDisable()
    {
        this.gameObject.SetActive(false);
    }

    public void EnablePage()
    {
        Invoke("ActualEnable", 0.9f);
    }

    private void ActualEnable()
    {
        this.gameObject.SetActive(true);
    }
}
