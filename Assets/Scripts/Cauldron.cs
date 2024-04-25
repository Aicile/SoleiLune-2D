using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Cauldron : MonoBehaviour, IDropHandler
{
    public List<int> ingredientIDs = new List<int>();
    public GameObject healthPotion;
    public GameObject manaPotion;
    public GameObject energyPotion;

    public TimingMinigameOne timingMinigameOne;
    public TimingMinigameTwo timingMinigameTwo;
    public StirringMinigame stirringMinigame;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableIngredient draggableIngredient = eventData.pointerDrag.GetComponent<DraggableIngredient>();
        if (draggableIngredient != null)
        {
            Debug.Log("Ingredient with ID " + draggableIngredient.ingredientID + " was dropped into the cauldron.");
            ingredientIDs.Add(draggableIngredient.ingredientID);

           
            if (ingredientIDs.Count == 2)
            {
                CraftPotion();
            }
        }
    }

    private void CraftPotion()
    {
        
        ingredientIDs.Sort();

        
        StartMinigamesBasedOnIngredients();
    }

    
    private void StartMinigamesBasedOnIngredients()
    {
        ClearMinigames();
        if (ingredientIDs.Contains(1) && ingredientIDs.Contains(2))
        {
            timingMinigameOne.StartMinigame(() => StartStirringMinigame(), FailCallback);
        }
        else if (ingredientIDs.Contains(3) && ingredientIDs.Contains(4))
        {
            timingMinigameTwo.StartMinigame(() => StartStirringMinigame(), FailCallback);
        }
        else if (ingredientIDs.Contains(5) && ingredientIDs.Contains(6))
        {
            timingMinigameOne.StartMinigame(() => timingMinigameTwo.StartMinigame(() => StartStirringMinigame(), FailCallback), FailCallback);
        }
        else
        {
            Debug.Log("Incorrect ingredients. No potion crafted.");
            
            ingredientIDs.Clear();
        }
    }

    private void StartStirringMinigame()
    {
        stirringMinigame.StartMinigame(FinalSuccess, FailCallback);
    }

    private void ClearMinigames()
    {
        timingMinigameOne.gameObject.SetActive(false);
        timingMinigameTwo.gameObject.SetActive(false);
        stirringMinigame.gameObject.SetActive(false);
    }

    private void FinalSuccess()
    {
        ActivatePotionBasedOnIngredients();
        IngredientManager.Instance.ResetIngredients(); // Reset ingredients after crafting success
        ingredientIDs.Clear();
    }


    private void ActivatePotionBasedOnIngredients()
    {
        Debug.Log($"FinalSuccess called with ingredients: {string.Join(", ", ingredientIDs)}");

        if (ingredientIDs.Contains(1) && ingredientIDs.Contains(2))
        {
            JournalManager.instance.UpdatePotionStock("Health", 1);
            Debug.Log("Activating Health Potion.");
            healthPotion.SetActive(true);
        }
        else if (ingredientIDs.Contains(3) && ingredientIDs.Contains(4))
        {
            JournalManager.instance.UpdatePotionStock("Mana", 1);
            Debug.Log("Activating Mana Potion.");
            manaPotion.SetActive(true);
        }
        else if (ingredientIDs.Contains(5) && ingredientIDs.Contains(6))
        {
            JournalManager.instance.UpdatePotionStock("Energy", 1);
            Debug.Log("Activating Energy Potion.");
            energyPotion.SetActive(true);
        }
        else
        {
            Debug.Log("No matching potion for the ingredient combination.");
        }
    }

    private void FailCallback()
    {
        Debug.Log("Failed to craft potion. Try again.");
        IngredientManager.Instance.ResetIngredients(); // Reset ingredients after crafting failure
        ingredientIDs.Clear();
    }
}
