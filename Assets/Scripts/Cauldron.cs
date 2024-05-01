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

        // Check for health potion combination
        if (ingredientIDs.Contains(1) && ingredientIDs.Contains(2))
        {
            if (healthPotion != null)
            {
                JournalManager.instance.UpdatePotionStock("Health", 1);
                healthPotion.SetActive(true);
            }
            else
            {
                Debug.LogError("Health potion GameObject is not assigned in the inspector.");
            }
        }

        // Check for mana potion combination
        if (ingredientIDs.Contains(3) && ingredientIDs.Contains(4))
        {
            if (manaPotion != null)
            {
                JournalManager.instance.UpdatePotionStock("Mana", 1);
                manaPotion.SetActive(true);
            }
            else
            {
                Debug.LogError("Mana potion GameObject is not assigned in the inspector.");
            }
        }

        // Check for energy potion combination
        if (ingredientIDs.Contains(5) && ingredientIDs.Contains(6))
        {
            if (energyPotion != null)
            {
                JournalManager.instance.UpdatePotionStock("Energy", 1);
                energyPotion.SetActive(true);
            }
            else
            {
                Debug.LogError("Energy potion GameObject is not assigned in the inspector.");
            }
        }

        // Log if no matching combination is found
        if (!ingredientIDs.Contains(1) && !ingredientIDs.Contains(2) &&
            !ingredientIDs.Contains(3) && !ingredientIDs.Contains(4) &&
            !ingredientIDs.Contains(5) && !ingredientIDs.Contains(6))
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
