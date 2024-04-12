using UnityEngine;
using UnityEngine.UI;
using System; // Added for Action delegate

public class StirringMinigame : MonoBehaviour
{
    public Slider stirringProgressSlider; 
    public float stirringSpeed = 1f; 
    private bool isStirring = false; 
    private Action onSuccess; 
    private Action onFail; 

    
    public void StartMinigame(Action successCallback, Action failCallback)
    {
        gameObject.SetActive(true);
        
        stirringProgressSlider.value = 0;
        this.onSuccess = successCallback;
        this.onFail = failCallback;
        isStirring = false;

        
        stirringProgressSlider.gameObject.SetActive(true);
    }

    void Update()
    {
       
        if (onSuccess == null && onFail == null)
            return;

        
        CheckStirringInput();

        
        if (isStirring)
        {
            UpdateStirringProgress();
        }
    }

    private void CheckStirringInput()
    {
        
        if (Input.GetMouseButton(0) && Input.GetAxis("Mouse X") != 0) 
        {
            isStirring = true;
        }
        else
        {
            isStirring = false;
        }
    }

    private void UpdateStirringProgress()
    {
        
        if (isStirring)
        {
            stirringProgressSlider.value += Time.deltaTime * stirringSpeed;
            if (stirringProgressSlider.value >= stirringProgressSlider.maxValue)
            {
                
                PotionCompleted();
            }
        }
        else if (stirringProgressSlider.value > 0)
        {
           
            stirringProgressSlider.value -= Time.deltaTime * (stirringSpeed / 2); 
        }
    }

    private void PotionCompleted()
    {
        
        onSuccess?.Invoke();

       
        stirringProgressSlider.gameObject.SetActive(false);

        
        onSuccess = null;
        onFail = null;
    }
}
